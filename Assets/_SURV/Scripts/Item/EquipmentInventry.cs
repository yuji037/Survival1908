using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentInventry : Singleton<EquipmentInventry>
{
	private const string SAVE_KEY = "EquipmentItems";

	private Dictionary<int, EquipmentItem> itemDict = new Dictionary<int, EquipmentItem>();
	private HashSet<int> equipmentIdTable = new HashSet<int>();

	public IReadOnlyDictionary<int, EquipmentItem> ItemDict { get => itemDict; }

	public void Add(string itemId)
	{
		if (!FindEquipmentId(out var id)) { return; }
		var newEquipment = new EquipmentItem(id, itemId, 0);
		Add(newEquipment);
	}

	private void Add(EquipmentItem item)
	{
		itemDict.Add(item.EquipmentId, item);
		equipmentIdTable.Add(item.EquipmentId);
	}

	/// <summary>
	/// ダブらないIDを取得
	/// </summary>
	private bool FindEquipmentId(out int id)
	{
		int attempt = 0;
		while(attempt < 10000)
		{
			id = Random.Range(0, int.MaxValue);
			if(false == equipmentIdTable.Contains(id))
			{
				return true;
			}
		}
		Debug.LogError($"EquipmentInventry ID取得に失敗しました。");
		id = -1;
		return false;
	}

	public void Remove(int equipmentId)
	{
		if (LocalPlayer.Instance.EquipmentModule.IsEquipping(equipmentId, out var part))
		{
			// 装備しているアイテムなら外す
			LocalPlayer.Instance.EquipmentModule.RemoveEquip(part);
		}
		itemDict.Remove(equipmentId);
		equipmentIdTable.Remove(equipmentId);
	}

	public List<int> GetHasEquipmentIds(string itemId)
	{
		var ret = new List<int>();
		foreach(var pair in itemDict)
		{
			if (pair.Value.ItemId == itemId)
			{
				ret.Add(pair.Key);
			}
		}
		return ret;
	}

	public void AddChangeStrengthPlus(int equipmentId, int delta)
	{
		itemDict[equipmentId].AddChangeStrengthPlus(delta);
		Save();
	}

	public void Save()
	{
		var itemList = new List<EquipmentItem>(itemDict.Values);
		SaveData.SetList(SAVE_KEY, itemList);
		SaveData.Save();
	}

	public void Load()
	{
		var itemList = SaveData.GetList(SAVE_KEY, new List<EquipmentItem>());
		foreach(var item in itemList)
		{
			if (itemDict.ContainsKey(item.EquipmentId))
			{
				Debug.LogError($"EquipmentInventry IDエラー EquipmentId:{item.EquipmentId} ItemID:{item.ItemId}");
				continue;
			}
			Add(item);
		}
	}
}
