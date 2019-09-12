using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPartyDetailMan : CSingletonMonoBehaviour<CPartyDetailMan>
{
    [SerializeField]
    private GameObject[] m_pcWindow;

    public void SwitchPartyDetail(int index)
    {
        if (m_pcWindow[index].activeSelf)
            return;

        for(int i = 0; i < m_pcWindow.Length; ++i)
        {
            m_pcWindow[i].SetActive(i == index);
        }
    }
}
