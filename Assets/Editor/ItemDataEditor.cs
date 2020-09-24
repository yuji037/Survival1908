using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var cT = target as ItemData;

        //if (GUILayout.Button("Reflesh"))
        //{
        //    CItemDataMan.Reflesh();
        //    Repaint();
        //}
        if (GUILayout.Button("Sort")) {
			cT.itemStatusList = cT.itemStatusList.OrderBy(st => st.ID).ToArray();
			EditorUtility.SetDirty(cT);
		}

		base.OnInspectorGUI();
	}
}
