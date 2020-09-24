using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DebugMenu
{
	public static void SaveReset()
	{
		SaveData.Clear();
		SaveData.Save();
		LocalPlayer.Instance.RequestNoSave();
		UIInstantMessage.Instance?.RequestMessage($"デバッグ: リセット");
	}

	public static void GetItem(string itemIDOrName, int delta)
	{
		var itemStatus = ItemDataMan.Instance.GetItemStatusById(itemIDOrName);
		if (itemStatus == null)
		{
			itemStatus = ItemDataMan.Instance.GetItemStatusByName(itemIDOrName);
			if ( itemStatus == null)
			{
				UIInstantMessage.Instance.RequestMessage($"デバッグ：Item情報取得できませんでした。入力:{itemIDOrName}");
				return;
			}
		}

		ItemInventry.Instance.AddChangeItemCount(itemStatus.ID, delta);
	}

	public static void BuffTest()
	{
		LocalPlayer.Instance.EfficacyModule.AddEfficacy(EfficacyType.AttackCoefficient, 3f, 0f, 10f);
	}

	[Conditional("UNITY_EDITOR")]
	public static void InputDebug()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			BuffTest();
		}
	}
}
