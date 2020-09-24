using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EquipmentModule
{
	private const int EMPTY = -1;
	private int[] equippingItems = new int[(int)EquipmentPart.MAX];

	public EquipmentModule()
	{
		for(int i = 0; i < equippingItems.Length; ++i)
		{
			equippingItems[i] = EMPTY;
		}
	}

	public void Equip(int equipmentId)
	{
		if(!EquipmentInventry.Instance.ItemDict.TryGetValue(equipmentId, out var equipment))
		{
			Debug.LogError($"IDに合致する装備が見つかりませんでした。 EquipmentId:{equipmentId}");
			return;
		}
		var itemStatus = ItemDataMan.Instance.GetItemStatusById(equipment.ItemId);
		EquipmentPart equipmentPart;
		switch (itemStatus.Type)
		{
			case ItemType.Weapon:	equipmentPart = EquipmentPart.Weapon;	break;
			case ItemType.Armor:	equipmentPart = EquipmentPart.Body;		break;
			default: Debug.LogError($"未対応の装備部位です。アイテム名:{itemStatus.Name}"); equipmentPart = EquipmentPart.Weapon; break;
		}
		equippingItems[(int)equipmentPart] = equipmentId;
	}

	public void RemoveEquip(EquipmentPart part)
	{
		equippingItems[(int)part] = EMPTY;
	}

	public bool IsEquipping(int equipmentId, out EquipmentPart equippingPart)
	{
		for(int i = 0; i < equippingItems.Length; ++i)
		{
			if(equippingItems[i] == equipmentId)
			{
				equippingPart = (EquipmentPart)i;
				return true;
			}
		}
		equippingPart = (EquipmentPart)(-1);
		return false;
	}

	public void GetEquipment(EquipmentPart part, out EquipmentItem equipment)
	{
		var id = equippingItems[(int)part];
		equipment = null;
		if (EMPTY == id) { return; }
		equipment = EquipmentInventry.Instance.ItemDict[id];
	}

	public string GetEquippingItemName(EquipmentPart part)
	{
		GetEquipment(part, out var equipment);
		return equipment?.GetName();
	}

	public float GetPlusAtk()
	{
		var atk = 0f;
		GetEquipment(EquipmentPart.Weapon, out var weaponEquip);
		if (null != weaponEquip) { atk += weaponEquip.GetItemStatus().EffectValue1 + weaponEquip.StrengthPlus; }
		return atk;
	}

	public float GetPlusDef()
	{
		var def = 0f;
		GetEquipment(EquipmentPart.Head, out var headEquip);
		GetEquipment(EquipmentPart.Body, out var bodyEquip);
		if (null != headEquip) { def += headEquip.GetItemStatus().EffectValue1 + headEquip.StrengthPlus; }
		if (null != bodyEquip) { def += bodyEquip.GetItemStatus().EffectValue1 + bodyEquip.StrengthPlus; }
		return def;
	}

	public string GetSaveKey(string charaId)
	{
		return $"{charaId}_Equip";
	}

	public void Save(string charaId)
	{
		var list = new List<int>(equippingItems);
		SaveData.SetList(GetSaveKey(charaId), list);
		SaveData.Save();
	}

	public void Load(string charaId)
	{
		var list = SaveData.GetList(GetSaveKey(charaId), new List<int>());
		foreach(var equipmentId in list)
		{
			if (equipmentId == EMPTY) { continue; }
			Equip(equipmentId);
		}
	}
}
