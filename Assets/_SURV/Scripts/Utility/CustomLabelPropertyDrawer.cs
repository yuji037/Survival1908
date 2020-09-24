#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// ラベル表示を、フィールド変数によって変えるための基底クラス
/// 使用法：
/// 　以下のクラスを継承し、
/// 　[CustomPropertyDrawer(typeof(T))]を付ける
/// </summary>
public abstract class CustomLabelPropertyDrawer : PropertyDrawer
{
	protected abstract string GetOverrideLabel(SerializedProperty property);

	public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var overrideLabel = GetOverrideLabel(property);
		if (!string.IsNullOrWhiteSpace(overrideLabel))
		{
			label = new GUIContent(overrideLabel);
		}
		EditorGUI.PropertyField(position, property, new GUIContent(label), true);
	}

	public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, true);
	}
}
#endif