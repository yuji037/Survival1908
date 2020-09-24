using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

//[CustomEditor(typeof(CraftData))]
//public class CraftDataEditor : Editor
//{
//	private SerializedProperty recipesProp;

//	private void OnEnable()
//	{
//		recipesProp = serializedObject.FindProperty("craftRecipeList");
//	}

//	public override void OnInspectorGUI()
//    {
//        var cT = target as CraftData;
//        if (GUILayout.Button("Reflesh"))
//        {
//            foreach(var status in cT.craftRecipeList)
//            {
//                status.dstItemUnit.CorrectItemInfo();
//                foreach(var srcUnit in status.srcItemUnitList)
//                {
//                    srcUnit.CorrectItemInfo();
//                }
//            }
//            EditorUtility.SetDirty(cT);
//        }
//        if (GUILayout.Button("Sort"))
//        {
//            cT.craftRecipeList = cT.craftRecipeList.OrderBy(st => st.dstItemUnit.itemID).ToArray();
//            EditorUtility.SetDirty(cT);
//        }

//        base.OnInspectorGUI();
//    }

//	private void DispRecipeList()
//	{
//		if(recipesProp.isExpanded = EditorGUILayout.Foldout(recipesProp.isExpanded, "Craft Recipe List"))
//		{
//			//recipesProp.propertyType == SerializedPropertyType.Vector3
//			//for(int i = 0; i < )
//		}
//	}
//}

[CustomPropertyDrawer(typeof(CraftRecipe))]
public class CraftRecipeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var name = property.FindPropertyRelative("dstItemUnit.itemName").stringValue;
		if (!string.IsNullOrWhiteSpace(name)) { label = new GUIContent(name); }
		PropertyDrawerUtility.DrawDefaultGUI(position, property, label);
		return;
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return PropertyDrawerUtility.GetDefaultPropertyHeight(property, label);
	}
}