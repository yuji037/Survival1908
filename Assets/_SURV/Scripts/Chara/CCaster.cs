using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
	Alpha,
	Bravo,
}

public class CCaster : CActor
{
	[SerializeField]
	private Team teamType;
	public Team TeamType { get { return teamType; } }

	public Transform baseSpriteTransform;

	protected bool isAttacking = false;

	[SerializeField]
	public AnimationCurve stepForwardCurve;

	protected float atkPower = 3f;

	protected override void Awake()
	{
		base.Awake();
		baseSpriteTransform = transform.Find("BaseSprite");
	}

	public bool IsEnemy(CCaster other)
	{
		return teamType != other.teamType;
	}

	public void SendDamage(CBody target, CAttackInfo attackInfo)
	{
		if ( false == IsEnemy(target) )
			return;

		attackInfo.attackPower *= Random.Range(0.8f, 1.2f);

		target.ReceiveDamage(attackInfo);

	}
}

public class CAttackInfo
{
	public CCaster	attacker;
	public float	attackPower;
	public Vector2	hitPosition;
	public float	knockbackPower;

	public CAttackInfo(
		CCaster	_attacker,
		float	_attackPower,
		Vector2	_hitPosition,
		float	_knockbackPower
		)
	{
		attacker		=	_attacker;
		attackPower		=	_attackPower;
		hitPosition		=	_hitPosition;
		knockbackPower	=	_knockbackPower;
	}
}