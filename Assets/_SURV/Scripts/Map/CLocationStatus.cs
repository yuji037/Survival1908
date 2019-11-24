using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CLocationStatus
{
	public string name;
	public Color  debugMapColor;

	public List<CSearchItemParam> searchItemList = new List<CSearchItemParam>();

	public void AddSearchItemList(float fRate, string sItem){
		var param = new CSearchItemParam();
		param.rate = fRate;
		param.itemName = sItem;
		searchItemList.Add(param);
	}

	public List<string> SearchItem(){
		var lsFindItem = new List<string>();
		foreach (var param in searchItemList) {
			var random = UnityEngine.Random.Range(0f, 100f);
			if (random < param.rate) {
				lsFindItem.Add(param.itemName);
			}
		}

		return lsFindItem;
	}
}

[Serializable]
public class CSearchItemParam{
	public float rate;
	public string itemName;
}
