using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SURV/CEffectSettings", fileName = "CEffectSettings")]
public class CEffectSettings : ScriptableObject
{
	public COneEffectSet[] effects;
}
