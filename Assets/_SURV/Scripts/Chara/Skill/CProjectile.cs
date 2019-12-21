using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CProjectile : MonoBehaviour
{
	[SerializeField]
	private ProjectileType projectileType = ProjectileType.Collider;

	CCaster attacker;

	CSkill skill;

	[SerializeField]
	private float		moveSpeed		= 10f;

	[SerializeField]
	private float		shootRange		= 5f;
	
	[SerializeField]
	private float		remainTime		= 5f;

	[SerializeField]
	private string		hitEffect;

	[SerializeField]
	private float		coliderCastDelay = 0f;
	
	[SerializeField]
	private float		coliderCastInterval = 1f;

	[SerializeField]
	private int			coliderCastCountMax = 1;

	[SerializeField]
	private bool		pierceFlag		= false;

	[SerializeField]
	private CCameraShake.StartDirectionType camShakeStartDirectionType = CCameraShake.StartDirectionType.DOWN;

	[SerializeField]
	private float		camShakeDelay = 0f;
	
	[SerializeField]
	private float		camShakeStrength = 0.1f;

	public Vector2 moveDirection;
	public CBody target;

	private float flyingDistance = 0f;
	private Vector3 prevPos;
	private float elapsedTime = 0f;
	private float nextColliderCastTime;
	private int colliderCastCount;

	private Collider2D collider2D;

	private bool calledCamShake = false;

	// Start is called before the first frame update
	void Start()
    {
		prevPos = transform.position;

		if ( moveSpeed > 0f )
			GetComponent<Rigidbody2D>().AddForce(moveDirection * moveSpeed, ForceMode2D.Impulse);

		collider2D = GetComponentInChildren<Collider2D>();

		nextColliderCastTime = coliderCastDelay;
	}

    // Update is called once per frame
    void Update()
    {
		flyingDistance += ( transform.position - prevPos ).magnitude;
		prevPos = transform.position;

		elapsedTime += Time.deltaTime;

		if ( flyingDistance >= shootRange
			|| elapsedTime >= remainTime	)
			Destroy(gameObject);

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
			CCameraShake.Instance.Shake(camShakeStartDirectionType, camShakeStrength);
		}
    }

	public void SetUp(CCaster _attacker, CSkill _skill)
	{
		attacker = _attacker;
		skill = _skill;
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

		collider2D.Cast(Vector2.up, hits, 0f, true);
		
		foreach(var hit in hits )
		{
			if ( hit.transform != null )
				OneDamage(hit.collider, hit.point);
		}
	}

	private void OneDamage(Collider2D collider, Vector2 hitPosition)
	{
		var cBody = collider.transform.GetComponentInChildren<CBody>();
		if ( cBody == null )
			return;

		if ( false == attacker.IsEnemy(cBody) )
			return;

		if ( false == pierceFlag )
			Destroy(gameObject);

		var damage = skill.attackPower;

		attacker.SendDamage(cBody, new CAttackInfo(
			attacker,
			skill.attackPower,
			hitPosition,
			skill.knockbackPower
		));

		CEffectMan.Instance.PlayOneEffect(hitEffect, hitPosition, Quaternion.identity);
	}

	private enum ProjectileType
	{
		Collider,
		RangeCast,
	}

}
