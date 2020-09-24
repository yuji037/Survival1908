using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class StatusModule : MonoBehaviour
{
	[SerializeField] private int level = 1;
	[SerializeField] private float maxHP = 20f;

	private ValueModule hpModule;
	protected static CharaStatusTable statusTable;
	protected CharaStatusMasterData statusMasterData;

	protected EfficacyModule efficacyModule;
	private Chara owner;
	protected float atk = 3f;
	protected float def;

	public int Level { get { return level; } }
	public float HP { get { return hpModule.Value; } }
	public float MaxHP { get { return hpModule.MaxValue; } }
	public abstract float MP { get; }
	public abstract float MaxMP { get; }

	public virtual void Enable(Chara owner, CustomStatusRate customStatusRate, EfficacyModule efficacyModule)
	{
		this.owner = owner;
		this.efficacyModule = efficacyModule;

		LoadMasterData();
		hpModule = new ValueModule();
		atk = statusMasterData.ATK;
		def = statusMasterData.DEF;
		InitStatus(customStatusRate);
	}

	public void LoadMasterData()
	{
		if (statusTable == null)
		{
			statusTable = new CharaStatusTable();
			statusTable.Load();
		}
		statusMasterData = statusTable.GetMasterData(level);
	}

	public void AddModifiedHPEvent(Action ev)
	{
		hpModule.AddModifiedEvent(ev);
	}

	public virtual void Tick()
	{
		hpModule.Tick();
	}

	public abstract void InitStatus(CustomStatusRate customStatusRate);
	public abstract void InitContinueStatus(float hp, float mp, float food);

	public void SetLevel(int level)
	{
		this.level = level;
	}

	public virtual void ChangeCombatExp(float diff) { }

	public virtual float GetAtk()
	{
		return atk * (1f + efficacyModule.GetEfficacy(EfficacyType.AttackCoefficient));
	}

	public virtual float GetDef()
	{
		return def * (1f + efficacyModule.GetEfficacy(EfficacyType.DefenceCoefficient));
	}

	public void ReceiveDamage(AttackInfo attackInfo)
	{
		var attacker = attackInfo.attacker;
		var atk = attacker.StatusModule.GetAtk() * attackInfo.effectPowerFactor;
		var damage = statusTable.CalculateDamage(atk, GetDef());

		damage *= Random.Range(0.8f, 1.2f);
		var intDamage = Mathf.CeilToInt(damage);

		AddChangeHP(-intDamage);

		DamageTextManager.Instance.DispDamage(intDamage, attackInfo.hitPosition, owner.TeamType);
	}

	public void AddChangeHP(float delta)
	{
		hpModule.SetValue(hpModule.Value + delta);
	}
	public void AddChangeMaxHP(float delta)
	{
		hpModule.SetMaxValue(hpModule.MaxValue + delta);
	}

	public void SetMaxHP(float maxHP)
	{
		this.hpModule.SetMaxValue(maxHP);
	}

	public virtual bool CanConsumeMP(float useMp)
	{
		// 基底クラスにはMP消費の概念を作っておらず、無限に使える
		return true;
	}

	public virtual void ConsumeMP(float useMP) { }

	public virtual void AddChangeMP(float delta) { }
}