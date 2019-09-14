using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SURV/CCharaItemDropData", fileName = "CCharaItemDropData")]
public class CCharaItemDropData : ScriptableObject
{
    public CCharaItemDropUnit[] m_pcCharaItemDropUnits;
}

[System.Serializable]
public class CCharaItemDropUnit
{
    public string           CharaName;
    public string           CharaID;
    public CItemDropUnit[]  ItemDropUnits;
}

[System.Serializable]
public class CItemDropUnit
{
    public float DropRatePercent;
    public CItemCountUnit ItemCountUnit;
}