using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(CSkillData))]
public class CSkillDataEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var cT = target as CSkillData;

		//if (GUILayout.Button("Reflesh"))
		//{
		//    CItemDataMan.Reflesh();
		//    Repaint();
		//}
		if ( GUILayout.Button("Sort") )
		{
			cT.skills = cT.skills.OrderBy(s => s.id).ToArray();
			EditorUtility.SetDirty(cT);
		}

		base.OnInspectorGUI();
	}
}
