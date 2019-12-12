using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLocalPlayer : CBody
{
	public static CLocalPlayer Instance { get; private set; }

	[SerializeField]
	private float moveSpeed = 1.0f;

	private CAttackModule[] attackModules;

	private Vector2 input;
	private Vector2 movePad;
	private Vector2 inputTouchMove;

	protected override void Awake()
	{
		base.Awake();
		Instance = this;
	}

	// Start is called before the first frame update
	protected override void Start()
    {
		base.Start();

		attackModules = new CAttackModule[2];
		for(int i = 0; i < attackModules.Length; ++i )
		{
			attackModules[i] = new CAttackModule();
			attackModules[i].Init(this);
		}
		attackModules[0].SetSkill(10001);
		attackModules[1].SetSkill(20002);
	}

	// Update is called once per frame
	protected override void Update()
    {
		base.Update();

		UpdateInput();
		UpdateAnim();
		UpdateAction();
    }

	void FixedUpdate()
	{
		if ( isAttacking )
		{
			rigidbdy2D.velocity = Vector2.zero;
			return;
		}

		if ( isWincing )
			return;

		var moveVelocity = movePad * moveSpeed;

		rigidbdy2D.velocity = moveVelocity;
	}

	void UpdateInput()
	{
		var hori = Input.GetAxis("Horizontal");
		var vert = Input.GetAxis("Vertical");
		input = new Vector2(hori, vert);
		if ( input.sqrMagnitude > 1f )
			input = input.normalized;

		if( input.sqrMagnitude < inputTouchMove.sqrMagnitude )
		{
			input = inputTouchMove;
			inputTouchMove = Vector2.zero;
		}

		if ( isAttacking )
			movePad = Vector2.zero;
		else
			movePad = input;
	}

	public void UpdateAnim()
	{
		//animator.SetFloat("Speed", movePad.sqrMagnitude);

		var speed = Mathf.Max(movePad.sqrMagnitude, 0.1f);

		if( movePad.x != 0f || movePad.y != 0f )
		{
			direction = movePad;
		}
		animator.SetFloat("Horizontal", Direction.x * speed);
		animator.SetFloat("Vertical", Direction.y * speed);
	}

	public void UpdateAction()
	{
		//if ( Input.GetButtonDown("Fire1") )
		//{
		//	StartCoroutine(AttackCoroutine(0));
		//}

		//if ( Input.GetButtonDown("Fire2") )
		//{
		//	StartCoroutine(AttackCoroutine(1));
		//}
	}

	public void MeleeAttack()
	{
		StartCoroutine(AttackCoroutine(0));
	}

	public void RangeAttack()
	{
		StartCoroutine(AttackCoroutine(1));
	}

	private IEnumerator AttackCoroutine(int attackModuleIndex)
	{
		if ( isAttacking )
			yield break;

		isAttacking = true;
		yield return StartCoroutine(attackModules[attackModuleIndex].AttackCoroutine());
		isAttacking = false;
	}

	public override void ReceiveDamage(CAttackInfo attackInfo)
	{
		base.ReceiveDamage(attackInfo);

		CSoundMan.Instance.Play("SE_Hit02");
	}

	public void InputTouchMovement(Vector2 vec)
	{
		if ( vec.sqrMagnitude > 1f )
			vec = vec.normalized;

		inputTouchMove = vec;
	}
}
