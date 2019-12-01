using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBody : CCaster
{

	public void ReceiveDamage()
	{
		Debug.Log("ヒット");
	}

	public void SetVelocity(Vector2 velocity)
	{
		var nowVelocity = rigidbdy2D.velocity;
		var needForce = velocity - nowVelocity;
		rigidbdy2D.AddForce(needForce, ForceMode2D.Impulse);
	}
}
