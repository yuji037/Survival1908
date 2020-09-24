using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

[RequireComponent(typeof(NpcStatusModule))]
public class Npc : Chara, ISpawnable
{
	[SerializeField] private float moveSpeedRate = 10f;
	[SerializeField] public AnimationCurve moveForwardSpeedCurve = default;
	[SerializeField] public AnimationCurve moveForwardJumpCurve = default;
	[SerializeField] private float bodyKnockbackPower = 10f;
	[SerializeField] private float bodyKnockbackReturnPower = 10f;
	[SerializeField] private CustomStatusRate customStatusRate = default;

	private Chara target;
	public string charaId;
	public string charaName;
	public Spawner spawner = null;
	private Behavior behavior;

	public Chara Target { get { return target; } }
	protected override CustomStatusRate CustomStatusRate { get => customStatusRate; }

	public override void Awake()
	{
		base.Awake();
		behavior = GetComponent<Behavior>();
	}

	public override void Begin()
    {
		base.Begin();
	}

	public void SetSpawner(Spawner spawner)
	{
		this.spawner = spawner;
	}

	public void SetTarget()
	{
		target = LocalPlayer.Instance;
	}

	public override void Tick()
    {
		base.Tick();
		IngameCoordinator.Instance.BehaviorManager.Tick(behavior);

		animator.SetFloat("Horizontal", direction.x);
		animator.SetFloat("Vertical", direction.y);
	}

	public void SetDirection(Vector2 _direction)
	{
		direction = _direction;
	}

	public void LookAtTarget(float rotateSpeed = float.PositiveInfinity)
	{
		Vector2 targetDirection;

		var disToTarget = target.transform.position - transform.position;
		if ( disToTarget == Vector3.zero )
			targetDirection = Vector2.down;
		else
			targetDirection = disToTarget.normalized;

		if( rotateSpeed >= 1000f )
		{
			direction = targetDirection;
		}
		else
		{
			var beforeRot = Quaternion.LookRotation(Direction, Vector3.back);
			var targetRot = Quaternion.LookRotation(targetDirection, Vector3.back);

			var afterRot = Quaternion.RotateTowards(beforeRot, targetRot, rotateSpeed * IngameTime.DeltaTime);

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
		var body = collision.gameObject.GetComponent<Chara>();
		if ( body == null )
			return;

		// 敵に当たるとダメージ
		if ( false == IsEnemy(body) )
			return;

		SendDamage(body, new AttackInfo(
			this,
			1f,
			collision.collider.ClosestPoint(transform.position),
			bodyKnockbackPower
			));

		// 自分もノックバック
		Knockback(transform.position - body.transform.position, bodyKnockbackReturnPower);
	}

	public override void ReceiveDamage(AttackInfo attackInfo)
	{
		base.ReceiveDamage(attackInfo);
		LookAtTarget();
	}

	protected override void GiveDefeatReward(Chara attacker)
	{
		base.GiveDefeatReward(attacker);

		var itemDropList = CharaItemDropDataMan.Instance.GetDropItemsByCharaName(charaName);
		foreach(var itemDrop in itemDropList )
		{
			ItemInventry.Instance.AddChangeItemCount(itemDrop.itemID, itemDrop.count);
		}

		if ( attacker is PartyChara partyChara )
		{
			partyChara.StatusModule.ChangeCombatExp((statusModule as NpcStatusModule).GainExp);
		}
	}

	protected override void OnDead()
	{
		base.OnDead();

		if ( spawner )
		{
			spawner.UnregisterDeadObject(this.gameObject);
		}
	}
}
