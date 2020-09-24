using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISkillOpenButton : EventTrigger
{
	private GameObject skillOpen;
	private UISkillButton[] uiSkillButtons;

    // Start is called before the first frame update
    private void Awake()
    {
		skillOpen = transform.Find("Button").gameObject;
		var skillsParent = transform.parent.Find("Skills");
		uiSkillButtons = new UISkillButton[2];
		for(int i = 0; i < uiSkillButtons.Length; ++i)
		{
			uiSkillButtons[i] = skillsParent.Find($"Skill{i}").GetComponentInChildren<UISkillButton>();
		}
    }

	private void Start()
	{
		CloseSkills();
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		OpenSkills();
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		CloseSkills();
	}

	private void OpenSkills()
	{
		skillOpen.SetActive(false);
		foreach(var ui in uiSkillButtons)
		{
			ui.Disp(true);
		}
	}

	private void CloseSkills()
	{
		skillOpen.SetActive(true);
		foreach (var ui in uiSkillButtons)
		{
			ui.Disp(false);
		}
	}
}
