using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu( menuName = "SURV/CItemData", fileName = "CItemData" )]
public class CItemData : ScriptableObject
{
    public  CItemStatus[]   m_pcItemStatus;
}
