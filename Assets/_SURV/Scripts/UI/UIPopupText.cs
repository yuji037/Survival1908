using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupText : MonoBehaviour
{
	[SerializeField] private Text text = default;
	[SerializeField] private Vector2 positionCorrectSize = new Vector2(200f, 200f);

	private bool isInit = false;
	private Vector2 screenSize;

	private void Init()
	{
		screenSize = new Vector2(Screen.width, Screen.height) * 0.5f;
	}

	public void Popup(Vector2 position, string message)
	{
		if ( !isInit )
		{
			Init();
			isInit = true;
		}
		this.text.text = message;

		// スクリーン中央に寄せる補正
		var judgeCorner = position - screenSize;
		var pos = position;
		if (judgeCorner.x < 0f) { pos.x += positionCorrectSize.x; }
		else { pos.x -= positionCorrectSize.x; }
		if (judgeCorner.y < 0f) { pos.y += positionCorrectSize.y; }
		else { pos.y -= positionCorrectSize.y; }

		gameObject.SetActive(true);
		transform.position = pos;
	}

	public void Deactive()
	{
		if (!gameObject.activeSelf) { return; }
		gameObject.SetActive(false);
	}
}
