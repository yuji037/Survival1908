using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPartyChara : CBody
{
	public float combatExp;

    public float food;

	//public string GetEquipmentItemName(EquipmentPart equipmentPart)
	//{
	//	var id = currentEquipIds[(int)equipmentPart];
	//	if ( string.IsNullOrWhiteSpace(id) )
	//		return "";

	//	return CItemDataMan.Instance.GetItemStatusById(id).name;
	//}
	//   public override float GetAtk()
	//   {
	//   }
	//   public override float GetDef()
	//{
	//   }

	public override void ReceiveDamage(CAttackInfo attackInfo)
	{
		base.ReceiveDamage(attackInfo);

		CPartyStatus.Instance.UpdatePartyText();
	}

	public void Equip(EquipmentPart equipmentPart, string itemId){
    }

	public void GainCombatExp(float delta)
	{
		combatExp += delta;
		if(combatExp >= 50f )
		{
			combatExp = 0f;
			maxHp += 1;
			hp = maxHp;
		}
		CPartyStatus.Instance.UpdatePartyText();
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