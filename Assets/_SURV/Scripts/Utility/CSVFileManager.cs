using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVFileManager
{
	public static List<string[]> LoadFile(string resourcePath, int ignoreHeaderLinesCount)
	{
		var csvFile = Resources.Load(resourcePath) as TextAsset; // Resouces下のCSV読み込み
		StringReader reader = new StringReader(csvFile.text);

		// 最初の一定数行はデータ説明部分なので読み込まない
		for (int i = 1; i <= ignoreHeaderLinesCount; ++i)
		{
			if (reader.Peek() == -1)
			{
				Debug.LogError($"{resourcePath}は{i - 1}行で終わっています。");
				return null;
			}
			string line = reader.ReadLine();
		}

		var lines = new List<string[]>();

		// , で分割しつつ一行ずつ読み込み
		// リストに追加していく
		while (reader.Peek() != -1) // reader.Peekが-1になるまで
		{
			var readLine = reader.ReadLine(); // 一行ずつ読み込み
			var words = readLine.Split(',');
			lines.Add(words);
			//Debug.Log($"内容: {readLine}");
		}

		return lines;
	}

	public Dictionary<string, FieldEventUnit> LoadEvents()
	{
		var eventDictionary = new Dictionary<string, FieldEventUnit>();
		return eventDictionary;
	}
}
