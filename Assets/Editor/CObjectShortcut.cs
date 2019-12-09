using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CObjectShortcut : EditorWindow
{
	private static CObjectShortcut window;

	private Object[] objects;

	Vector2 scrollPos;

	[MenuItem("SURV/オブジェクトショートカット")]
	private static void Open()
	{
		// 生成
		window = GetWindow<CObjectShortcut>("ObjectShortcut");
	}

	private void OnEnable()
	{
		var objList = new List<Object>();
		for(int i = 0; i < 99; ++i )
		{
			var path = PlayerPrefs.GetString($"ObjectShortcutPath{i}", null);
			if ( string.IsNullOrWhiteSpace(path) )
				break;

			Object obj;
			if ( path == "None" )
				obj = null;
			else
				obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));

			objList.Add(obj);
		}

		objects = objList.ToArray();
	}

	private void OnGUI()
	{
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

		using ( var scope = new EditorGUILayout.HorizontalScope() )
		{
			if ( GUILayout.Button("+", GUILayout.MaxWidth(30f)) )
			{
				System.Array.Resize(ref objects, objects.Length + 1);
				PlayerPrefs.SetString($"ObjectShortcutPath{objects.Length}", "None");
				PlayerPrefs.Save();
			}
			if ( GUILayout.Button("-", GUILayout.MaxWidth(30f)) )
			{
				if ( objects.Length >= 1 )
				{
					System.Array.Resize(ref objects, objects.Length - 1);
					PlayerPrefs.DeleteKey($"ObjectShortcutPath{objects.Length + 1}");
					PlayerPrefs.Save();
				}
			}
		}

		var oldWidth = EditorGUIUtility.labelWidth;
		EditorGUIUtility.labelWidth = 1f;

		for (int i= 0; i < objects.Length; ++i )
		{
			using ( var check = new EditorGUI.ChangeCheckScope() )
			{
				objects[i] = EditorGUILayout.ObjectField((string)null, objects[i], typeof(Object), false);

				if ( check.changed )
				{
					var path = AssetDatabase.GetAssetPath(objects[i]);

					if ( string.IsNullOrWhiteSpace(path) )
						path = "None";

					PlayerPrefs.SetString($"ObjectShortcutPath{i}", path);
					PlayerPrefs.Save();
				}

			}
		}
		EditorGUIUtility.labelWidth = oldWidth;

		EditorGUILayout.EndScrollView();
	}
}
