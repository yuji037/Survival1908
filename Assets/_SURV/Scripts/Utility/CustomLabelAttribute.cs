using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Qita: インスペクタのパラメータの名前を簡単に変えるエディタ拡張
/// https://qiita.com/Basuhei_Acheron/items/419f19d6532f1ffd3f33
/// 
/// 使用例
/// [SerializeField, CustomLabel("表示ラベル名")] private int customLabelValue;
/// </summary>
public class CustomLabelAttribute : PropertyAttribute
{
	public readonly string Value;

	public CustomLabelAttribute(string value)
	{
		Value = value;
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(CustomLabelAttribute))]
public class CustomLabelDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var newLabel = attribute as CustomLabelAttribute;
		EditorGUI.PropertyField(position, property, new GUIContent(newLabel.Value), true);
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, true);
	}
}
#endif
