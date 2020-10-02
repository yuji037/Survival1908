using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftFacility : MonoBehaviour
{
	[SerializeField] private CraftConditionType conditionType = default;

	private UIFieldPopup uiFieldPopup = default;

	private CraftRecipe currentRecipe = null;
	private int currentCreateCount = 0;
	private ItemCountUnit[] storingItems = null;
	private float craftEndTime;
	private AudioSource craftingSound = null;

	public CraftConditionType ConditionType { get { return conditionType; } }

	private void Start()
	{
		uiFieldPopup = UIFieldPopup.SecureUI(transform);
	}

	private void Update()
	{
		if(currentRecipe == null) { return; }

		uiFieldPopup.SetSliderRate((currentRecipe.needTime - (craftEndTime - IngameTime.Time)) / currentRecipe.needTime);

		if(IngameTime.Time >= craftEndTime)
		{
			CompleteCraft();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject != LocalPlayer.Instance.gameObject) { return; }

		CraftDataMan.Instance.SetCurrentFacility(this);
		uiFieldPopup.Popup(transform);
		UpdatePopupText();
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject != LocalPlayer.Instance.gameObject) { return; }

		CraftDataMan.Instance.SetCurrentFacility(null);
		if(currentRecipe == null)
		{
			uiFieldPopup.Deactive();
		}
	}

	public void StartCraft(CraftRecipe recipe, int createCount)
	{
		currentRecipe = recipe;
		currentCreateCount = createCount;
		storingItems = recipe.srcItemUnitList;
		craftEndTime = IngameTime.Time + recipe.needTime * createCount;
		UpdatePopupText();
		craftingSound = SoundManager.Instance.Play("SE_CookMeat00", true);
	}

	private void CompleteCraft()
	{
		var itemUnit = new ItemCountUnit();
		itemUnit.itemID = currentRecipe.dstItemUnit.itemID;
		itemUnit.count = currentRecipe.dstItemUnit.count * currentCreateCount;
		// TODO: 途中で中断したら途中までの数を提供
		storingItems = new ItemCountUnit[] { currentRecipe.dstItemUnit };
		EndCraft();
	}

	private void EndCraft()
	{
		if(currentRecipe != null && IngameTime.Time < craftEndTime)
		{
			UIInstantMessage.Instance.RequestMessage("製作を中断しました");
		}
		currentRecipe = null;
		uiFieldPopup.DispSlider(false);
		UpdatePopupText();
		craftingSound?.Stop();
	}

	public void ReceiveStoringItem()
	{
		EndCraft();
		if(storingItems == null) { return; }
		foreach (var item in storingItems)
		{
			ItemInventry.Instance.AddChangeItemCount(item.itemID, item.count);
		}
		storingItems = null;
		UpdatePopupText();
	}

	private void UpdatePopupText()
	{
		if(storingItems == null)
		{
			uiFieldPopup.SetText(CraftRecipe.GetConditionName(conditionType));
		}
		else if(currentRecipe != null)
		{
			uiFieldPopup.SetText("製作中...");
		}
		else
		{
			uiFieldPopup.SetText("完了");
		}
	}
}
