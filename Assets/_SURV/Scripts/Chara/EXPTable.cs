using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPTable : Singleton<EXPTable>
{
    List<int> m_lsExpTable = new List<int>();

    public EXPTable(){
        int preNeedExp = 10;
        int totalExp = 0;

        for (int level = 0; level < 100; ++level) {
            if (level == 0)
                m_lsExpTable.Add(0);
            else if (level == 1) {
                totalExp = 10;
                m_lsExpTable.Add(totalExp);
            }
            else {
                var needExp = (int)(preNeedExp * 1.1f);
                totalExp += needExp;
                m_lsExpTable.Add(totalExp);
//                Debug.Log(level + " : " + totalExp);
                preNeedExp = needExp;
            }
        }
    }

    public void GetLevel(int iTotalExp, out int iLeftExp, out int iLevel){
        for (int level = 0; level < 100; ++level) {
            if (iTotalExp >= m_lsExpTable[level]) {

            }
            else {
                iLevel = level;
                iLeftExp = iTotalExp - m_lsExpTable[level];
                return;
            }
        }

        // 成長限界
        iLevel = 99;
        iLeftExp = iTotalExp - m_lsExpTable[99];
    }
}
