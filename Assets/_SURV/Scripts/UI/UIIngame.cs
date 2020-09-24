using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

interface ISwitchUI
{
	bool IsDisplay { get; }
	void Disp(bool isDisp);
}

public class UIIngame : SingletonMonoBehaviour<UIIngame>
{
	[SerializeField] private UIBattle uiBattle;
	[SerializeField] private UIInventry uiInventry;
	[SerializeField] private UICraftMan uiCraftMan;
	[SerializeField] private UIOption uiOption;
	[SerializeField] private GameObject uiIngameCommon;

	private List<ISwitchUI> uis = new List<ISwitchUI>();

	public void Initialize()
	{
		UIFieldPopup.Initialize();

		uiBattle.Initialize();
		uiInventry.Initialize();
		uiCraftMan.Initialize();

		uis.Add(uiBattle);
		uis.Add(uiInventry);
		uis.Add(uiCraftMan);
		uis.Add(uiOption);
	}

	/// <summary>
	/// 時間停止/解除を伴うUI切り替え
	/// </summary>
	public void OnClickSwitchUIButton(int uiIndex)
	{
		SwitchUI(uiIndex);
		IngameCoordinator.Instance.SetTimeActive(uiIndex == 0);
	}

	// index == -1 で全表示OFF
	public void SwitchUI(int index)
	{
		for(int i = 0; i < uis.Count; ++i)
		{
			bool isDisp = i == index;
			if (uis[i].IsDisplay != isDisp) uis[i].Disp(isDisp);
		}

		uiIngameCommon.gameObject.SetActive(index >= 0);
	}
}
