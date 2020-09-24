using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharaStatusMasterData
{
	public float HP;
	public float MP;
	public float ATK;
	public float DEF;
	public float LvupExp;
	public float OneNpcExp;

	public CharaStatusMasterData(float hp, float mp, float atk, float def, float lvupExp, float oneEnemyExp)
	{
		HP = hp;
		MP = mp;
		ATK = atk;
		DEF = def;
		LvupExp = lvupExp;
		OneNpcExp = oneEnemyExp;
	}
}

public class CharaStatusTable
{
	private CharaStatusMasterData[] tables;

	private float damageFactor;
	private float defWeightFactor;

	public float NpcHPRate { get; private set; }

	public void Load()
	{
		try
		{
			var csvLines = CSVFileManager.LoadFile("PartyCharaStatusTable", 0);
			damageFactor = float.Parse(csvLines[8][0]);
			defWeightFactor = float.Parse(csvLines[9][0]);
			NpcHPRate = float.Parse(csvLines[13][0]);
			var tableLines = csvLines.GetRange(45, csvLines.Count - 45);
			TryCreateTable(tableLines);
		}
		catch (System.Exception e)
		{
			Debug.LogError(e);
		}
	}

	private void TryCreateTable(List<string[]> lines)
	{
		tables = new CharaStatusMasterData[lines.Count];
		Debug.Log($"csvLines.Count is {lines.Count}");
		for (int i = 0; i < tables.Length; ++i)
		{
			var words = lines[i];
			if (!int.TryParse(words[0], out var level))
			{
				Debug.LogError($"読み取りエラー {words[0]}");
				return;
			}
			if (i + 1 != level)
			{
				Debug.LogError($"読み取りエラー i+1:{i + 1} level:{level}");
				return;
			}
			tables[i] = new CharaStatusMasterData(
				float.Parse(words[1]),
				float.Parse(words[2]),
				float.Parse(words[3]),
				float.Parse(words[5]),
				float.Parse(words[11]),
				float.Parse(words[8])
				);
		}
	}

	public CharaStatusMasterData GetMasterData(int level)
	{
		return tables[level - 1];
	}

	public float CalculateDamage(float atk, float def)
	{
		return (atk * atk / (atk + def * defWeightFactor)) * damageFactor;
	}
}

[Serializable]
public class CustomStatusRate
{
	public float maxHpRate = 1f;
	public float atkRate = 1f;
	public float defRate = 1f;
}