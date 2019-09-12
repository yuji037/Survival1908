using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CLocationStatus
{
	public string Name;
    public Color  DebugMapColor;

	public List<CSearchItemParam> SearchItemList = new List<CSearchItemParam>();

	public void AddSearchItemList(float fRate, string sItem){
		var param = new CSearchItemParam();
		param.Rate = fRate;
		param.Item = sItem;
		SearchItemList.Add(param);
	}

	public List<string> SearchItem(){
		var lsFindItem = new List<string>();
		foreach (var param in SearchItemList) {
			var random = UnityEngine.Random.Range(0f, 100f);
			if (random < param.Rate) {
				lsFindItem.Add(param.Item);
			}
		}

		return lsFindItem;
	}
}

[Serializable]
public class CSearchItemParam{
	public float Rate;
	public string Item;
}
