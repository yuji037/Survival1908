using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CItemStatus
{
    public string       Name;
    public string       ID;
    public eItemType    ItemType;
    public int          EffectTypeID;
    public float        EffectValue1;

    public void Use(){
        switch (ItemType) {
            case eItemType.Food:
                CSoundMan.Instance.Play("SE_Eat00");
                CPartyStatus.Instance.GetPartyChara(0).GainFood(EffectValue1);
                break;
            case eItemType.Weapon:
                CPartyStatus.Instance.GetPartyChara(0).EquipWeapon(ID);
                break;
            case eItemType.Facility:
                var ivPos = CPartyStatus.Instance.GetPartyPos();
                var cFacility = new CFacility();
                cFacility.eType = (eFacilityType)EffectTypeID;
                CMapMan.Instance.SetMapFacility(ivPos.x, ivPos.y, cFacility);
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
