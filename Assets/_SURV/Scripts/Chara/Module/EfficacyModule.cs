using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EfficacyType
{
	None = 0,
	AddHP = 1,
	AddMP = 2,
	AddFood = 4,
	RegeneHP = 51,
	RegeneMP = 52,
	AttackCoefficient = 101,
	DefenceCoefficient = 102,
	SpeedCoefficient = 111,
	Hungry = 501,
}

public class EfficacyModule : IModuleTick
{
	private Dictionary<EfficacyType, EfficacyUnit> efficacyDict = new Dictionary<EfficacyType, EfficacyUnit>();
	private bool	modifiedEfficacy	= false;
	private Action	onModifiedEfficacy	= null;

	public void AddModifiedEvent(Action onModifiedEvent)
	{
		onModifiedEfficacy += onModifiedEvent;
	}

	public void Tick()
	{
		var endKeys = new List<EfficacyType>();
		foreach (var pair in efficacyDict)
		{
			if (pair.Value == null) { continue; }
			if (pair.Value.CheckEnd())
			{
				endKeys.Add(pair.Key);
			}
		}
		foreach(var key in endKeys)
		{
			efficacyDict[key] = null;
			modifiedEfficacy = true;
		}

		if (modifiedEfficacy)
		{
			modifiedEfficacy = false;
			onModifiedEfficacy?.Invoke();
		}
	}

	public void AddEfficacy(EfficacyType efficacyType, float value, float value2, float duration)
	{
		if (false == efficacyDict.ContainsKey(efficacyType))
		{
			efficacyDict.Add(efficacyType, null);
		}
		var newUnit = new EfficacyUnit(value, duration);
		efficacyDict[efficacyType] = newUnit;
		modifiedEfficacy = true;
	}

	public float GetEfficacy(EfficacyType efficacyType)
	{
		if(false == efficacyDict.ContainsKey(efficacyType) )
		{
			return 0f;
		}

		var totalValue = 0f;
		if(efficacyDict[efficacyType] != null)
		{
			totalValue = efficacyDict[efficacyType].Value;
		}
		return totalValue;
	}

	public void RemoveEfficacy(EfficacyType efficacyType)
	{
		if (false == efficacyDict.ContainsKey(efficacyType))
		{
			return;
		}
		if(efficacyDict[efficacyType] != null)
		{
			efficacyDict[efficacyType] = null;
			modifiedEfficacy = true;
		}
	}

	class EfficacyUnit
	{
		private float value;
		private float endTime;

		public float Value { get { return value; } }

		public EfficacyUnit(float value, float duration)
		{
			this.value = value;
			this.endTime = IngameTime.Time + duration;
		}

		public bool CheckEnd() { return IngameTime.Time >= endTime; }
	}
}

