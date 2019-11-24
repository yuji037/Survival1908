using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "SURV/CSoundSettings", fileName = "CSoundSettings" )]
public class CSoundSettings : ScriptableObject
{
	public CSoundClip[] soundClips;
}
