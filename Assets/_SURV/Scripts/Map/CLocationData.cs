using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "SURV/CLocationData", fileName = "CLocationData" )]
public class CLocationData : ScriptableObject
{
    public  CLocationStatus[]   locationStatusList;
}
