using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾1発分の挙動
/// </summary>
public class Projectile : MonoBehaviour
{
	private const string EFFECT_HIT_MAP = "ef_hit_map01";

	[SerializeField] private ProjectileType projectileType		= ProjectileType.Collider;
	[SerializeField] private float			animatorSpeed		= 1f;
	[SerializeField] private float			shootRange			= 5f;
	[SerializeField] private float			remainTime			= 5f;
	[SerializeField] private string			hitEffect			= default;
	[SerializeField] private float			coliderCastDelay	= 0f;
	[SerializeField] private float			coliderCastInterval = 1f;
	[SerializeField] private int			coliderCastCountMax = 1;
	[SerializeField] private bool			pierceFlag			= false;
	[SerializeField] private CameraShakeManager.StartDirectionType camShakeStartDirectionType = CameraShakeManager.StartDirectionType.DOWN;
	[SerializeField] private float			camShakeDelay		= 0f;
	[SerializeField] private float			camShakeStrength	= 0.1f;

	private Chara attacker;
	private Skill skill;
	private float flyingDistance = 0f;
	private Vector3 prevPos;
	private float elapsedTime = 0f;
	private float nextColliderCastTime;
	private int colliderCastCount;
	private Animator animator;
	private Collider2D colider2D;
	private bool ended = false;
	private bool calledCamShake = false;

	public void Init(Chara attacker, Skill skill)
	{
		this.attacker = attacker;
		this.skill = skill;
	}

	// Start is called before the first frame update
	void Start()
    {
		animator = GetComponent<Animator>();
		if (animator)
		{
			animator.speed = animatorSpeed;
		}
		colider2D = GetComponentInChildren<Collider2D>();

		prevPos = transform.position;
		nextColliderCastTime = coliderCastDelay;
	}

    // Update is called once per frame
    void Update()
    {
		if (ended) { return; }

		CheckEnd();
		CheckRangeCast();
		CheckCameraShake();
    }

	private void CheckEnd()
	{
		// 厳密ではない
		var deltaDistance = (colider2D.transform.position - prevPos).magnitude;
		flyingDistance += deltaDistance;
		prevPos = colider2D.transform.position;

		elapsedTime += IngameTime.DeltaTime;

		if (flyingDistance >= shootRange
		|| elapsedTime >= remainTime)
		{
			End();
		}
	}

	private void CheckRangeCast()
	{
		if (projectileType == ProjectileType.RangeCast)
		{
			if (elapsedTime >= nextColliderCastTime)
			{
				nextColliderCastTime += coliderCastInterval;

				if (colliderCastCount < coliderCastCountMax)
				{
					colliderCastCount++;
					CastOverlapDamage();
				}
			}
		}
	}

	private void CheckCameraShake()
	{
		if (calledCamShake
		||  elapsedTime < camShakeDelay) { return; }

		calledCamShake = true;
		CameraShakeManager.Instance.Shake(camShakeStartDirectionType, camShakeStrength);
	}

	private void End()
	{
		colider2D.enabled = false;

		foreach(var par in GetComponentsInChildren<ParticleSystem>())
		{
			par.Stop();
		}

		foreach (var sp in GetComponentsInChildren<SpriteRenderer>())
		{
			sp.enabled = false;
		}
		Destroy(gameObject, 2f);
		ended = true;
	}


	private void OnTriggerEnter2D(Collider2D otherCollider)
	{
		//Debug.Log("あたった");
		if ( projectileType != ProjectileType.Collider )
			return;

		var hitPosition = otherCollider.ClosestPoint(colider2D.transform.position);

		CheckEnemy(otherCollider, hitPosition);
		CheckHitMap(otherCollider, hitPosition);
	}

	private void CastOverlapDamage()
	{
		RaycastHit2D[] hits = new RaycastHit2D[10];

		colider2D.Cast(Vector2.up, hits, 0f, true);
		
		foreach(var hit in hits )
		{
			if ( hit.transform != null )
				CheckEnemy(hit.collider, hit.point);
		}
	}

	private void CheckEnemy(Collider2D otherCollider, Vector2 hitPosition)
	{
		var body = otherCollider.transform.GetComponentInChildren<Chara>();
		if (body == null) { return; }
		if (false == attacker.IsEnemy(body)) { return; }

		if ( false == pierceFlag)
		{
			End();
		}

		attacker.SendDamage(body, new AttackInfo(
			attacker,
			skill.effectValue1,
			hitPosition,
			skill.knockbackPower
		));

		EffectMan.Instance.PlayOneEffect(hitEffect, hitPosition, Quaternion.identity);
	}

	private void CheckHitMap(Collider2D otherCollider, Vector2 hitPosition)
	{
		if (otherCollider.gameObject.layer != (int)ObjectLayer.Map) { return; }
		if (pierceFlag) { return; }

		End();
		EffectMan.Instance.PlayOneEffect(EFFECT_HIT_MAP, hitPosition, Quaternion.identity);
	}

	private enum ProjectileType
	{
		Collider,
		RangeCast,
	}

}
