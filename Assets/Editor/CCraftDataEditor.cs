using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(CCraftData))]
public class CCraftDataEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var cT = target as CCraftData;
        if (GUILayout.Button("Reflesh"))
        {
            foreach(var status in cT.craftStatusList)
            {
                status.dstItemUnit.CorrectItemInfo();
                foreach(var srcUnit in status.srcItemUnitList)
                {
                    srcUnit.CorrectItemInfo();
                }
            }
            EditorUtility.SetDirty(cT);
        }
        if (GUILayout.Button("Sort"))
        {
            cT.craftStatusList = cT.craftStatusList.OrderBy(st => st.dstItemUnit.itemID).ToArray();
            EditorUtility.SetDirty(cT);
        }

        base.OnInspectorGUI();
    }
}
