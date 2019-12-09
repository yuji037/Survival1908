﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNpc : CBody
{
	private CActor target;

	public CActor Target { get { return target; } }

	[SerializeField]
	private float moveSpeedRate = 10f;

	[SerializeField]
	public AnimationCurve moveForwardSpeedCurve;
	[SerializeField]
	public AnimationCurve moveForwardJumpCurve;

	[SerializeField]
	private float bodyKnockbackPower = 10f;

	// Start is called before the first frame update
	protected override void Start()
    {
		base.Start();

		target = CLocalPlayer.Instance;
    }

    // Update is called once per frame
    protected override void Update()
    {
		base.Update();

		//if ( target == null ) return;

		//var distance = target.transform.position - transform.position;
		//var force = distance.normalized * moveSpeedRate;
		//rigidbdy2D.AddForce(force, ForceMode2D.Force);

		animator.SetFloat("Horizontal", direction.x);
		animator.SetFloat("Vertical", direction.y);
	}

	public void SetDirection(Vector2 _direction)
	{
		direction = _direction;
	}

	public void SetAIMovement(Vector2 _velocity, float elapsedTime)
	{
		var speedCurveFactor = moveForwardSpeedCurve.Evaluate(elapsedTime);
		var jumpCurveFactor = moveForwardJumpCurve.Evaluate(elapsedTime);

		rigidbdy2D.velocity = _velocity * moveSpeedRate * speedCurveFactor;

		baseSpriteTransform.localPosition = new Vector3(0f, jumpCurveFactor, 0f);
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		var body = collision.gameObject.GetComponent<CBody>();
		if ( body == null )
			return;

		SendDamage(body, new CAttackInfo(
			this,
			atkPower,
			collision.collider.ClosestPoint(transform.position),
			bodyKnockbackPower
			));

	}
}
