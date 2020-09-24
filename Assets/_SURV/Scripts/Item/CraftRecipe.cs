using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CraftRecipe
{
	public ItemCountUnit 		dstItemUnit;
	public ItemCountUnit[]		srcItemUnitList;
	public float				needTime;
	public CraftConditionType	craftConditionType;

	public string GetConditionName()
	{
		return GetConditionName(craftConditionType);
	}

	public static string GetConditionName(CraftConditionType condition)
	{
		switch (condition)
		{
			case CraftConditionType.None: return "なし";
			case CraftConditionType.Bonfire: return "たき火";
			default: return "エラー";
		}
	}
}

[Serializable]
public class ItemCountUnit{
    public string   itemName;
	public string 	itemID;
	public int 		count;

	public void CorrectItemInfo()
    {
        // 名前で検索
        ItemStatus status = ItemDataMan.Instance.GetItemStatusByName(itemName);
        if (status == null)
        {
            // IDで検索
            status = ItemDataMan.Instance.GetItemStatusById(itemID);
        }
        if (status == null)
        {
            Debug.LogError("存在しないアイテムです ID:" + itemID + " Name:" + itemName);
            return;
        }
        itemID      = status.ID;
        itemName    = status.Name;
    }
}

public enum CraftConditionType
{
    None = 0,
    Bonfire = 10,
}