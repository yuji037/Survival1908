using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISkillButton : EventTrigger
{
	private Image icon = default;
	private int attackModuleIndex;
	private Action<int> attackCallback;
	private Image[] renderImages;

	private void Awake()
	{
		renderImages = GetComponentsInChildren<Image>();
	}

	public void Init(int attackModuleIndex, Action<int> attackCallback)
	{
		icon = transform.Find("Icon").GetComponent<Image>();

		this.attackModuleIndex = attackModuleIndex;
		this.attackCallback = attackCallback;
	}

	public void SetSkillInfo(int skillID)
	{
		var skill = SkillLoader.Instance.Load(skillID);
		icon.sprite = skill.icon;
	}

	public void Disp(bool isDisp)
	{
		foreach(var ren in renderImages)
		{
			ren.enabled = isDisp;
		}
	}

	public override void OnDrop(PointerEventData eventData)
	{
		attackCallback.Invoke(attackModuleIndex);
	}
}
