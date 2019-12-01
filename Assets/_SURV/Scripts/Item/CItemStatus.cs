using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CItemStatus
{
    public string       name;
	public string       id;
	public eItemType    itemType;
    public int          effectTypeID;
	public float        effectValue1;

	public void Use(){
        switch (itemType) {
            case eItemType.Food:
                CSoundMan.Instance.Play("SE_Eat00");
                CPartyStatus.Instance.GetPartyChara(0).GainFood(effectValue1);
                break;
            case eItemType.Weapon:
                CPartyStatus.Instance.GetPartyChara(0).Equip(EquipmentPart.Weapon, id);
                break;
			case eItemType.Armor:
				CPartyStatus.Instance.GetPartyChara(0).Equip(EquipmentPart.Body, id);
				break;
			case eItemType.Facility:
                //var ivPos = CPartyStatus.Instance.GetPartyPos();
                //var cFacility = new CFacility();
                //cFacility.type = (eFacilityType)effectTypeID;
                //CMapMan.Instance.SetMapFacility(ivPos.x, ivPos.y, cFacility);
                break;
        }
    }
}

public enum eItemType{
    Food,
    Weapon,
	NoUse,
    Facility,
    Armor,
    MAX,
}
