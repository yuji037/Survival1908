using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
	Alpha,
	Bravo,
}

public class AttackInfo
{
	public Chara	attacker;
	public float	effectPowerFactor;		// スキルなどの威力係数
	public Vector2	hitPosition;
	public float	knockbackPower;

	public AttackInfo(
		Chara	_attacker,
		float	_effectPowerFactor,
		Vector2	_hitPosition,
		float	_knockbackPower
		)
	{
		attacker			=	_attacker;
		effectPowerFactor	=	_effectPowerFactor;
		hitPosition			=	_hitPosition;
		knockbackPower		=	_knockbackPower;
	}
}