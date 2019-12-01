using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "SURV/CSoundSettings", fileName = "CSoundSettings" )]
public class CSoundSettings : ScriptableObject
{
	[Range(0f, 1f)]
	public float masterVolume;

	public CSoundClip[] soundClips;
}
