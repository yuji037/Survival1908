using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestEditorWindow : EditorWindow
{
	private static TestEditorWindow window;

	private static Object asset;

	[MenuItem("Test/テストエディタ")]
	private static void Open()
	{
		// 生成
		window = GetWindow<TestEditorWindow>("テストエディタ");
		window.minSize = new Vector2(800f, 600f);
	}

	private void OnEnable()
	{
	}

	private void OnGUI()
	{
		using ( var check = new EditorGUI.ChangeCheckScope() )
		{
			asset = EditorGUILayout.ObjectField("obj", asset, typeof(Object), false);

			if ( check.changed )
			{
				var path = AssetDatabase.GetAssetPath(asset);

				if(false == string.IsNullOrWhiteSpace(path) )
				{
					var objs = AssetDatabase.LoadAllAssetsAtPath(path);
					foreach(var obj in objs )
					{
						Debug.Log(obj.name);
					}
				}
			}
		}
	}

	private void Update()
	{
	}
}
