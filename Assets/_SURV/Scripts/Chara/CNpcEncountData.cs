using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SURV/CNpcEncountData", fileName = "CNpcEncountData")]
public class CNpcEncountData : ScriptableObject
{
	public CNpcEncountStatus[] npcEncountStatusList;
}

[System.Serializable]
public class CNpcEncountStatus
{
	public string label;
	public Color debugMapColor;
	public CNpcEncountUnit[] npcEncountUnits;
}

[System.Serializable]
public class CNpcEncountUnit
{
	public float ratePercent;
	public string npcName;
}