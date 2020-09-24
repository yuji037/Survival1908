using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UIItemShortcutWindow : MonoBehaviour
{
	[SerializeField] private GameObject templateObj;

	private List<UIItemShortcut> items = new List<UIItemShortcut>();

	public void Init(UIDragItem uiDragItem)
	{
		items.Add(templateObj.GetComponent<UIItemShortcut>());
		while ( items.Count < 3 )
		{
			var obj = Instantiate(templateObj, transform);
			items.Add(obj.GetComponent<UIItemShortcut>());
		}

		for(int i = 0; i < items.Count; ++i)
		{
			items[i].Init(i, uiDragItem);
		}
	}

	public void Load()
	{
		foreach(var item in items )
		{
			item.Load();
		}
	}
}
