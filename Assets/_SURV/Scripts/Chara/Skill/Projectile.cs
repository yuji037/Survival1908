using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾1発分の挙動
/// </summary>
public class Projectile : MonoBehaviour
{
	[SerializeField] private ProjectileType projectileType		= ProjectileType.Collider;
	[SerializeField] private float			animatorSpeed		= 1f;
	[SerializeField] private float			shootRange			= 5f;
	[SerializeField] private float			remainTime			= 5f;
	[SerializeField] private string			hitEffect			= default;
	[SerializeField] private float			coliderCastDelay	= 0f;
	[SerializeField] private float			coliderCastInterval = 1f;
	[SerializeField] private int			coliderCastCountMax = 1;
	[SerializeField] private bool			pierceFlag			= false;
	[SerializeField] private CameraShake.StartDirectionType camShakeStartDirectionType = CameraShake.StartDirectionType.DOWN;
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

	// Start is called before the first frame update
	void Start()
    {
		prevPos = transform.position;

		animator = GetComponent<Animator>();
		if (animator)
		{
			animator.speed = animatorSpeed;
		}
		colider2D = GetComponentInChildren<Collider2D>();

		nextColliderCastTime = coliderCastDelay;
	}

    // Update is called once per frame
    void Update()
    {
		if (ended) { return; }

		// 厳密ではない
		flyingDistance += (colider2D.transform.position - prevPos ).magnitude;
		prevPos = colider2D.transform.position;

		elapsedTime += IngameTime.DeltaTime;

		if ( flyingDistance >= shootRange
			|| elapsedTime >= remainTime)
		{
			End();
		}

		if(projectileType == ProjectileType.RangeCast )
		{
			if ( elapsedTime >= nextColliderCastTime )
			{
				nextColliderCastTime += coliderCastInterval;

				if(colliderCastCount < coliderCastCountMax )
				{
					colliderCastCount++;
					CastOverlapDamage();
				}
			}
		}

		if( false == calledCamShake &&
			elapsedTime >= camShakeDelay )
		{
			calledCamShake = true;
			CameraShake.Instance.Shake(camShakeStartDirectionType, camShakeStrength);
		}
    }

	private void End()
	{
		colider2D.enabled = false;

		// NOTE:これで子が止まらない場合は全部止める
		GetComponentInChildren<ParticleSystem>()?.Stop();

		foreach (var sp in GetComponentsInChildren<SpriteRenderer>())
		{
			sp.enabled = false;
		}
		Destroy(gameObject, 2f);
		ended = true;
	}

	public void SetUp(Chara attacker, Skill skill)
	{
		this.attacker = attacker;
		this.skill = skill;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("あたった");
		if ( projectileType != ProjectileType.Collider )
			return;

		OneDamage(collision, collision.ClosestPoint(transform.position));
	}

	private void CastOverlapDamage()
	{
		RaycastHit2D[] hits = new RaycastHit2D[10];

		colider2D.Cast(Vector2.up, hits, 0f, true);
		
		foreach(var hit in hits )
		{
			if ( hit.transform != null )
				OneDamage(hit.collider, hit.point);
		}
	}

	private void OneDamage(Collider2D collider, Vector2 hitPosition)
	{
		var body = collider.transform.GetComponentInChildren<Chara>();
		if ( body == null )
			return;

		if ( false == attacker.IsEnemy(body) )
			return;

		if ( false == pierceFlag)
		{
			End();
		}

		var damage = skill.effectValue1;

		attacker.SendDamage(body, new AttackInfo(
			attacker,
			skill.effectValue1,
			hitPosition,
			skill.knockbackPower
		));

		EffectMan.Instance.PlayOneEffect(hitEffect, hitPosition, Quaternion.identity);
	}

	private enum ProjectileType
	{
		Collider,
		RangeCast,
	}

}
