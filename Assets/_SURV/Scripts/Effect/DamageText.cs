using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class DamageText : MonoBehaviour
{
	[Serializable]
	public class ColorPattern
	{
		public Color color1 = Color.red;
		public Color color2 = Color.red;
	}

	[SerializeField] private ColorPattern[] colorPatterns = default;
	[SerializeField] public float remainTime = 3f;
	[SerializeField] private Vector2 velocityMax;
	[SerializeField] private float colorChangeSpeed = 5f;

	private TextMeshPro textMeshPro;
	private int colorPatternIndex = 0;
	public float elapsedTime = 0f;
	private Vector3 velocity;

	private void Start()
	{
		if ( textMeshPro == null )
			textMeshPro = GetComponentInChildren<TextMeshPro>();

		velocity = new Vector3(
			Random.Range(-velocityMax.x, velocityMax.x),
			Random.Range(-velocityMax.y, velocityMax.y),
			0f);
	}

	public void Init(int patternIndex)
	{
		colorPatternIndex = patternIndex;
	}

	private void Update()
	{
		transform.position += velocity * Time.deltaTime;

		var pattern = colorPatterns[colorPatternIndex];
		var f = Mathf.PingPong(Time.time * colorChangeSpeed, 1f);

		textMeshPro.color = pattern.color1 * ( 1 - f ) + pattern.color2 * f;

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