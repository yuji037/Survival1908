using System.Collections;
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
            foreach (var charaDropUnit in cT.m_pcCharaItemDropUnits)
            {
                var chara = CCharaDataMan.Instance.GetCharaDataByName(charaDropUnit.CharaName);
                if (chara == null)
                {
                    chara = CCharaDataMan.Instance.GetCharaDataByID(charaDropUnit.CharaID);
                }
                charaDropUnit.CharaName = chara.Name;
                charaDropUnit.CharaID = chara.ID;

                foreach(var itemDropUnit in charaDropUnit.ItemDropUnits)
                {
                    itemDropUnit.ItemCountUnit.CorrectItemInfo();
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
