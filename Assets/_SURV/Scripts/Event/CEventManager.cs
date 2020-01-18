using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CEventManager : MonoBehaviour
{
	TextAsset csvFile; // CSVファイル
	List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

	void Start()
	{
		string filePath = "EventData";
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
			Debug.Log($"説明：{line}");
		}

		// , で分割しつつ一行ずつ読み込み
		// リストに追加していく
		while ( reader.Peek() != -1 ) // reader.Peaekが-1になるまで
		{
			string line = reader.ReadLine(); // 一行ずつ読み込み
			Debug.Log($"内容：{line}");
			csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
		}

		// csvDatas[行][列]を指定して値を自由に取り出せる
		Debug.Log(csvDatas[0][1]);

	}
}
