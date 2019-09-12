using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(CItemData))]
public class CItemDataEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var cT = target as CItemData;

		if (GUILayout.Button("Sort")) {
			cT.m_pcItemStatus = cT.m_pcItemStatus.OrderBy(st => st.ID).ToArray();
			EditorUtility.SetDirty(cT);
		}

		base.OnInspectorGUI();
	}
}
