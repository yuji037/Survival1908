using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class EquipmentItem
{
	[SerializeField] private int equipmentId;
	[SerializeField] private string itemId;
	[SerializeField] private int strengthPlus;

	private ItemStatus itemStatus = null;

	public int EquipmentId { get => equipmentId; }
	public string ItemId { get => itemId; }
	public int StrengthPlus { get => strengthPlus; }

	public EquipmentItem(int equipmentId, string itemId, int strengthPlus)
	{
		this.equipmentId = equipmentId;
		this.itemId = itemId;
		this.strengthPlus = strengthPlus;
	}

	public void AddChangeStrengthPlus(int delta)
	{
		strengthPlus += delta;
	}

	public ItemStatus GetItemStatus()
	{
		if(itemStatus == null)
		{
			itemStatus = ItemDataMan.Instance.GetItemStatusById(itemId);
		}
		return itemStatus;
	}

	public string GetName()
	{
		return $"{GetItemStatus().Name}{GetStrengthPlusText(strengthPlus)}";
	}

	public string GetDescription()
	{
		var itemStatus = GetItemStatus();
		var str = $"{GetName()}\n";
		var effectValue1 = itemStatus.EffectValue1 + strengthPlus;
		switch (itemStatus.Type)
		{
			case ItemType.Weapon:	str += $"攻撃力:+{effectValue1.ToString("f0")}";	break;
			case ItemType.Armor:	str += $"防御力:+{effectValue1.ToString("f0")}";	break;
		}
		return str;
	}

	public string GetStrengthPlusText(int strengthPlus)
	{
		var str = "";
		if (strengthPlus >= 1)
			str += "+" + strengthPlus;
		return str;
	}
}
