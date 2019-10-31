﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCharaDataMan : CSingleton<CCharaDataMan>
{
    private Dictionary<string, CChara> m_dicChara = new Dictionary<string, CChara>();

    public CCharaDataMan()
    {
        var pcChara = Resources.Load<CCharaData>("CCharaData").m_pcCharas;
        foreach (var chara in pcChara)
        {
            m_dicChara[chara.Name] = chara;
        }
    }

    public CChara CreateByName(string sName)
    {
        foreach (var chara in m_dicChara.Values)
        {
            if (chara.Name == sName)
            {
                var newChara = chara.DeepClone();
                newChara.Hp = newChara.MaxHp;
                return newChara;
            }
        }
        Debug.LogError(sName + "という名前のキャラデータがありません");
        return null;
    }

    public CChara GetCharaDataByName(string sName)
    {
        if(false == m_dicChara.ContainsKey(sName))
        {
            Debug.LogError(sName + "という名前のキャラデータがありません");
            return null;
        }

        return m_dicChara[sName];
    }

    public CChara GetCharaDataByID(string sId)
    {
        foreach(var chara in m_dicChara.Values)
        {
            if (chara.ID == sId)
                return chara;
        }

        Debug.LogError(sId + "というIDのキャラデータがありません");
        return null;
    }
}