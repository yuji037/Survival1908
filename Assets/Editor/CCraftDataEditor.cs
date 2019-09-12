using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        }

        base.OnInspectorGUI();
    }
}
