using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBody : CCaster
{
	[SerializeField]
	private int hp = 20;

	[SerializeField]
	private float winceTimeMax = 1f;

	public bool isWincing { get; private set; }
	private float winceTime = 0f;

	protected override void Update()
	{
		base.Update();

		if ( isWincing )
		{
			winceTime += Time.deltaTime;
			if ( winceTime >= winceTimeMax )
			{
				isWincing = false;
				winceTime = 0f;
			}
		}
	}

	public virtual void ReceiveDamage(CAttackInfo attackInfo)
	{
		var attacker = attackInfo.attacker;
		var damage = attackInfo.attackPower;
		var intDamage = Mathf.FloorToInt(damage);

		hp -= intDamage;

		var distance = transform.position - attacker.transform.position;
		if ( distance == Vector3.zero ) distance = new Vector3(0f, 0f, -1f);
		SetForce(distance.normalized * attackInfo.knockbackPower);

		CDamageTextManager.Instance.DispDamage(intDamage, attackInfo.hitPosition);

		if (hp <= 0 )
		{
			Destroy(gameObject);
		}
	}

	public void SetForce(Vector2 velocity)
	{
		isWincing = true;
		baseSpriteTransform.localPosition = Vector3.zero;

		var nowVelocity = rigidbdy2D.velocity;
		var needForce = velocity - nowVelocity;
		rigidbdy2D.AddForce(needForce, ForceMode2D.Impulse);
	}
}
