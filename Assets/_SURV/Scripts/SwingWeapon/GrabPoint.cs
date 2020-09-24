using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPoint : MonoBehaviour
{

	private SwingWeapon swingWeapon;

	public void Initialize(SwingWeapon weapon)
	{
		swingWeapon = weapon;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var localPlayer = collision.GetComponent<LocalPlayer>();

		//Debug.Log("つうか");
		if ( localPlayer == null ) return;
		//Debug.Log("けんち");

	}
}
