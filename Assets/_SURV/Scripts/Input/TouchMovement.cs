using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerUpHandler
{
	//Vector2 beginDragPos;
	Vector2 dragVec;

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
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		dragVec = Vector2.zero;
	}
}
