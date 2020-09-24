using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ValueModule
{
	private bool modifiedValue = false;
	private Action onModifiedValue;
	private float value;
	private float maxValue;

	public float Value { get { return value; } }
	public float MaxValue { get { return maxValue; } }

	public void AddModifiedEvent(Action onModifiedEvent)
	{
		onModifiedValue += onModifiedEvent;
	}

	public void Tick()
	{
		if (modifiedValue)
		{
			modifiedValue = false;
			onModifiedValue?.Invoke();
		}
	}

	public void SetValue(float val)
	{
		value = Mathf.Clamp(val, 0f, MaxValue);
		modifiedValue = true;
	}

	public void SetMaxValue(float val)
	{
		maxValue = val;
		modifiedValue = true;
	}
}
