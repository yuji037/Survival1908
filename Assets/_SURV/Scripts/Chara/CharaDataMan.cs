using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaDataMan : Singleton<CharaDataMan>
{
    private Dictionary<string, Chara> m_dicChara = new Dictionary<string, Chara>();

    public CharaDataMan()
    {
        var pcChara = Resources.Load<CharaData>("CharaData").m_pcCharas;
        foreach (var chara in pcChara)
        {
            m_dicChara[chara.name] = chara;
        }
    }

    //public Chara CreateByName(string sName)
    //{
    //            var newChara = chara.DeepClone();
    //            newChara.hp = newChara.maxHp;
    //}

    public Chara GetCharaDataByName(string sName)
    {
        if(false == m_dicChara.ContainsKey(sName))
        {
            Debug.LogError(sName + "という名前のキャラデータがありません");
            return null;
        }

        return m_dicChara[sName];
    }

    //public Chara GetCharaDataByID(string sId)
    //{
    //    foreach(var chara in m_dicChara.Values)
    //    {
    //        if (chara.id == sId)
    //            return chara;
    //    }

    //    Debug.LogError(sId + "というIDのキャラデータがありません");
    //    return null;
    //}
}
