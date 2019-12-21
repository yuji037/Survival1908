using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerUpHandler
{
	//Vector2 beginDragPos;
	Vector2 dragVec;

	[SerializeField]
	bool debugDispDrag = false;

	[SerializeField]
	RectTransform debugDispDragTrans;

	private void Update()
	{
		if(dragVec != Vector2.zero )
		{
			CLocalPlayer.Instance.InputTouchMovement(dragVec);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		//beginDragPos = eventData.pos
	}

	public void OnDrag(PointerEventData eventData)
	{
		dragVec = eventData.position - eventData.pressPosition;

		if ( debugDispDrag )
		{
			var pos = ( eventData.position + eventData.pressPosition ) / 2 - new Vector2(Screen.width >> 1, Screen.height >> 1);
			var height = ( eventData.position - eventData.pressPosition ).magnitude;

			var rot = Quaternion.LookRotation(Vector3.back, dragVec);

			debugDispDragTrans.localPosition = pos;
			debugDispDragTrans.localRotation = rot;
			var sizeDelta = debugDispDragTrans.sizeDelta;
			sizeDelta.y = height;
			debugDispDragTrans.sizeDelta = sizeDelta;
		}

	}

	public void OnPointerUp(PointerEventData eventData)
	{
		dragVec = Vector2.zero;

		if ( debugDispDrag )
		{
			debugDispDragTrans.localPosition = new Vector3(-10000f, -10000f, 0f);
		}
	}
}
