#if UNITY_EDITOR
using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class ReorderableTest : MonoBehaviour
{
	public List<MMotion> HogeList;

	// ... snip ...
}

[System.Serializable]
public class MMotion
{
	[SerializeField] private string Name;
	[SerializeField] private int Age;
}


[CustomEditor(typeof(ReorderableTest))]
public class HogeEditor : Editor
{
	ReorderableList hogeListReorderable;

	void OnEnable()
	{
		hogeListReorderable = new ReorderableList(serializedObject, serializedObject.FindProperty("HogeList"));
		hogeListReorderable.drawElementCallback = (rect, index, isActive, isFocused) => {
			var motion = hogeListReorderable.serializedProperty.GetArrayElementAtIndex(index);
			var displayName = string.Format("{0} : {1}",
				motion.FindPropertyRelative("Name").stringValue,
				motion.FindPropertyRelative("Age").intValue
			);
			EditorGUI.LabelField(rect, displayName);
		};
	}

	// 変更可能なリストを表示
	public override void OnInspectorGUI()
	{
		// とりあえず元のプロパティ表示はしておく
		DrawDefaultInspector();

		serializedObject.Update();

		// リスト・配列の変更可能なリストの表示
		hogeListReorderable.DoLayoutList();

		serializedObject.ApplyModifiedProperties();
	}
}
#endif