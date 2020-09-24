using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameTime : SingletonMonoBehaviour<IngameTime>
{
	private static float time;
	public static float Time { get => time; }
	public static float DeltaTime
	{
		get
		{
			if (!IngameCoordinator.Instance.IsBattleActive) { return 0f; }
			return UnityEngine.Time.deltaTime;
		}
	}

	private void Update()
	{
		if (!IngameCoordinator.Instance.IsBattleActive) { return; }

		time += UnityEngine.Time.deltaTime;
	}
}
