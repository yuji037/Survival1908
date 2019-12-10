﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CDamageTextManager : CSingletonMonoBehaviour<CDamageTextManager>
{
	[SerializeField]
	int				textCount = 10;

	[SerializeField]
	GameObject		damageTextElement;

	GameObject[]	damageTexts;

	private void Start()
	{
		damageTexts = new GameObject[textCount];
		for ( int i = 0; i < textCount; ++i )
		{
			damageTexts[i] = Instantiate(damageTextElement, transform);
			damageTexts[i].SetActive(false);
		}
	}

	public void DispDamage(int damage, Vector2 position)
	{
		int oldestTextIndex = 0;
		float maxElapsedTime = 0f;
		int useIndex = -1;
		for(int i = 0; i < damageTexts.Length; ++i )
		{
			if ( damageTexts[i].activeSelf )
			{
				var dmgText = damageTexts[i].GetComponent<CDamageText>();
				if ( maxElapsedTime <= dmgText.elapsedTime )
				{
					maxElapsedTime = dmgText.elapsedTime;
					oldestTextIndex = i;
				}

			}
			else
			{
				useIndex = i;
				break;
			}
		}
		if ( useIndex == -1 )
		{
			useIndex = oldestTextIndex;
		}

		var damageText = damageTexts[useIndex];
		damageText.transform.position = position;
		damageText.GetComponentInChildren<TextMeshPro>().text = damage.ToString();

		damageText.SetActive(true);
	}
}