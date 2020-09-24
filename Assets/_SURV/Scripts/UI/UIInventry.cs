using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;


public class UIInventry : SingletonMonoBehaviour<UIInventry>, ISwitchUI
{
	//========================================================
	// Serializeフィールド
	//========================================================
	[SerializeField] private GameObject				inventryCellParentObj	= default;
	[SerializeField] private GameObject				inventryCellTemp		= default;
    [SerializeField] private UIDragItem				uiDragItem				= default;
	[SerializeField] private Button					useItemButton			= default;
	[SerializeField] private Button					bgScreenButton			= default;
	[SerializeField] private UIItemShortcutWindow	uiItemShortcutWindow	= default;
	[SerializeField] private UIPopupText			uiPopupText				= default;
	[SerializeField] private Text					partyStatusText			= default;

	//========================================================
	// NonSerializeフィールド
	//========================================================
	private GameObject				window;
	private List<ItemListElement>	inventryElements		= new List<ItemListElement>();
	private	ItemListElement			selectElement			= null;
    private string					selectItemId;
	private int						selectItemIndex;
	private int						selectEquipmentId	= -1;
	private Text					useItemButtonText;
	private List<PartyChara>		partyCharas				= new List<PartyChara>();

	//========================================================
	// プロパティ
	//========================================================
	public bool IsDisplay { get => window.activeSelf; }

	//========================================================
	// メソッド
	//========================================================

	public void Initialize()
	{
		GetComponent<Canvas>().enabled = true;
		InitUI();
	}

	private void Update()
	{
		uiDragItem.Tick();
	}

	public void Disp(bool bDisp)
    {
        window.SetActive(bDisp);
        if (bDisp)
        {
			UpdateInventryUI();
			UpdateStatusUI();
			uiItemShortcutWindow.Load();
		}
	}

	public void RegisterParyChara(PartyChara chara)
	{
		partyCharas.Add(chara);
	}

	private void InitUI()
	{
		window = transform.Find("Window").gameObject;
		useItemButton.onClick.AddListener(OnClickUseItemButton);
		useItemButtonText = useItemButton.GetComponentInChildren<Text>();

		bgScreenButton.onClick.AddListener(() => SelectItem(-1));

		inventryCellTemp.SetActive(false);

		uiItemShortcutWindow.Init(uiDragItem);
		SelectItem(-1);
		Disp(false);
	}

	public void UpdateInventryUI()
    {
        int elementIndex = 0;

		foreach (var hasCount in ItemInventry.Instance.ItemHasCountDict)
        {
            if (hasCount.Value == 0)
                continue;

            var itemListElement = GetActiveElement(elementIndex);

			// アイテム情報更新
			itemListElement.SetItemData(hasCount.Key, hasCount.Value);

            elementIndex++;
        }
		var itemDict = EquipmentInventry.Instance.ItemDict;
		foreach(var pair in itemDict)
		{
			var equipmentId = pair.Key;
			var itemListElement = GetActiveElement(elementIndex);

			// 装備アイテム情報更新
			var isEquipping = LocalPlayer.Instance.EquipmentModule.IsEquipping(equipmentId, out var equippingPart);
			itemListElement.SetEquipmentData(isEquipping, itemDict[equipmentId]);
			elementIndex++;
		}

		// 選択解除
		SelectItem(-1);

		for (; elementIndex < inventryElements.Count; ++elementIndex)
        {
            // WARNING:ここ既にアクティブかどうかチェックした方が、動作軽くなるかも
            inventryElements[elementIndex].gameObject.SetActive(false);
        }
    }

	private ItemListElement GetActiveElement(int elementIndex)
	{
		ItemListElement element = null;
		if (elementIndex >= inventryElements.Count)
		{
			var obj = Instantiate(inventryCellTemp);
			obj.transform.SetParent(inventryCellParentObj.transform, false);
			element = new ItemListElement(obj, elementIndex, StartDragItem, SelectItem);
			inventryElements.Add(element);
		}
		element = inventryElements[elementIndex];
		if (!element.gameObject.activeSelf)
		{
			element.gameObject.SetActive(true);
		}
		return element;
	}

	private void StartDragItem(int index)
	{
		var itemID = inventryElements[index].itemID;
		uiDragItem.SetDraggingItemID(itemID);
	}

	public void SelectItem(int index)
    {
        selectItemIndex = index;
		selectEquipmentId = -1;
		selectElement = null;

		for (int i = 0; i < inventryElements.Count; ++i)
		{
			var element = inventryElements[i];
			if (i == selectItemIndex)
			{
				selectElement = element;
				element.SetIsSelected(true);
			}
			else
			{
				element.SetIsSelected(false);
			}
		}

		if(selectElement == null )
		{
			// 選択アイテムを無しに変更
			// ポップアップが出てるなら消す
			uiPopupText.Deactive();
			useItemButton.gameObject.SetActive(false);
			return;
		}


		selectItemId = selectElement.itemID;

		var itemStatus = ItemDataMan.Instance.GetItemStatusById(selectItemId);
		if ( itemStatus.IsUsableType() )
		{
			useItemButton.gameObject.SetActive(true);
			useItemButtonText.text = itemStatus.GetVerbUseAction();
		}
		else
		{
			useItemButton.gameObject.SetActive(false);
		}

		string description = string.Empty;
		switch (selectElement.type)
		{
			case ItemListElement.Type.Item:
				description = itemStatus.GetDescription();
				break;
			case ItemListElement.Type.EquipmentItem:
				selectEquipmentId = selectElement.equipmentId;
				var equipment = EquipmentInventry.Instance.ItemDict[selectEquipmentId];
				description = equipment.GetDescription();
				break;
		}

		uiPopupText.Popup(Input.mousePosition, description);
	}

	public void OnClickUseItemButton()
    {
		if(selectEquipmentId == -1)
		{
			// アイテム使用
			ItemInventry.Instance.UseItem(selectItemId);
		}
		else
		{
			// 装備
			LocalPlayer.Instance.EquipmentModule.Equip(selectEquipmentId);
		}

		UpdateInventryUI();
		UpdateStatusUI();
    }

	private void UpdateStatusUI()
	{
		var pc = partyCharas[0];
		var status = pc.StatusModule as PartyCharaStatusModule;
		var equipModule = pc.EquipmentModule;
		// 持ち物画面のパーティステータス
		partyStatusText.text = "キャラ" +
			"\nLevel : "	+ status.Level +
			"\nEXP : "		+ status.CombatExp.ToString("f0") + " / " + status.NeedLvupExp.ToString("f0") +
			"\nHP : "		+ Mathf.CeilToInt(status.HP) + " / " + Mathf.CeilToInt(status.MaxHP) +
			"\nMP : "		+ Mathf.FloorToInt(status.MP) + " / " + Mathf.FloorToInt(status.MaxMP) +
			"\nFood : "		+ Mathf.CeilToInt(status.Food) +
			"\n攻撃力 : "	+ status.GetAtk() +
			"\n防御力 : "	+ status.GetDef() +
			"\n武器 : "		+ equipModule.GetEquippingItemName(EquipmentPart.Weapon		) +
			"\n頭 : "		+ equipModule.GetEquippingItemName(EquipmentPart.Head		) +
			"\n胴 : "		+ equipModule.GetEquippingItemName(EquipmentPart.Body		) +
			"\nアクセ1 : "	+ equipModule.GetEquippingItemName(EquipmentPart.Accessory1	) +
			"\nアクセ2 : "	+ equipModule.GetEquippingItemName(EquipmentPart.Accessory2	) +
			"";
	}
}
