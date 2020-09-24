using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPartyStatus : SingletonMonoBehaviour<UIPartyStatus>
{
	public enum Type
	{
		HP,
		MP,
		Food,
	}

	[SerializeField] private Text			nameText		= default;
	[SerializeField] private Text			hpText			= default;
	[SerializeField] private Text			mpText			= default;
	[SerializeField] private Text			foodText		= default;
	[SerializeField] private Slider			hpSlider		= default;
	[SerializeField] private Slider			mpSlider		= default;
	[SerializeField] private Slider			foodSlider		= default;
	[SerializeField] private GameObject		efficacyTemp	= default;

	private     List<PartyCharaStatusModule>	pcStatusModules = new List<PartyCharaStatusModule>();
	private		int					dispFood = -1;
	private		EfficacyModule		efficacyModule;
	private		List<GameObject>	efficacyUIs = new List<GameObject>();

	public void Initialize()
	{
		efficacyUIs.Add(efficacyTemp);
		efficacyTemp.SetActive(false);
	}

	public void RegisterPartyChara(PartyChara partyChara){
		pcStatusModules.Add(partyChara.StatusModule as PartyCharaStatusModule);
		efficacyModule = partyChara.EfficacyModule;
		efficacyModule.AddModifiedEvent(UpdateEfficacyUIs);
	}

	public void UpdateName()
	{
		var pc = pcStatusModules[0];
		nameText.text = "キャラ";
	}

	public void UpdateHP()
	{
		var status = pcStatusModules[0];
		hpText.text = "HP : " + Mathf.CeilToInt(status.HP).ToString() + " / " + Mathf.CeilToInt(status.MaxHP).ToString();
		hpSlider.value = status.HP / status.MaxHP;

		//// MaxHPによってSliderUIの長さを変える処理
		//var rt = m_sliderHP.GetComponent<RectTransform>();
		//var sizeDelta = rt.sizeDelta;
		//// MaxHp:50 で 100f
		//sizeDelta.x = pc.MaxHp * 2f;
		//rt.sizeDelta = sizeDelta;
	}

	public void UpdateMP()
	{
		var status = pcStatusModules[0];
		mpText.text = "MP : " + Mathf.FloorToInt(status.MP).ToString() + " / " + Mathf.FloorToInt(status.MaxMP).ToString();
		mpSlider.value = status.MP / status.MaxMP;
	}

	public void UpdateFood()
	{
		var status = pcStatusModules[0];
		var food = status.Food;
		var _dispFood = Mathf.CeilToInt(food);
		if (dispFood != _dispFood )
		{
			dispFood = _dispFood;
			foodText.text = "Food : " + dispFood.ToString();
			foodSlider.value = dispFood / 100f;
		}
	}

	private void UpdateEfficacyUIs()
	{
		var uiIndex = 0;
		var efficacyData = GameCoordinator.Instance.EfficacyData;
		foreach (var eff in efficacyData.Efficacys)
		{
			var val = efficacyModule.GetEfficacy((EfficacyType)eff.ID);
			if (val > 0f)
			{
				var icon = eff.StatusIcon;
				if(icon == null) { Debug.LogError($"アイコン画像が登録されていません。Id:{eff.ID}"); }
				var elementObj = GetActiveUIElement(uiIndex);
				elementObj.GetComponent<Image>().sprite = icon;
				uiIndex++;
			}
		}
		for (; uiIndex < efficacyUIs.Count; uiIndex++)
		{
			efficacyUIs[uiIndex].SetActive(false);
		}
	}

	private GameObject GetActiveUIElement(int uiIndex)
	{
		if (uiIndex >= efficacyUIs.Count)
		{
			var newUI = Instantiate(efficacyTemp, efficacyTemp.transform.parent);
			efficacyUIs.Add(newUI);
		}
		var elementObj = efficacyUIs[uiIndex];
		elementObj.SetActive(true);
		return elementObj;
	}
}

public struct IntVector2
{
    public int x;
    public int y;

    public IntVector2(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}

public static class Vector2Extention
{
    public static IntVector2 GetIntVector2(this Vector2 vec2)
    {
        return new IntVector2(
            Mathf.RoundToInt(vec2.x),
            Mathf.RoundToInt(vec2.y));
    }
}