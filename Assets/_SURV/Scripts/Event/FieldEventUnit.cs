using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UniRx.Async;

public enum EventBeginTrigger
{
	None = 0,
	DecideButton,
	EnterArea,
}

public class FieldEventUnit
{
	//==================================================================================
	// 列挙型
	//==================================================================================
	private enum Column
	{
		EventName,
		BeginTrigger,
		EventType,
		Content,
	}

	private enum EventType
	{
		DialogClear		= 1,
		Dialog			= 2,
		GetItem			= 3,
		CharaAnimation	= 4,
		WaitSeconds		= 20,
		FadeScreenColor	= 21,
	}

	//==================================================================================
	// フィールド
	//==================================================================================
	private EventBeginTrigger	beginTrigger;
	private int					currentLineIndex;
	private List<string[]>		lines;

	//==================================================================================
	// プロパティ
	//==================================================================================
	public EventBeginTrigger	BeginTrigger	{ get => beginTrigger; }
	public bool					ReadyToBegin	{ get => currentLineIndex == 0; }
	public bool					IsInProgress	{ get => currentLineIndex != 0; }

	//==================================================================================
	// コンストラクタ
	//==================================================================================
	public FieldEventUnit(List<string[]> lines)
	{
		currentLineIndex = 0;

		var firstLine = lines[0];
		if ( false == int.TryParse(firstLine[(int)Column.BeginTrigger], out int result) )
		{
			Debug.LogError($"intとして読み取れませんでした。「{firstLine[(int)Column.BeginTrigger]}」");
			return;
		}
		beginTrigger = (EventBeginTrigger)result;

		this.lines = new List<string[]>();
		foreach ( var line in lines )
		{
			if ( string.IsNullOrWhiteSpace(line[(int)Column.EventType]) )
			{
				// 空白行として判定
				continue;
			}
			this.lines.Add(new string[] { line[(int)Column.EventType], line[(int)Column.Content] });
		}
	}

	//==================================================================================
	// メソッド
	//==================================================================================
	public async UniTask<bool> Next()
	{
		if(currentLineIndex >= lines.Count )
		{
			Debug.Log("終了");
			UIDialogMessage.Instance.ShowOffWindow();
			currentLineIndex = 0;
			return false;
		}

		var line = lines[currentLineIndex];
		currentLineIndex++;

		if ( false == int.TryParse(line[0], out int eventType) )
		{
			Debug.LogError($"intとして読み取れませんでした。「{line[0]}」");
			return false;
		}

		DebugLogContent(line, false);

		UIIngame.Instance.SwitchUI(-1);
		IngameCoordinator.Instance.SetBattleActive(false);

		var content = line[1];
		switch ( (EventType)eventType )
		{
			case EventType.DialogClear:
				UIDialogMessage.Instance.ShowOnWindowContinuous();
				UIDialogMessage.Instance.ClearText();
				UIDialogMessage.Instance.AddText(content);
				break;
			case EventType.Dialog:
				UIDialogMessage.Instance.ShowOnWindowContinuous();
				UIDialogMessage.Instance.AddText(content);
				break;
			case EventType.GetItem:
				if (!GetItem(content))
				{
					return false;
				}
				return await Next(); // すぐ次の行へ
			case EventType.CharaAnimation:
				UIDialogMessage.Instance.ShowOffWindow();
				SetCharaMotion(content);
				return await Next(); // すぐ次の行へ
			case EventType.WaitSeconds:
				UIDialogMessage.Instance.ShowOffWindow();
				await WaitSeconds(content);
				return await Next(); // 終わったら次の行へ
			case EventType.FadeScreenColor:
				await FadeScreenColor(content);
				return await Next(); // 終わったら次の行へ
		}

		return true;
	}

	private bool GetItem(string content)
	{
		var getItems = content.Split('#');
		foreach (var str in getItems)
		{
			var words = str.Split(':');

			if (!TryIntParse(words[1], out int itemCount))
			{
				// 読み取り失敗
				DebugLogContent(new string[] { content }, true);
				return false;
			}
			ItemInventry.Instance.AddChangeItemCount(words[0], itemCount);
		}
		return true;
	}

	private bool SetCharaMotion(string content)
	{
		var words = content.Split(':');
		if(!TryIntParse(words[0], out int charaId))
		{
			// 読み取り失敗
			DebugLogContent(new string[] { content }, true);
			return false;
		}
		var chara = CharasController.Instance.GetChara(charaId);
		chara.FadeAnimatorState(words[1]);
		return true;
	}

	private async UniTask WaitSeconds(string content)
	{
		if (!TryFloatParse(content, out float seconds))
		{
			// 読み取り失敗
			DebugLogContent(new string[] { content }, true);
			seconds = 1f;
		}
		await UIBlockInput.Instance.BlockForSeconds(seconds);
	}

	private async UniTask FadeScreenColor(string content)
	{
		UIBlockInput.Instance.SetActiveBlock(true);
		var afterColor = Color.clear;
		switch (content)
		{
			case "White":	afterColor = Color.white;	break;
			case "Black":	afterColor = Color.black;	break;
			case "Red":		afterColor = Color.red;		break;
			case "Clear":	afterColor = Color.clear;	break;
		}
		await UIFadeScreen.Instance.Fade(UIFadeScreen.Screen.Ingame, afterColor, 0.5f);
		UIBlockInput.Instance.SetActiveBlock(false);
	}

	public async UniTask Skip()
	{
		await UIFadeScreen.Instance.Fade(UIFadeScreen.Screen.Ingame, Color.clear, 0f);
		UIDialogMessage.Instance.ShowOffWindow();
		UIBlockInput.Instance.SetActiveBlock(false);
	}

	private bool TryIntParse(string s, out int result)
	{
		if (false == int.TryParse(s, out result))
		{
			Debug.LogError($"intとして読み取れませんでした。「{s}」");
			return false;
		}
		return true;
	}

	private bool TryFloatParse(string s, out float result)
	{
		if (false == float.TryParse(s, out result))
		{
			Debug.LogError($"floatとして読み取れませんでした。「{s}」");
			return false;
		}
		return true;
	}

	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	private void DebugLogContent(string[] contents, bool isErrorLog)
	{
		var str = string.Empty;
		foreach(var content in contents)
		{
			str += content;
			str += " ";
		}
		if (isErrorLog)
		{
			Debug.LogError($"Event: {str}");
		}
		else
		{
			Debug.Log($"Event: {str}");
		}
	}
}