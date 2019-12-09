using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CDamageText : MonoBehaviour
{
	[SerializeField]
	Color color1 = Color.red;
	[SerializeField]
	Color color2 = Color.red;

	TextMeshPro textMeshPro;

	[SerializeField]
	public float remainTime = 3f;

	public float elapsedTime = 0f;

	private Vector3 velocity;

	[SerializeField]
	private Vector2 velocityMax;

	[SerializeField]
	private float colorChangeSpeed = 5f;

	private void Start()
	{
		if ( textMeshPro == null )
			textMeshPro = GetComponentInChildren<TextMeshPro>();

		velocity = new Vector3(
			Random.Range(-velocityMax.x, velocityMax.x),
			Random.Range(-velocityMax.y, velocityMax.y),
			0f);
	}

	private void Update()
	{
		transform.position += velocity * Time.deltaTime;

		var f = Mathf.PingPong(Time.time * colorChangeSpeed, 1f);

		textMeshPro.color = color1 * ( 1 - f ) + color2 * f;

		elapsedTime += Time.deltaTime;

		if( elapsedTime >= remainTime - 1f )
		{
			var fadeoutRate = remainTime - elapsedTime;
			var color = textMeshPro.color;
			color.a = Mathf.Min(color.a, fadeoutRate);
			textMeshPro.color = color;
		}
		if(elapsedTime >= remainTime )
		{
			elapsedTime = 0f;
			gameObject.SetActive(false);
		}
	}

}