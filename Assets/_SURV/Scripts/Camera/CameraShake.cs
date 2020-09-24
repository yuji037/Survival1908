using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShake : Singleton<CameraShake>
{
	FollowCamera followCamera;

	public Vector3 shakeOffset;

	public StartDirectionType ShakeType;
	//[Range(0f, 1f)]
	public float Strength = 0.5f;
	//[Range(0f, 20f)]
	public float Frequency = 5f;
	//[Range(0f, 5f)]
	public float TimeLength = 1f;

	Vector3 defaultPos;

	Vector3 velocity = Vector3.zero;

	public float elasticity = 2f;

	public float dampFactor = 1f;

	public void Init(FollowCamera _followCamera)
	{
		followCamera = _followCamera;
	}

	public void OnUpdate()
	{
		var accel = -shakeOffset * elasticity;
		velocity += accel * Time.deltaTime;
		var rate = 1f - dampFactor * Time.deltaTime;
		rate = Mathf.Clamp(rate, 0f, 1f);
		velocity *= rate;
		shakeOffset += velocity * Time.deltaTime;
	}


	public void Shake(StartDirectionType shakeType, float strength)
	{
		float rad = 0f;

		// 振る方向の決定
		switch ( shakeType )
		{
			case StartDirectionType.DOWN:
				rad = Mathf.PI * 1.5f;
				break;
			case StartDirectionType.UP:
				rad = Mathf.PI * 0.5f;
				break;
			case StartDirectionType.LEFT:
				rad = Mathf.PI * 1f;
				break;
			case StartDirectionType.RIGHT:
				rad = 0f;
				break;
			case StartDirectionType.DOWNWARD:
				rad = Random.Range(Mathf.PI * 1f, Mathf.PI * 2f);
				break;
			case StartDirectionType.RANDOM_DIRECTION:
				rad = Random.Range(0f, Mathf.PI * 2f);
				break;
		}

		Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);

		velocity += dir * strength;
	}

	public void ShakeDirection(Vector3 dir, float strength)
	{
		if ( dir == Vector3.zero ) return;

		dir = dir.normalized;
		velocity += dir * strength;
	}

	public enum StartDirectionType
	{
		DOWN,
		UP,
		LEFT,
		RIGHT,
		DOWNWARD,
		RANDOM_DIRECTION,
	}
}
