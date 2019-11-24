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

        foreach(var unit in m_cNpcEncountData.npcEncountStatusList[iEncountType].npcEncountUnits)
        {
            fRate += unit.ratePercent;
            Debug.Log("random : " + random.ToString("f0") + "\nfRate : " + fRate.ToString("f0"));
            if(random < fRate)
            {
                return CCharaDataMan.Instance.CreateByName(unit.npcName);
            }
        }
        return null;
    }
}
