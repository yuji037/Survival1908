using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(SkillData))]
public class SkillDataEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var cT = target as SkillData;

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

[CustomPropertyDrawer(typeof(Skill))]
public class SkillDrawer: CustomLabelPropertyDrawer
{
	protected override string GetOverrideLabel(SerializedProperty property)
	{
		return property.FindPropertyRelative("dispName")?.stringValue;
	}
}