using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// 1個のスキルを発動する。ゲーム中に付け替え可能。
/// </summary>
public class AttackModule
{
	private Chara owner;
	private Skill skill;

	private bool isInStartAction = false;

	public AttackModule(Chara owner)
	{
		this.owner = owner;
	}

	public void SetSkill(int skillId)
	{
		skill = SkillLoader.Instance.Load(skillId);
	}

	/// <summary>
	/// 攻撃
	/// </summary>
	public IEnumerator AttackCoroutine()
	{
		if (!owner.StatusModule.CanConsumeMP(skill.costMP))
		{
			UIInstantMessage.Instance.RequestMessage($"MPが足りません。");
			yield break;
		}
		owner.StatusModule.ConsumeMP(skill.costMP);

		owner.SetAttackBindMove(true);
		owner.StartCoroutine(StartActionCoroutine());

		// キャストタイム
		if ( skill.castTime > 0f )
		{
			yield return new WaitForSeconds(skill.castTime);
		}

		owner.SetAttackBindMove(false);
		if(false == string.IsNullOrWhiteSpace(skill.burstSound))
		{
			SoundManager.Instance.Play(skill.burstSound, false, owner.transform.position, true);
		}

		var shootAngle = Vector2.SignedAngle(Vector2.down, owner.DirectionRightAngle.normalized);
		var shootDirRot = Quaternion.AngleAxis(shootAngle, Vector3.forward);

		for (int i = 0; i < skill.shootCountMax; ++i )
		{
			var offsetPos = Vector2.zero;
			var offsetRot = Quaternion.identity;

			if ( skill.projectileAppearOffsetPosList.Length > 0 )
			{
				var offsetIndex = i % skill.projectileAppearOffsetPosList.Length;
				offsetPos = skill.projectileAppearOffsetPosList[offsetIndex];
				offsetPos += MathExt.GetRandomPointInCircle(skill.posRandomizeRadius);
				offsetPos = shootDirRot * offsetPos;
			}
			if (skill.projectileAppearOffsetRotList.Length > 0)
			{
				var offsetIndex = i % skill.projectileAppearOffsetRotList.Length;
				offsetRot = Quaternion.Euler(0f, 0f, skill.projectileAppearOffsetRotList[offsetIndex]);
			}

			var popPos = (Vector2)owner.transform.position + offsetPos;
			var projectileObj = GameObject.Instantiate(skill.projectile, popPos, Quaternion.identity);

			var projectile = projectileObj.GetComponent<Projectile>();

			projectileObj.transform.rotation = offsetRot * shootDirRot;

			projectile.GetComponentInChildren<Projectile>().SetUp(owner, skill);

			if ( i < skill.shootCountMax && skill.shootInterval > 0)
				yield return new WaitForSeconds(skill.shootInterval);
		}

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
				var stepForwardCurve = owner.StepForwardCurve;
				var duration = stepForwardCurve.keys[stepForwardCurve.length-1].time;
				for(float t = 0f; t < duration; t += IngameTime.DeltaTime )
				{
					var distance = stepForwardCurve.Evaluate(t);
					var pos = owner.Direction.normalized * distance;
					owner.baseSpriteTransform.localPosition = pos;
					yield return null;
				}
				owner.baseSpriteTransform.localPosition = Vector3.zero;
				break;
			case SkillStartActionType.MagicCircle01:
				var eff = EffectMan.Instance.PlayOneEffect("ef_magic_circle01", owner.transform.position, owner.transform.rotation, owner.transform, 0f);
				yield return new WaitForSeconds(skill.castTime);
				GameObject.Destroy(eff);
				break;
		}

		isInStartAction = false;
	}

	Chara[] GetTargets()
	{
		var hits = Physics2D.CircleCastAll(owner.transform.position, 15f, Vector2.up, 0f);

		if ( hits == null )
			return null;

		var targets = new Chara[hits.Length];

		for ( int i = 0; i < hits.Length; ++i)
		{
			targets[i] = hits[i].transform.GetComponentInChildren<Chara>();
		}

		return targets;
	}
}
