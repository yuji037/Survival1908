using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPartyChara : CChara
{
	private 	string[]	currentEquipIds;

    public CPartyChara(string _Name) : base(_Name)
    {
		currentEquipIds = new string[(int)EquipmentPart.MAX];
	}

    public float Food;

    public int Exp;
    public int Level;

	public string GetEquipmentItemName(EquipmentPart equipmentPart)
	{
		var id = currentEquipIds[(int)equipmentPart];
		if ( string.IsNullOrWhiteSpace(id) )
			return "";

		return CItemDataMan.Instance.GetItemStatusById(id).name;
	}
    public override float GetAtk()
    {
		var atkEquip = 0f;
		var weaponStatus = CItemDataMan.Instance.GetItemStatusById(currentEquipIds[(int)EquipmentPart.Weapon]);
		if ( weaponStatus != null )
			atkEquip = weaponStatus.effectValue1;
		return atkNaked + atkEquip;
    }
    public override float GetDef()
	{
		var defEquip = 0f;
		
		for(int i = (int)EquipmentPart.Head; i <= (int)EquipmentPart.Body; ++i )
		{
			var armorStatus = CItemDataMan.Instance.GetItemStatusById(currentEquipIds[i]);
			if ( armorStatus != null )
				defEquip += armorStatus.effectValue1;
		}
		return defNaked + defEquip;
    }

    public void GainExp(int iExp){
        Exp += iExp;
        var iPreLevel = Level;
        int iLeftExp;
        CEXPTable.Instance.GetLevel(Exp, out iLeftExp, out Level);
        if (iPreLevel != Level) {
            // レベルアップ
            maxHp = 50 + (Level - 1) * 2;
            atkNaked = 10 + (Level - 1) * 1;
            defNaked = 0 + (Level - 1) * 1;
        }
    }

    public void GainFood(float fFood){
        Food += fFood;
        if (Food > 100)
            Food = 100;
    }

    public void Equip(EquipmentPart equipmentPart, string itemId){
		currentEquipIds[(int)equipmentPart] = itemId;
        var itemStatus = CItemDataMan.Instance.GetItemStatusById(itemId);
    }
}

public enum EquipmentPart
{
	Weapon,
	Head,
	Body,
	Accessory1,
	Accessory2,
	MAX,
}