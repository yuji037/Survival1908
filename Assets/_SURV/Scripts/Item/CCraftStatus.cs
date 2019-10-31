using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CCraftStatus
{
    public eCraftConditionType  eCraftConditionType;
	public CItemCountUnit 	    DstItemUnit;
	public CItemCountUnit[]     SrcItemUnitList;
}

[Serializable]
public class CItemCountUnit{
    public string   ItemName;
	public string 	ItemID;
	public int 		Count;

    public void CorrectItemInfo()
    {
        // 名前で検索
        CItemStatus status = CItemDataMan.Instance.GetItemStatusByName(ItemName);
        if (status == null)
        {
            // IDで検索
            status = CItemDataMan.Instance.GetItemStatusById(ItemID);
        }
        if (status == null)
        {
            Debug.LogError("存在しないアイテムです ID:" + ItemID + " Name:" + ItemName);
            return;
        }
        ItemID      = status.ID;
        ItemName    = status.Name;
    }
}

public enum eCraftConditionType
{
    None = 0,
    Bonfire = 10,
}