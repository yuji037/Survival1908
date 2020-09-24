using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FieldEventManager : Singleton<FieldEventManager>
{
	private TextAsset							csvFile;			// CSVファイル
	private Dictionary<string, FieldEventUnit>	eventDictionary = new Dictionary<string, FieldEventUnit>();

	private bool isInitialized = false;
	private FieldEvent fieldEventCanProceed;
	public FieldEvent EventCanProceed { get => fieldEventCanProceed; }

	public void Load()
	{
		string filePath = "FieldEventData";
		csvFile = Resources.Load(filePath) as TextAsset; // Resouces下のCSV読み込み
		StringReader reader = new StringReader(csvFile.text);

		// 1～10行はデータ説明部分なので読み込まない
		for(int i = 1; i <= 10; ++i )
		{
			if(reader.Peek() == -1 )
			{
				Debug.LogError($"{filePath}は{i - 1}行で終わっています。");
				return;
			}
			string line = reader.ReadLine();
			//Debug.Log($"説明：{line}");
		}

		string reserveLine = null;
		// , で分割しつつ一行ずつ読み込み
		// リストに追加していく
		while ( reader.Peek() != -1 ) // reader.Peekが-1になるまで
		{
			string eventName = null;
			List<string[]> lines = new List<string[]>();
			while ( reader.Peek() != -1 ) // reader.Peekが-1になるまで
			{
				string readLine;
				if(reserveLine != null )
				{
					readLine = reserveLine;
					reserveLine = null;
				}
				else
				{
					readLine = reader.ReadLine(); // 一行ずつ読み込み
				}
				//Debug.Log($"内容：{readLine}");
				var words = readLine.Split(',');

				string _eventName = words[0];
				if ( false == string.IsNullOrWhiteSpace(_eventName) )
				{
					if ( eventName != null )
					{
						// すでにイベント名取得済みで、次のイベント名の行に来たので終わり
						reserveLine = readLine;
						break;
					}
					else
					{
						// イベント名を取得
						eventName = _eventName;
					}
				}
				lines.Add(words);
			}

			eventDictionary[eventName] = new FieldEventUnit(lines);
		}
	}

	public FieldEventUnit GetEventUnit(string eventName)
	{
		if(false == isInitialized )
		{
			Load();
		}
		if(eventDictionary.TryGetValue(eventName, out var ev))
		{
			return ev;
		}
		Debug.LogError($"EventName:{eventName}に合致するイベントがありません。");
		return null;
	}

	public void RegisterEventCanProceed(FieldEvent ev)
	{
		fieldEventCanProceed = ev;
	}

	public void UnregisterEventCanProceed(FieldEvent ev)
	{
		if (fieldEventCanProceed == ev)
		{
			fieldEventCanProceed = null;
		}
	}
}
