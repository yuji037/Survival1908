using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIType
{
	Seek,
	Wander,
}

public class AIModule
{
	[SerializeField]
	private AIType aiType;

	private List<GameObject> targetList = new List<GameObject>();

	private GameObject target = null;
	private GameObject owner;

	[SerializeField]
	private float sightingDistance = 2.0f;
	private float sqrSightingDistance;

	private Vector2 inputMove = Vector2.zero;
	public Vector2 InputMove { get { return inputMove; } }

	private void Setup(GameObject _owner)
	{
		owner = _owner;
		sqrSightingDistance = sightingDistance * sightingDistance;
	}

	public void UpdateAI()
	{
		foreach ( var ta in targetList )
		{
			if ( sqrSightingDistance > Vector3.SqrMagnitude(ta.transform.position - owner.transform.position) )
			{
				target = ta;
			}
		}

		switch ( aiType )
		{
			case AIType.Seek:
				if ( target != null )
				{

				}
				break;
		}
	}
}
