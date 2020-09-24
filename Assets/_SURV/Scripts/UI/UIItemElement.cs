using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// アイテム欄1個
/// タップ：選択
/// ドラッグ：つかむ
/// 機能を持つ
/// </summary>
public class UIItemElement : EventTrigger
{
	private bool isPointed = false;
	private float pointerDownTime = 0f;
	private int index;

	public Action<int> OnStartDrag;
	public Action<int> OnSelectItem;

	public void Init(int inventryIndex)
	{
		this.index = inventryIndex;
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		isPointed = true;
		pointerDownTime = Time.unscaledTime;
	}

	public override void OnDrag(PointerEventData eventData)
	{
		if( isPointed
		&&  Time.unscaledTime > pointerDownTime + 0.3f )
		{
			isPointed = false;
			OnStartDrag?.Invoke(index);
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		isPointed = false;
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if ( isPointed )
		{
			OnSelectItem?.Invoke(index);
		}
		isPointed = false;
	}
}
