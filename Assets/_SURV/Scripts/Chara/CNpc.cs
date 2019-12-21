using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNpc : CBody
{
	public string charaId;
	public string charaName;

	public float gainCombatExp;

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
	[SerializeField]
	private float bodyKnockbackReturnPower = 10f;

	public CNpcSpawner spawner = null;

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

	public void LookAtTarget(float rotateSpeed = 99999f)
	{
		var targetDirection = Vector2.down;

		var disToTarget = target.transform.position - transform.position;
		if ( disToTarget == Vector3.zero )
			targetDirection = Vector2.down;
		else
			targetDirection = disToTarget.normalized;

		if(rotateSpeed >= 99999f )
		{
			direction = targetDirection;
		}
		else
		{
			var beforeRot = Quaternion.LookRotation(Direction, Vector3.back);
			var targetRot = Quaternion.LookRotation(targetDirection, Vector3.back);

			var afterRot = Quaternion.RotateTowards(beforeRot, targetRot, rotateSpeed * Time.deltaTime);

			direction = afterRot * Vector3.forward;
		}
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

		if ( false == IsEnemy(body) )
			return;

		SendDamage(body, new CAttackInfo(
			this,
			atkPower,
			collision.collider.ClosestPoint(transform.position),
			bodyKnockbackPower
			));

		// 攻撃者の方もノックバック
		Knockback(transform.position - body.transform.position, bodyKnockbackReturnPower);
	}

	protected override void GiveDefeatReward(CCaster attacker)
	{
		base.GiveDefeatReward(attacker);

		var itemDropList = CCharaItemDropDataMan.Instance.GetDropItemsByCharaName(charaName);
		foreach(var itemDrop in itemDropList )
		{
			CInventryMan.Instance.ManipulateItemCount(itemDrop.itemID, itemDrop.count);
		}

		if ( attacker is CPartyChara partyChara )
		{
			partyChara.GainCombatExp(gainCombatExp);
		}
	}

	protected override void OnDead()
	{
		base.OnDead();

		if ( spawner )
		{
			spawner.UnregisterNpc(this);
		}
	}
}
