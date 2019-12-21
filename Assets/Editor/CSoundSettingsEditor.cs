using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


[CustomEditor(typeof(CSoundSettings))]
public class CSoundSettingsEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var cT = target as CSoundSettings;

		if ( GUILayout.Button("Sort") )
		{
			cT.soundClips = cT.soundClips.OrderBy(sc => sc.id).ToArray();
			EditorUtility.SetDirty(cT);
		}

		base.OnInspectorGUI();
	}
}
