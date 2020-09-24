using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu( menuName = "SURV/ItemData", fileName = "ItemData" )]
public class ItemData : ScriptableObject
{
    public  ItemStatus[]   itemStatusList;
}
