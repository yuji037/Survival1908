using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;

public class UIBlockInput : SingletonMonoBehaviour<UIBlockInput>
{
	[SerializeField] private GameObject screen;

	public void SetActiveBlock(bool active)
	{
		screen.SetActive(active);
	}

	public async UniTask BlockForSeconds(float seconds)
	{
		SetActiveBlock(true);
		var endTime = Time.unscaledTime + seconds;
		await UniTask.WaitUntil(()=> Time.unscaledTime >= endTime);
		SetActiveBlock(false);
	}
}
