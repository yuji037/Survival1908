using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CCraftStatus
{
	public CItemCountUnit 	    dstItemUnit;
	public CItemCountUnit[]     srcItemUnitList;
	public eCraftConditionType  craftConditionType;
}

[Serializable]
public class CItemCountUnit{
    public string   itemName;
	public string 	itemID;
	public int 		count;

	public void CorrectItemInfo()
    {
        // 名前で検索
        CItemStatus status = CItemDataMan.Instance.GetItemStatusByName(itemName);
        if (status == null)
        {
            // IDで検索
            status = CItemDataMan.Instance.GetItemStatusById(itemID);
        }
        if (status == null)
        {
            Debug.LogError("存在しないアイテムです ID:" + itemID + " Name:" + itemName);
            return;
        }
        itemID      = status.id;
        itemName    = status.name;
    }
}

public enum eCraftConditionType
{
    None = 0,
    Bonfire = 10,
}