using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharasController : Singleton<CharasController>
{
	private List<Chara> charas = new List<Chara>();
	private LinkedList<Chara> addCharas = new LinkedList<Chara>();
	private LinkedList<Chara> removeCharas = new LinkedList<Chara>();
	private Dictionary<int, Chara> idCharaDict = new Dictionary<int, Chara>();

	private int[] assignIDs;

	public void Init()
	{
		assignIDs = Enum.GetValues(typeof(CharaID)) as int[];
	}

	public void RegisterChara(Chara chara, CharaBorder charaBorder)
	{
		// 全キャラ重複無いようにID割り当てる
		assignIDs[(int)charaBorder]++;
		var id = assignIDs[(int)charaBorder];
		chara.InitID(id);
		addCharas.AddLast(chara);
		idCharaDict.Add(id, chara);
	}

	public void UnregisterChara(Chara chara)
	{
		removeCharas.AddLast(chara);
		idCharaDict.Remove(chara.ID);
	}

	public Chara GetChara(int id)
	{
		if(idCharaDict.TryGetValue(id, out var chara))
		{
			return chara;
		}
		Debug.LogError($"ID:{id} のキャラは登録されていないようです。");
		return null;
	}

	public void Tick()
	{
		if(false == IngameCoordinator.Instance.IsBattleActive) { return; }

		foreach (var chara in addCharas)
		{
			chara.Begin();
			charas.Add(chara);
		}
		addCharas.Clear();

		foreach (var chara in removeCharas)
		{
			charas.Remove(chara);
		}
		removeCharas.Clear();

		foreach ( var chara in charas )
		{
			chara.Tick();
			if ( chara.IsRemove )
			{
				removeCharas.AddLast(chara);
			}
		}
	}

	public void FixedTick()
	{
		foreach (var chara in charas)
		{
			chara.FixedTick();
		}
	}

	public void Clear()
	{
		charas.Clear();
	}
}
