using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


[CustomEditor(typeof(SoundSettings))]
public class SoundSettingsEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var cT = target as SoundSettings;

		if ( GUILayout.Button("Sort") )
		{
			cT.soundClips = cT.soundClips.OrderBy(sc => sc.id).ToArray();
			EditorUtility.SetDirty(cT);
		}

		base.OnInspectorGUI();
	}
}
