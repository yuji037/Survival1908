using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItemShortcut : EventTrigger
{
	private Image selectBG = default;
	private Image itemImage = default;
	private Text countText;
	private string itemID = null;
	private UIDragItem uiDragItem;
	private int index = 0;
	private int equipmentIndex = -1;

	private string SaveKey { get { return $"ItemShortcut{index}"; } }

	public void Init(int index, UIDragItem uiDragItem, int equipmentIndex = -1)
	{
		selectBG = transform.Find("SelectBG").GetComponent<Image>();
		selectBG.color = Color.clear;
		itemImage = transform.Find("ItemImage").GetComponent<Image>();
		countText = transform.Find("CountText").GetComponent<Text>();

		this.index = index;
		this.uiDragItem = uiDragItem;
		this.equipmentIndex = equipmentIndex;

		ItemInventry.Instance.OnChangedItemCount += UpdateUI;
		Load();
	}

	public void Load()
	{
		var _itemID = PlayerPrefs.GetString(SaveKey, null);
		SetItemID(_itemID);
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if ( string.IsNullOrWhiteSpace(itemID) ) { return; }

		if(equipmentIndex == -1)
		{
			// アイテム使用
			ItemInventry.Instance.UseItem(itemID);
		}
		else
		{
			// 装備アイテム使用
			LocalPlayer.Instance.EquipmentModule.Equip(equipmentIndex);
		}
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		if ( string.IsNullOrWhiteSpace(itemID) ) { return; }

		uiDragItem.SetDraggingItemID(itemID);
		SetItemID(null);
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if ( false == string.IsNullOrWhiteSpace( uiDragItem.DraggingItemID) )
		{
			selectBG.color = new Color(1f, 1f, 1f, 0.4f);
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		selectBG.color = Color.clear;
	}

	public override void OnDrop(PointerEventData eventData)
	{
		Debug.Log("uwagaki");
		SetItemID(uiDragItem.DraggingItemID);
	}

	private void SetItemID(string _itemID)
	{
		itemID = _itemID;
		PlayerPrefs.SetString(SaveKey, itemID);
		PlayerPrefs.Save();
		UpdateUI();
	}

	private void UpdateUI()
	{
		if ( string.IsNullOrWhiteSpace(itemID) )
		{
			itemImage.gameObject.SetActive(false);
			countText.text = "";
			return;
		}
		else
		{
			itemImage.gameObject.SetActive(true);
			itemImage.sprite = ItemImageData.Instance.GetSprite(itemID);
			countText.text = $"×{ItemInventry.Instance.GetHasItemCount(itemID)}";
		}
	}

	private void OnDestroy()
	{
		ItemInventry.Instance.OnChangedItemCount -= UpdateUI;
	}
}
