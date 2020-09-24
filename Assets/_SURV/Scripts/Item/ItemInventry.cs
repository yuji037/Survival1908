using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventry : Singleton<ItemInventry>
{
	private const string SAVE_KEY_HAS_ITEM_IDS		= "HasItemIds";
	private const string SAVE_KEY_HAS_ITEM_COUNTS	= "HasItemCounts";

	private Dictionary<string, int> itemHasCountDict = new Dictionary<string, int>();

	public Action OnChangedItemCount = null;

	public IReadOnlyDictionary<string, int> ItemHasCountDict { get { return itemHasCountDict; } }

	public void Load()
	{
		var hasItemIds = SaveData.GetList<string>(SAVE_KEY_HAS_ITEM_IDS, null);
		var hasItemCounts = SaveData.GetList<int>(SAVE_KEY_HAS_ITEM_COUNTS, null);
		if ( hasItemIds == null || hasItemCounts == null )
			return;

		for ( int i = 0; i < hasItemIds.Count; ++i )
		{
			itemHasCountDict[hasItemIds[i]] = hasItemCounts[i];
		}
	}

	public int GetHasItemCount(string itemId)
	{
		var itemStatus = ItemDataMan.Instance.GetItemStatusById(itemId);
		if (itemStatus.IsEquipment())
		{
			return EquipmentInventry.Instance.GetHasEquipmentIds(itemId).Count;
		}
		if ( false == itemHasCountDict.ContainsKey(itemId) )
		{
			return 0;
		}
		return itemHasCountDict[itemId];
	}

	public void AddChangeItemCount(string itemId, int delta)
	{
		var itemStatus = ItemDataMan.Instance.GetItemStatusById(itemId);
		UIInstantMessage.Instance.RequestMessage($"{(delta > 0 ? "獲得" : "消費")}：{itemStatus.Name} x {delta}");
		switch (itemStatus.Type)
		{
			case ItemType.Weapon:
			case ItemType.Armor:
				while(delta > 0)
				{
					EquipmentInventry.Instance.Add(itemId);
					delta--;
				}
				if(delta < 0)
				{
					var removeEquipmentIds = EquipmentInventry.Instance.GetHasEquipmentIds(itemId);
					if(removeEquipmentIds.Count < Mathf.Abs(delta))
					{
						Debug.LogError($"減少数＞所持数");
						return;
					}
					for (int i = 0; delta < 0; ++i)
					{
						EquipmentInventry.Instance.Remove(removeEquipmentIds[i]);
						delta++;
					}
				}
				EquipmentInventry.Instance.Save();
				return;
		}

		if ( false == itemHasCountDict.ContainsKey(itemId))
		{
			itemHasCountDict[itemId] = 0;
		}
		itemHasCountDict[itemId] += delta;

		OnChangedItemCount?.Invoke();

		SaveData.SetList(SAVE_KEY_HAS_ITEM_IDS,		itemHasCountDict.Keys.ToList());
		SaveData.SetList(SAVE_KEY_HAS_ITEM_COUNTS,	itemHasCountDict.Values.ToList());
		SaveData.Save();
	}

	public void ResetSave()
	{
		Debug.Log($"ResetSave {GetType().Name}");
		SaveData.Remove(SAVE_KEY_HAS_ITEM_IDS);
		SaveData.Remove(SAVE_KEY_HAS_ITEM_COUNTS);
		SaveData.Save();
	}

	public void UseItem(string itemID)
	{
		if(GetHasItemCount(itemID) <= 0 ) { return; }

		var itemStatus = ItemDataMan.Instance.GetItemStatusById(itemID);
		itemStatus.Use();
		AddChangeItemCount(itemID, -1);
	}
}
