using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SURV/CNpcEncountData", fileName = "CNpcEncountData")]
public class CNpcEncountData : ScriptableObject
{
	public CNpcEncountStatus[] m_pcNpcEncountStatus;
}

[System.Serializable]
public class CNpcEncountStatus
{
	public string Name;
	public Color DebugMapColor;
	public CNpcEncountUnit[] NpcEncountUnits;
}

[System.Serializable]
public class CNpcEncountUnit
{
	public float RatePercent;
	public string NpcName;
}