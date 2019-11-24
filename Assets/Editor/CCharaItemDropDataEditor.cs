﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(CCharaItemDropData))]
public class CCharaItemDropDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var cT = target as CCharaItemDropData;

        if (GUILayout.Button("Reflesh"))
        {
            foreach (var charaDropUnit in cT.charaItemDropUnits)
            {
                var chara = CCharaDataMan.Instance.GetCharaDataByName(charaDropUnit.charaName);
                if (chara == null)
                {
                    chara = CCharaDataMan.Instance.GetCharaDataByID(charaDropUnit.charaID);
                }
                charaDropUnit.charaName = chara.name;
                charaDropUnit.charaID = chara.id;

                foreach(var itemDropUnit in charaDropUnit.itemDropUnits)
                {
                    itemDropUnit.itemCountUnit.CorrectItemInfo();
                }
            }
            EditorUtility.SetDirty(cT);
        }
        //if (GUILayout.Button("Sort"))
        //{
        //    cT.m_pcCharaItemDropUnits = cT.m_pcCharaItemDropUnits.OrderBy(st => st.ID).ToArray();
        //    EditorUtility.SetDirty(cT);
        //}

        base.OnInspectorGUI();
    }
}
