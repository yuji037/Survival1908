using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chara : MonoBehaviour
{
	//=============================================================
	// フィールド
	//=============================================================
	[SerializeField] private	Team			teamType			= default;
	[SerializeField] protected	int[]			skillIDs			= default;
	[SerializeField] private	AnimationCurve	stepForwardCurve	= default;
	[SerializeField] private	float			winceTimeMax		= 1f;


	private		int				id			= -1;
	protected	Rigidbody2D		rigidbdy2D;
	protected	Animator		animator;
	protected	Vector2			direction;
	protected	bool			isRemove	= false;
	
	public		Transform				baseSpriteTransform;
	protected	AttackModule[]			attackModules;
	protected	bool					isAttacking = false;
	protected	bool					isAttackBindMove = false;

	private float winceTime = 0f;
	private ModuleController moduleController = new ModuleController();
	protected StatusModule statusModule;
	private EfficacyModule efficacyModule;

	//=============================================================
	// プロパティ
	//=============================================================
	public Vector2 Direction
	{
		get
		{
			if (direction == Vector2.zero)
				direction = new Vector2(0f, -0.01f);
			return direction;
		}
	}
	public Vector2 DirectionRightAngle
	{
		get
		{
			var dir = Direction;
			if (dir.x != 0f && dir.y != 0f)
			{
				if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
					dir.y = 0f;
				else
					dir.x = 0f;
			}
			return dir;
		}
	}
	public int		ID { get { return id; } }
	public bool		IsRemove { get { return isRemove; } }
	public Team		TeamType { get { return teamType; } }
	public bool		IsWincing { get; private set; }
	public AnimationCurve StepForwardCurve { get { return stepForwardCurve; } }
	public StatusModule StatusModule { get { return statusModule; } }
	public EfficacyModule EfficacyModule { get { return efficacyModule; } }
	protected virtual CustomStatusRate CustomStatusRate { get => null; }

	//=============================================================
	// メソッド
	//=============================================================
	public virtual void Awake()
	{
		rigidbdy2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		statusModule = GetComponent<StatusModule>();
		baseSpriteTransform = transform.Find("BaseSprite");
		efficacyModule = new EfficacyModule();
	}

	public virtual void Begin()
	{
		statusModule.Enable(this, CustomStatusRate, efficacyModule);
		SetSkills();
	}

	public virtual void Tick()
	{
		efficacyModule.Tick();
		statusModule.Tick();

		if ( IsWincing )
		{
			winceTime += Time.deltaTime;
			if ( winceTime >= winceTimeMax )
			{
				IsWincing = false;
				winceTime = 0f;
			}
		}
	}

	public virtual void FixedTick(){ }

	public void InitID(int id)
	{
		this.id = id;
	}

	public void SetLevel(int level)
	{
		statusModule.SetLevel(level);
	}

	protected void SetSkills()
	{
		attackModules = new AttackModule[skillIDs.Length];
		for (int i = 0; i < attackModules.Length; ++i)
		{
			attackModules[i] = new AttackModule(this);
			attackModules[i].SetSkill(skillIDs[i]);
		}
	}

	public virtual void ReceiveDamage(AttackInfo attackInfo)
	{
		statusModule.ReceiveDamage(attackInfo);

		var attacker = attackInfo.attacker;
		if (attacker != null) // 攻撃者がもう死亡している可能性もある
		{
			Knockback(transform.position - attacker.transform.position, attackInfo.knockbackPower);
		}

		if (statusModule.HP <= 0 )
		{
			GiveDefeatReward(attacker);
			OnDead();
		}
	}

	public bool IsEnemy(Chara other)
	{
		return teamType != other.teamType;
	}

	public void Attack(int attackModuleIndex)
	{
		StartCoroutine(AttackCoroutine(attackModuleIndex));
	}

	private IEnumerator AttackCoroutine(int attackModuleIndex)
	{
		if (isAttacking)
			yield break;

		isAttacking = true;
		yield return StartCoroutine(attackModules[attackModuleIndex].AttackCoroutine());
		isAttacking = false;
	}

	public virtual bool UseMP(float useMP)
	{
		if (useMP <= 0)
		{
			return true;
		}
		return false;
	}

	public void SendDamage(Chara target, AttackInfo attackInfo)
	{
		if (false == IsEnemy(target))
			return;

		target.ReceiveDamage(attackInfo);
	}

	public void SetAttackBindMove(bool value)
	{
		isAttackBindMove = value;
	}

	/// <param name="direction">正規化必要なし</param>
	public void Knockback(Vector3 direction, float knockbackPower)
	{
		if ( direction == Vector3.zero ) direction = new Vector3(0f, 0f, -1f);
		SetForce(direction.normalized * knockbackPower);
	}

	public void SetForce(Vector2 velocity)
	{
		IsWincing = true;
		baseSpriteTransform.localPosition = Vector3.zero;

		var nowVelocity = rigidbdy2D.velocity;
		var needForce = velocity - nowVelocity;
		rigidbdy2D.AddForce(needForce, ForceMode2D.Impulse);
	}

	public void FadeAnimatorState(string stateName)
	{
		animator.CrossFade(stateName, 0.1f);
	}
	
	protected virtual void GiveDefeatReward(Chara attacker)
	{

	}

	protected virtual void OnDead()
	{
		DestroyObject();
	}

	public void DestroyObject()
	{
		Destroy(gameObject);
	}

	private void OnDestroy()
	{
		CharasController.Instance.UnregisterChara(this);
	}
}