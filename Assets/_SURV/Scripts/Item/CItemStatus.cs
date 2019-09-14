﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CItemStatus
{
    public string ID;
    public string Name;
    public eItemType ItemType;
    public float EffectValue1;

    public void Use(){
        switch (ItemType) {
            case eItemType.Food:
                CPartyStatus.Instance.GetPartyChara(0).GainFood(EffectValue1);
                break;
            case eItemType.Weapon:
                CPartyStatus.Instance.GetPartyChara(0).EquipWeapon(ID);
                break;
            case eItemType.Facility:
                var ivPos = CPartyStatus.Instance.GetPartyPos();
                var cFacility = new CFacility();
                cFacility.eType = eFacilityType.Shelter;
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
    MAX,
}
