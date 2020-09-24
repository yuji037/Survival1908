using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattle : SingletonMonoBehaviour<UIBattle>, ISwitchUI
{
	[SerializeField] private UIPartyStatus uiPartyStatus = default;
	[SerializeField] private UIItemShortcutWindow uiItemShortcutWindow = default;
	[SerializeField] private UIDragItem uiDropItem = default;
	[SerializeField] private UISkillButton[] uiSkillButtons = default;

	public bool IsDisplay { get => gameObject.activeSelf; }
	public IReadOnlyList<UISkillButton> UISkillButtons { get { return uiSkillButtons; } }

	public void Initialize()
	{
		GetComponent<Canvas>().enabled = true;
		uiPartyStatus.Initialize();
		uiItemShortcutWindow.Init(uiDropItem);
	}

	public void Disp(bool bDisp)
	{
		gameObject.SetActive(bDisp);
		if ( bDisp )
		{
			UpdateUI();
		}
	}

	public void UpdateUI()
	{
		uiItemShortcutWindow.Load();
	}
}
