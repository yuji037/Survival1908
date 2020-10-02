using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ItemListElement
{
	public enum Type
	{
		Item,
		EquipmentItem,
	}

	public string itemID;
	public int equipmentId;
	public Type type;
	public GameObject gameObject;
	public UIItemElement uiItemElement;
	public Image selectBG;
	public Image iconImage;
	public Text nameText;

	public ItemListElement(GameObject obj, int elementIndex, Action<int> onStartDrag = null, Action<int> onSelectItem = null)
	{
		gameObject = obj;
		nameText = obj.GetComponentInChildren<Text>();
		uiItemElement = obj.GetComponentInChildren<UIItemElement>();
		uiItemElement.Init(elementIndex);
		uiItemElement.OnStartDrag = onStartDrag;
		uiItemElement.OnSelectItem = onSelectItem;
		selectBG = obj.transform.Find("SelectBG").GetComponent<Image>();
		iconImage = gameObject.transform.Find("Image").GetComponent<Image>();
	}

	public void SetItemData(string itemID, int itemCount)
	{
		this.itemID = itemID;
		equipmentId = -1;
		type = Type.Item;
		var itemStatus = ItemDataMan.Instance.GetItemStatusById(itemID);
		iconImage.sprite = itemStatus.GetIconSprite();
		var str = itemStatus.Name;
		str += GetCountText(itemCount);
		nameText.text = str;
	}

	public void SetEquipmentData(bool isEquipping, EquipmentItem equipment)
	{
		type = Type.EquipmentItem;
		this.itemID = equipment.ItemId;
		equipmentId = equipment.EquipmentId;
		iconImage.sprite = equipment.GetItemStatus().GetIconSprite();
		var str = string.Empty;
		if (isEquipping) { str += "E:"; }
		str += equipment.GetName();
		nameText.text = str;
	}

	public string GetCountText(int count)
	{
		var str = "";
		if (count >= 2)
			str += " ×" + count;
		return str;
	}

	public void SetIsSelected(bool isSelected)
	{
		selectBG.color = isSelected ? new Color(1f, 1f, 1f, 0.4f) : new Color(1f, 1f, 1f, 0f);
	}
}
