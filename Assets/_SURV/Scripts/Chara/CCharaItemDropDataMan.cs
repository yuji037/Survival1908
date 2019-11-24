using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCharaItemDropDataMan : CSingleton<CCharaItemDropDataMan>
{
    private Dictionary<string, CCharaItemDropUnit> m_dicCharaItemDropUnits = new Dictionary<string, CCharaItemDropUnit>();

    public CCharaItemDropDataMan()
    {

        var pcCharaItemDropUnits = Resources.Load<CCharaItemDropData>("CCharaItemDropData").charaItemDropUnits;
        foreach (var unit in pcCharaItemDropUnits)
        {
            m_dicCharaItemDropUnits[unit.charaName] = unit;
        }
    }

    public List<CItemCountUnit> GetDropItemsByCharaName(string sCharaName)
    {
        var list = new List<CItemCountUnit>();

        if (false == m_dicCharaItemDropUnits.ContainsKey(sCharaName))
            return list;

        var charaDropUnit = m_dicCharaItemDropUnits[sCharaName];
        foreach(var dropUnit in charaDropUnit.itemDropUnits)
        {
            var random = Random.Range(0f, 100f);
            if( random < dropUnit.dropRatePercent)
            {
                var countUnit = new CItemCountUnit();
                var countRate = Random.Range(0.7f, 1.3f);
                var count = dropUnit.itemCountUnit.count * countRate;
                if (count <= 0) continue;
                countUnit.count = Mathf.RoundToInt(count);
                countUnit.itemID = dropUnit.itemCountUnit.itemID;
                countUnit.itemName = dropUnit.itemCountUnit.itemName;
                list.Add(countUnit);
            }
        }

        return list;
    }
}

