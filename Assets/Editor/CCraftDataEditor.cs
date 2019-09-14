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
            foreach(var status in cT.m_pcCraftStatus)
            {
                status.DstItemUnit.CorrectItemInfo();
                foreach(var srcUnit in status.SrcItemUnitList)
                {
                    srcUnit.CorrectItemInfo();
                }
            }
            EditorUtility.SetDirty(cT);
        }
        if (GUILayout.Button("Sort"))
        {
            cT.m_pcCraftStatus = cT.m_pcCraftStatus.OrderBy(st => st.DstItemUnit.ItemID).ToArray();
            EditorUtility.SetDirty(cT);
        }

        base.OnInspectorGUI();
    }
}
