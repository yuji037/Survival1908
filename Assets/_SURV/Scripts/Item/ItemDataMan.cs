using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataMan : Singleton<ItemDataMan>
{
    private Dictionary<string, ItemStatus>     itemStatusDict     = new Dictionary<string, ItemStatus>();

    public ItemDataMan(){

        var itemStatusList = Resources.Load<ItemData>("ItemData").itemStatusList;
        foreach (var status in itemStatusList) {
            itemStatusDict[status.ID] = status;
        }
    }

    public ItemStatus GetItemStatusById(string sId)
    {
		if ( sId == null )
			return null;

        if (false == itemStatusDict.ContainsKey(sId))
            return null;
        return itemStatusDict[sId];
    }
    public ItemStatus GetItemStatusByName(string sName)
    {
        foreach(var status in itemStatusDict.Values)
        {
            if (status.Name == sName)
                return status;
        }
        return null;
    }
}
