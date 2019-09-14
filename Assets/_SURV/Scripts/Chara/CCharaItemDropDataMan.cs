using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCharaItemDropDataMan : CSingleton<CCharaItemDropDataMan>
{
    private Dictionary<string, CCharaItemDropUnit> m_dicCharaItemDropUnits = new Dictionary<string, CCharaItemDropUnit>();

    public CCharaItemDropDataMan()
    {

        var pcCharaItemDropUnits = Resources.Load<CCharaItemDropData>("CCharaItemDropData").m_pcCharaItemDropUnits;
        foreach (var unit in pcCharaItemDropUnits)
        {
            m_dicCharaItemDropUnits[unit.CharaName] = unit;
        }
    }

    public List<CItemCountUnit> GetDropItemsByCharaName(string sCharaName)
    {
        var list = new List<CItemCountUnit>();

        if (false == m_dicCharaItemDropUnits.ContainsKey(sCharaName))
            return list;

        var charaDropUnit = m_dicCharaItemDropUnits[sCharaName];
        foreach(var dropUnit in charaDropUnit.ItemDropUnits)
        {
            var random = Random.Range(0f, 100f);
            if( random < dropUnit.DropRatePercent)
            {
                var countUnit = new CItemCountUnit();
                var countRate = Random.Range(0.7f, 1.3f);
                var count = dropUnit.ItemCountUnit.Count * countRate;
                countUnit.Count = Mathf.RoundToInt(count);
                countUnit.ItemID = dropUnit.ItemCountUnit.ItemID;
                countUnit.ItemName = dropUnit.ItemCountUnit.ItemName;
                list.Add(countUnit);
            }
        }

        return list;
    }
}

