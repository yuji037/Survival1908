using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "SURV/CCraftData", fileName = "CCraftData" )]
public class CCraftData : ScriptableObject
{
	public CCraftStatus[] craftStatusList;
}
