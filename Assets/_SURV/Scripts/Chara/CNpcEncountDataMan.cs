using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNpcEncountDataMan : CSingleton<CNpcEncountDataMan>
{
    private CNpcEncountData m_cNpcEncountData;

    public CNpcEncountDataMan()
    {
        m_cNpcEncountData = Resources.Load<CNpcEncountData>("CNpcEncountData");
    }

    public CChara GetEncountNpc(int iEncountType)
    {
        var random = Random.Range(0f, 100f);
        var fRate = 0f;

        foreach(var unit in m_cNpcEncountData.m_pcNpcEncountStatus[iEncountType].NpcEncountUnits)
        {
            fRate += unit.RatePercent;
            if(random < fRate)
            {
                return CCharaDataMan.Instance.CreateByName(unit.NpcName);
            }
        }
        return null;
    }
}
