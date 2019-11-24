using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class CMapData
{
	public CMapCellArray[] map;

//	[SerializeField, HideInInspector]
//	private int[,] serializableMap;

//	public void SetMap(int[,] newMap) {
//		map = newMap;
//	}
//
//	public void OnBeforeSerialize()
//	{
////		this.serializableMap = this.map == null ? null : this.map.Select(subList => new ListInt(subList)).ToList();
//		this.serializableMap = this.map;
//	}
//
//	public void OnAfterDeserialize()
//	{
////		this.map = this.serializableMap == null ? null : this.serializableMap.Select(listInt => listInt.data).ToList();
//		this.map = this.serializableMap;
//	}
}

[System.Serializable]
public class CMapCellArray{
	public CMapCell[] data;

	public int Leength{
		get{ return data.Length; }
	}

	public CMapCell this[int i]{
		get{ return data[i]; }
		set{ data[i] = value; }
	}
}

[System.Serializable]
public class CMapCell
{
    public int iLocationType;
	public int iEncountType;
	public CFacility cFacility;
}
