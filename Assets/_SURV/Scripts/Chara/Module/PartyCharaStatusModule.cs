using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public sealed class PartyCharaStatusModule : StatusModule
{
	//=============================================================
	// 定数
	//=============================================================
	private const float REGENE_HP_INTERVAL = 1f;

	//=============================================================
	// フィールド
	//=============================================================
	private ValueModule mpModule;
	private ValueModule foodModule;
	private EquipmentModule equipmentModule;
	private float needLvupExp;
	private float combatExp;
	private float foodFullness;
	private float regeneHPElapsedTime = 0f;
	private float regeneHPEfficacy = 0f;
	private float regeneMPEfficacy = 0f;
	private bool isHungry = false;

	//=============================================================
	// プロパティ
	//=============================================================
	public override float MP { get { return mpModule.Value; } }
	public override float MaxMP { get { return mpModule.MaxValue; } }
	public float Food { get { return foodModule.Value; } }
	public float NeedLvupExp { get { return needLvupExp; } }
	public float CombatExp { get { return combatExp; } }

	//=============================================================
	// メソッド
	//=============================================================

	public override void Enable(Chara owner, CustomStatusRate customStatusRate, EfficacyModule efficacyModule)
	{
		mpModule = new ValueModule();
		foodModule = new ValueModule();
		foodModule.SetMaxValue(100f);
		base.Enable(owner, customStatusRate, efficacyModule);
		efficacyModule.AddModifiedEvent(UpdateRegeneEfficacy);
	}

	public void AddModifiedMPEvent(Action ev)
	{
		mpModule.AddModifiedEvent(ev);
	}
	public void AddModifiedFoodEvent(Action ev)
	{
		foodModule.AddModifiedEvent(ev);
	}
	public override void Tick()
	{
		base.Tick();
		mpModule.Tick();
		foodModule.Tick();

		RegeneHP();
		RegeneMP();

		// 腹減り（時間経過）
		AddChangeFoodfullness(-IngameTime.DeltaTime * 0.2f);
		CheckHungry();
	}

	public override void InitStatus(CustomStatusRate customStatusRate)
	{
		SetLevelStatus();
	}

	public override void InitContinueStatus(float hp, float mp, float food)
	{
		AddChangeHP(hp - this.HP);
		AddChangeMP(mp - this.MP);
		AddChangeFoodfullness(food - this.Food);
	}

	public void SetEquipmentModule(EquipmentModule equipmentModule)
	{
		this.equipmentModule = equipmentModule;
	}

	private void SetLevelStatus()
	{
		SetMaxHP(statusMasterData.HP);
		SetMaxMP(statusMasterData.MP);
		needLvupExp = statusMasterData.LvupExp;
		atk = statusMasterData.ATK;
		def = statusMasterData.DEF;
	}

	public override void ChangeCombatExp(float diff)
	{
		combatExp += diff;
		UIInstantMessage.Instance.RequestMessage($"Exp:+{diff.ToString("f0")}");
		if (combatExp >= needLvupExp)
		{
			combatExp = 0f;
			int newLevel = Level + 1;
			SetLevel(Level + 1);
			statusMasterData = statusTable.GetMasterData(newLevel);
			SetLevelStatus();
		}
	}

	public void RegeneHP()
	{
		regeneHPElapsedTime += IngameTime.DeltaTime;
		if (regeneHPElapsedTime >= REGENE_HP_INTERVAL)
		{
			regeneHPElapsedTime = 0f;
			float regeneHP;
			if (isHungry)
			{
				regeneHP = MaxHP * -0.01f + regeneHPEfficacy;
			}
			else
			{
				regeneHP = MaxHP * 0.01f + regeneHPEfficacy;
			}
			AddChangeHP(regeneHP);
		}
	}

	private void RegeneMP()
	{
		float regeneMP;
		if (isHungry)
		{
			regeneMP = regeneMPEfficacy;
		}
		else
		{
			regeneMP = MaxMP * 0.04f + regeneMPEfficacy;
		}

		AddChangeMP(IngameTime.DeltaTime * regeneMP);
	}

	private void CheckHungry()
	{
		var currentIsHungry = Food <= 0f;
		if(isHungry != currentIsHungry)
		{
			if (currentIsHungry)
			{
				// 半永久デバフ
				efficacyModule.AddEfficacy(EfficacyType.Hungry, 1f, 0f, float.PositiveInfinity * 0.5f);
			}
			else
			{
				efficacyModule.RemoveEfficacy(EfficacyType.Hungry);
			}
		}
		isHungry = currentIsHungry;
	}

	private void UpdateRegeneEfficacy()
	{
		regeneHPEfficacy = efficacyModule.GetEfficacy(EfficacyType.RegeneHP);
		regeneMPEfficacy = efficacyModule.GetEfficacy(EfficacyType.RegeneMP);
	}

	private void SetMaxMP(float value)
	{
		mpModule.SetMaxValue(value);
	}

	public override void AddChangeMP(float delta)
	{
		mpModule.SetValue(mpModule.Value + delta);
	}

	private void AddChangeMaxMP(float delta)
	{
		mpModule.SetMaxValue(mpModule.MaxValue + delta);
	}

	public override bool CanConsumeMP(float useMp)
	{
		return useMp <= mpModule.Value;
	}

	public override void ConsumeMP(float useMp)
	{
		mpModule.SetValue(mpModule.Value - useMp);
	}

	/// <summary>
	/// 満腹度変化（増減両方可能）
	/// </summary>
	/// <param name="diff"></param>
	public void AddChangeFoodfullness(float diff)
	{
		foodModule.SetValue(foodModule.Value + diff);
	}

	public override float GetAtk()
	{
		var atk = base.GetAtk();
		atk += equipmentModule.GetPlusAtk();
		return atk;
	}
	public override float GetDef()
	{
		var def = base.GetAtk();
		def += equipmentModule.GetPlusDef();
		return def;
	}
}
