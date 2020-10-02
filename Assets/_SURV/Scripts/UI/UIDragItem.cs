using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDragItem : MonoBehaviour
{
	private string draggingItemId = null;
	public string DraggingItemID { get { return draggingItemId; } }

	// Start is called before the first frame update
	void Start()
    {
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	public void Tick()
    {
		if ( Input.GetMouseButton(0) )
		{
			if ( false == string.IsNullOrWhiteSpace(draggingItemId) )
			{
				transform.position = Input.mousePosition;
			}
		}
		else
		{
			SetDraggingItemID(null);
		}
	}

	public void SetDraggingItemID(string itemID)
	{
		draggingItemId = itemID;

		if(string.IsNullOrWhiteSpace(draggingItemId) )
		{
			gameObject.SetActive(false);
		}
		else
		{
			gameObject.SetActive(true);
			GetComponentInChildren<Image>().sprite = ItemDataMan.Instance.GetItemStatusById(itemID).GetIconSprite();
		}
	}
}
