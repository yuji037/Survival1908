using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "SURV/SoundSettings", fileName = "SoundSettings" )]
public class SoundSettings : ScriptableObject
{
	[Range(0f, 1f)]
	public float masterVolume;

	public SoundClip[] soundClips;
}
