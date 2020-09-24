using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICraftMan : SingletonMonoBehaviour<UICraftMan>, ISwitchUI
{
    [SerializeField] private GameObject		craftCellParentObj		= default;
    [SerializeField] private GameObject		craftCellTemp			= default;
	[SerializeField] private Button			bgScreenButton			= default;
    [SerializeField] private Button			craftActionButton		= default;
    [SerializeField] private Button			countMinus10Button		= default;
    [SerializeField] private Button			countMinus1Button		= default;
    [SerializeField] private Button			countPlus1Button		= default;
    [SerializeField] private Button			countPlus10Button		= default;
    [SerializeField] private Text			countText				= default;
	[SerializeField] private Text			selectCraftActionText	= default;
	[SerializeField] private Text			craftDesctiptionText	= default;
	[SerializeField] private UIPopupText	uiPopupText				= default;
	
    private int						selectRecipeIndex	= -1;
	private int						selectCreateCount	= 0;
	private GameObject				window;
    private List<ItemListElement>	craftElements		= new List<ItemListElement>();

	public bool IsDisplay { get => window.activeSelf; }

	public void Initialize()
	{
		GetComponent<Canvas>().enabled = true;
		window = transform.Find("Window").gameObject;
		craftCellTemp.SetActive(false);
		craftActionButton.onClick.AddListener(OnClickCraftButton);

		bgScreenButton.onClick.AddListener(() => SelectRecipe(-1));

		Disp(false);
	}

    public void Disp(bool bDisp)
    {
        window.SetActive(bDisp);
        if (bDisp)
        {
			CheckFacilityStore();
			UpdateCraftUI();
			SelectRecipe(-1);
		}
	}

    public void UpdateCraftUI()
    {
		var craftRecipeList = CraftDataMan.Instance.CraftRecipeList;
        int elementIndex = 0;
        for(int i = 0; i < craftRecipeList.Count; ++i)
        {
			var craftRecipe = craftRecipeList[i];
            // 表示しない条件


            ItemListElement element = null;
            if (elementIndex >= craftElements.Count)
            {
                var obj = Instantiate(craftCellTemp);
                obj.transform.SetParent(craftCellParentObj.transform, false);
                element = new ItemListElement(obj, elementIndex, null, SelectRecipe);
				craftElements.Add(element);
				// NOTE:ここ既にアクティブかどうかチェックした方が、動作軽くなるかも
				element.gameObject.SetActive(true);
            }
            else
            {
                element = craftElements[elementIndex];
				// NOTE:ここ既にアクティブかどうかチェックした方が、動作軽くなるかも
				element.gameObject.SetActive(true);
            }

			// アイテム情報更新
			element.SetItemData(craftRecipe.dstItemUnit.itemID, craftRecipe.dstItemUnit.count);

            elementIndex++;
        }
        while (selectRecipeIndex >= elementIndex)
        {
            // インベントリのアイテム種類数より選択インデックスが大きかった場合
            selectRecipeIndex--;
        }
        // 再度選択
        SelectRecipe(selectRecipeIndex);

        for (; elementIndex < craftElements.Count; ++elementIndex)
        {
            // WARNING:ここ既にアクティブかどうかチェックした方が、動作軽くなるかも
            craftElements[elementIndex].gameObject.SetActive(false);
        }
	}

	private void SelectRecipe(int index)
	{
		selectRecipeIndex = index;

		ItemListElement selectElement = null;

		for (int i = 0; i < craftElements.Count; ++i)
		{
			var element = craftElements[i];
			if (i == selectRecipeIndex)
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
			craftActionButton.interactable = false;
			return;
		}

        var craftRecipe = CraftDataMan.Instance.CraftRecipeList[selectRecipeIndex];

		var canCreateCount = int.MaxValue;
        var craftDesctiption = "必要アイテム\n";
        foreach(var srcUnit in craftRecipe.srcItemUnitList)
        {
			var hasCount = ItemInventry.Instance.GetHasItemCount(srcUnit.itemID);
			var rate = Mathf.FloorToInt(hasCount / srcUnit.count);
			if (canCreateCount > rate)
			{
				canCreateCount = rate;
			}
			craftDesctiption += $"{srcUnit.itemName} {hasCount}/{srcUnit.count}\n";
		}
		if (false == CraftDataMan.Instance.CanCraftCondition(craftRecipe))
		{
			// 設備条件を満たしていない
			canCreateCount = 0;
		}
		craftDesctiption += $"必要設備：{craftRecipe.GetConditionName()}";
        craftDesctiptionText.text = craftDesctiption;
		countText.color = canCreateCount == 0 ? Color.gray : Color.white;

		// 初期値で最大数作る状態にしてみる
		selectCreateCount = canCreateCount;

		UpdateRecipeDescription();

		var dstItemStatus = ItemDataMan.Instance.GetItemStatusById(craftRecipe.dstItemUnit.itemID);
		uiPopupText.Popup(Input.mousePosition, dstItemStatus.GetDescription());
	}

	private void UpdateRecipeDescription()
	{
		var craftRecipe = CraftDataMan.Instance.CraftRecipeList[selectRecipeIndex];

		var canCreateCount = int.MaxValue;
		craftDesctiptionText.text = "必要アイテム\n";
		foreach (var srcUnit in craftRecipe.srcItemUnitList)
		{
			var hasCount = ItemInventry.Instance.GetHasItemCount(srcUnit.itemID);
			canCreateCount = Mathf.FloorToInt(hasCount / srcUnit.count);
			craftDesctiptionText.text += $"{srcUnit.itemName} {hasCount}/{srcUnit.count}\n";
		}

	}

	public void OnClickCraftButton()
    {
		var recipe = CraftDataMan.Instance.CraftRecipeList[selectRecipeIndex];
		// 必要アイテム消費
		foreach (var srcUnit in recipe.srcItemUnitList)
		{
			ItemInventry.Instance.AddChangeItemCount(srcUnit.itemID, -srcUnit.count);
		}

		if (recipe.craftConditionType == CraftConditionType.None)
		{
			// 設備は不要（即完成）
			// 製作アイテム増加
			ItemInventry.Instance.AddChangeItemCount(
				recipe.dstItemUnit.itemID,
				recipe.dstItemUnit.count);

			UpdateCraftUI();
		}
		else
		{
			CraftDataMan.Instance.CurrentFacility.StartCraft(recipe);
			UIIngame.Instance.OnClickSwitchUIButton(0);
		}
    }

	/// <summary>
	/// プレイヤーが触れている製作設備をチェック
	/// </summary>
	private void CheckFacilityStore()
	{
		var currentFacility = CraftDataMan.Instance.CurrentFacility;
		if (currentFacility == null) { return; }

		currentFacility.ReceiveStoringItem();
	}
}
