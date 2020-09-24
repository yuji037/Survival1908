using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : MonoBehaviour, ISwitchUI
{
	[SerializeField] private InputField debugItemInputField = default;

	public bool IsDisplay { get => gameObject.activeSelf; }

	private void Awake()
	{
		GetComponent<Canvas>().enabled = true;
	}

	public void Disp(bool isDisp)
	{
		gameObject.SetActive(isDisp);
	}

	public void OnClickDebugButton(int index)
	{
		switch (index)
		{
			case 0:
				DebugMenu.SaveReset();
				break;
			case 1:
				var line = debugItemInputField.text;
				var words = line.Split(' ');
				if(words.Length == 2
					&& int.TryParse(words[1], out var count))
				{
					DebugMenu.GetItem(words[0], count);
				}
				else
				{
					UIInstantMessage.Instance.RequestMessage($"デバッグ：無効な入力[{line}]");
				}
				break;
			case 2:
				var pl = LocalPlayer.Instance;
				var plStatus = pl.StatusModule as PartyCharaStatusModule;
				plStatus.ChangeCombatExp(plStatus.NeedLvupExp - plStatus.CombatExp);
				break;
		}
	}
}
