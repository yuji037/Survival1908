using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SURV/CharaItemDropData", fileName = "CharaItemDropData")]
public class CharaItemDropData : ScriptableObject
{
    public CharaItemDropUnit[] charaItemDropUnits;
}

[System.Serializable]
public class CharaItemDropUnit
{
	public string           charaName;
	public string           charaID;
	public ItemRateUnit[]	itemDropUnits;
}

[System.Serializable]
public class ItemRateUnit
{
    public ItemCountUnit itemCountUnit;
	public float dropRatePercent;
}