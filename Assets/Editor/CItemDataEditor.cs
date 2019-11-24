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

        //if (GUILayout.Button("Reflesh"))
        //{
        //    CItemDataMan.Reflesh();
        //    Repaint();
        //}
        if (GUILayout.Button("Sort")) {
			cT.itemStatusList = cT.itemStatusList.OrderBy(st => st.id).ToArray();
			EditorUtility.SetDirty(cT);
		}

		base.OnInspectorGUI();
	}
}
