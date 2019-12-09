using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// 1個のスキルを発動する。ゲーム中に付け替え可能。
/// </summary>
public class CAttackModule
{
	private CCaster owner;
	private CSkill skill;
	private SkillType skillType;

	private bool isInStartAction = false;

	public void Init(CCaster _owner)
	{
		owner = _owner;
	}

	public void SetSkill(int skillId)
	{
		skill = CSkillLoader.Instance.Load(skillId);

		//if( skill.id < 20000 )
		//	skillType = SkillType.Melee;
		//else
			skillType = SkillType.Range;


	}

	public IEnumerator AttackCoroutine()
	{
		switch ( skillType )
		{
			case SkillType.Melee:
				yield return owner.StartCoroutine(MeleeAttackCoroutine());
				break;
			case SkillType.Range:
				yield return owner.StartCoroutine(RangeAttackCoroutine());
				break;
		}
	}

	IEnumerator MeleeAttackCoroutine()
	{
		////for(float t = 0f; t < )
		//var direction = owner.DirectionRightAngle;
		//var attackRot = Quaternion.FromToRotation(Vector2.down, direction);
		//var originPos = (Vector2)owner.transform.position + direction.normalized;
		//var actEffObj = GameObject.Instantiate(skill.actionEffect, originPos, attackRot);
		//GameObject.Destroy(actEffObj, 3f);

		//var hits = Physics2D.BoxCastAll(originPos, skill.rangeRect, 0f, direction, 0f);

		//foreach ( var hit in hits )
		//{
		//	if ( hit.transform != null )
		//	{
		//		var body = hit.transform.GetComponentInChildren<CBody>();
		//		if ( body != null && CCaster.IsOppositeTeam(owner, body) )
		//		{
		//			var damage = skill.attackPower;
		//			body.ReceiveDamage(damage);
		//			var distance = body.transform.position - owner.transform.position;
		//			if ( distance == Vector3.zero ) distance = new Vector3(0f, 0f, -1f);
		//			body.SetForce(distance.normalized * skill.knockbackPower);

		//			var hitEffObj = GameObject.Instantiate(skill.hitEffect, hit.point, attackRot);
		//			GameObject.Destroy(hitEffObj, 3f);

		//			CSoundMan.Instance.Play("SE_Punch00");

		//			CDamageTextManager.Instance.DispDamage(damage, hit.point);
		//		}
		//	}
		//}
		yield return null;
	}

	IEnumerator RangeAttackCoroutine()
	{
		owner.StartCoroutine(StartActionCoroutine());

		if ( skill.castTime > 0f )
		{
			yield return new WaitForSeconds(skill.castTime);
		}

		var shootDirRot = Quaternion.FromToRotation(Vector2.down, owner.Direction.normalized);

		for(int i = 0; i < skill.shootCountMax; ++i )
		{
			var offset = Vector2.zero;

			if ( skill.projectileAppearOffsets.Length > 0 )
			{
				var offsetIndex = i % skill.projectileAppearOffsets.Length;
				offset = skill.projectileAppearOffsets[offsetIndex];
				offset += new Vector2(
					Random.Range(-skill.randomizeRadius, skill.randomizeRadius),
					Random.Range(-skill.randomizeRadius, skill.randomizeRadius));
				offset = shootDirRot * offset;
			}

			var popPos = (Vector2)owner.transform.position + offset;
			var projectileObj = GameObject.Instantiate(skill.projectile, popPos, Quaternion.identity);

			var projectile = projectileObj.GetComponent<CProjectile>();

			projectileObj.transform.rotation = shootDirRot;

			projectile.GetComponentInChildren<CProjectile>().SetUp(owner, skill);
			projectile.moveDirection = owner.Direction.normalized;

			if ( i < skill.shootCountMax )
				yield return new WaitForSeconds(skill.shootInterval);
		}


		//CSoundMan.Instance.Play("SE_Magic00");

		while ( isInStartAction )
		{
			yield return null;
		}

		// 攻撃終了
	}

	IEnumerator StartActionCoroutine()
	{
		isInStartAction = true;

		switch ( skill.startActionType )
		{
			case SkillStartActionType.StepForward:
				var stepForwardCurve = owner.stepForwardCurve;
				var duration = stepForwardCurve.keys[stepForwardCurve.length-1].time;
				for(float t = 0f; t < duration; t += Time.deltaTime )
				{
					var distance = stepForwardCurve.Evaluate(t);
					var pos = owner.Direction.normalized * distance;
					owner.baseSpriteTransform.localPosition = pos;
					yield return null;
				}
				owner.baseSpriteTransform.localPosition = Vector3.zero;
				break;
			case SkillStartActionType.MagicCircle01:
				var eff = CEffectMan.Instance.PlayOneEffect("ef_magic_circle01", owner.transform.position, owner.transform.rotation, owner.transform, 0f);
				yield return new WaitForSeconds(skill.castTime);
				GameObject.Destroy(eff);
				break;
		}

		isInStartAction = false;
	}

	CBody[] GetTargets()
	{
		var hits = Physics2D.CircleCastAll(owner.transform.position, 15f, Vector2.up, 0f);

		if ( hits == null )
			return null;

		var targets = new CBody[hits.Length];

		for ( int i = 0; i < hits.Length; ++i)
		{
			targets[i] = hits[i].transform.GetComponentInChildren<CBody>();
		}

		return targets;
	}
}
