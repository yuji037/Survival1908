using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SURV/CCharaItemDropData", fileName = "CCharaItemDropData")]
public class CCharaItemDropData : ScriptableObject
{
    public CCharaItemDropUnit[] charaItemDropUnits;
}

[System.Serializable]
public class CCharaItemDropUnit
{
	public string           charaName;
	public string           charaID;
	public CItemDropUnit[] itemDropUnits;
}

[System.Serializable]
public class CItemDropUnit
{
    public CItemCountUnit itemCountUnit;
	public float dropRatePercent;
}