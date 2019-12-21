using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


[CustomEditor(typeof(CEffectSettings))]
public class CEffectSettingsEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var cT = target as CEffectSettings;

		if ( GUILayout.Button("Sort") )
		{
			cT.effects = cT.effects.OrderBy(eff => eff.id).ToArray();
			EditorUtility.SetDirty(cT);
		}

		base.OnInspectorGUI();
	}
}
