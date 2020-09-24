using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Async;

public class UIFadeScreen : SingletonMonoBehaviour<UIFadeScreen>
{
	public enum Screen
	{
		Overlay,	// 最前面
		Ingame,		// インゲームメッセージの後ろ
	}

	[SerializeField] private Image commonFadeImage;

	public async UniTask Fade(Screen screen, Color afterColor, float fadeDuration)
	{
		Image fadeImage = null;
		switch (screen)
		{
			case Screen.Overlay:
				fadeImage = commonFadeImage;
				break;
			case Screen.Ingame:
				// 一旦同じもの使う
				fadeImage = commonFadeImage;
				break;
		}
		await Fade(fadeImage, afterColor, fadeDuration);
	}

	private async UniTask Fade(Image fadeImage, Color afterColor, float fadeDuration)
	{
		var beforeColor = fadeImage.color;
		for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
		{
			var rate = t / fadeDuration;
			fadeImage.color =
				beforeColor * (1f - rate) +
				afterColor * rate;
			fadeImage.raycastTarget = true;
			await UniTask.Yield(PlayerLoopTiming.Update);
		}
		fadeImage.color = afterColor;
		fadeImage.raycastTarget = afterColor.a != 0f;
	}
}
