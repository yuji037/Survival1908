using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu( menuName = "SURV/CItemData", fileName = "CItemData" )]
public class CItemData : ScriptableObject
{
    public  CItemStatus[]   itemStatusList;
}

[Serializable]
public class CItemSaveDataUnit
{
	public string itemID;
	public string count;
}