using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiType
{
	Seek,
	Wander,
}

public class CAiModule
{
	[SerializeField]
	private AiType aiType;

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
			case AiType.Seek:
				if ( target != null )
				{

				}
				break;
		}
	}
}
