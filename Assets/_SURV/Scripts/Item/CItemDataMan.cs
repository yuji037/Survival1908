using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CItemDataMan : CSingleton<CItemDataMan>
{
    private Dictionary<string, CItemStatus>     m_dicItemStatus     = new Dictionary<string, CItemStatus>();

    public CItemDataMan(){

        var pcItemStatus = Resources.Load<CItemData>("CItemData").itemStatusList;
        foreach (var status in pcItemStatus) {
            m_dicItemStatus[status.id] = status;
        }
    }

    public CItemStatus GetItemStatusById(string sId)
    {
		if ( sId == null )
			return null;

        if (false == m_dicItemStatus.ContainsKey(sId))
            return null;
        return m_dicItemStatus[sId];
    }
    public CItemStatus GetItemStatusByName(string sName)
    {
        foreach(var status in m_dicItemStatus.Values)
        {
            if (status.name == sName)
                return status;
        }
        return null;
    }
}
