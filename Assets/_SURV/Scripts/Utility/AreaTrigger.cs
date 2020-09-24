using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AreaTrigger : MonoBehaviour
{
	public Action<Collider2D> onTriggerEnter2D;
	public Action<Collider2D> onTriggerExit2D;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		onTriggerEnter2D?.Invoke(collision);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		onTriggerExit2D?.Invoke(collision);
	}
}
