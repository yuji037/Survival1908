using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SURV/EffectSettings", fileName = "EffectSettings")]
public class EffectSettings : ScriptableObject
{
	public COneEffectSet[] effects;
}
