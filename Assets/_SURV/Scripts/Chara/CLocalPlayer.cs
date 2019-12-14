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

	[SerializeField]
	private bool isSwingingObject = false;

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
		UpdateSwing();
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

		if ( isSwingingObject )
		{
			var moveVelocity = movePad * swingingMoveSpeed;
			moveVelocity += (Vector2) ( swingMassCenter.position - transform.position ).normalized * Mathf.Abs(rotVel) * swingingMoveSpeedFactor;

			rigidbdy2D.drag = swingingLinearDrag;
			rigidbdy2D.velocity = moveVelocity;
		}
		else
		{
			var moveVelocity = movePad * moveSpeed;

			rigidbdy2D.drag = walkLinearDrag;
			rigidbdy2D.velocity = moveVelocity;
		}
	}

	[SerializeField]
	private float swingingMoveSpeed = 3f;
	[SerializeField]
	private float swingingLinearDrag = 3f;
	[SerializeField]
	private float walkLinearDrag = 8f;

	[SerializeField]
	private float swingingMoveSpeedFactor = 1.0f;

	[SerializeField]
	private Transform swingAnchor;

	[SerializeField]
	private Transform swingMassCenter;

	private float rotVel = 0f;

	[SerializeField]
	private float rotForceRate = 1.0f;

	[SerializeField]
	private float swingForceRate = 5f;

	[SerializeField]
	private float rotVelMax = 20f;

	[SerializeField]
	private float dampFactor = 1f;

	void UpdateSwing()
	{
		if ( false == isSwingingObject )
			return;

		var vec = swingAnchor.position - swingMassCenter.position;
		var rotForce = Vector3.Dot(input, vec) * rotForceRate;
		var swingForce = Vector3.Cross(input, vec).z * swingForceRate;

		if(rotVel < 0f )
		{
			rotForce = -rotForce;
		}

		rotVel += rotForce * Time.deltaTime + swingForce * Time.deltaTime;
		var dampF = dampFactor;
		if ( input.sqrMagnitude < 0.1f )
			dampF = 20f;
		rotVel *=  1f - dampF * Time.deltaTime;
		rotVel = Mathf.Clamp(rotVel, -rotVelMax, rotVelMax);

		var beforeRotEulerZ = swingAnchor.rotation.eulerAngles.z;

		swingAnchor.rotation *= Quaternion.Euler(0, 0, rotVel * Time.deltaTime);

		var afterRotEulerZ = swingAnchor.rotation.eulerAngles.z;

		if(	(	beforeRotEulerZ <= 330f && afterRotEulerZ > 330f && rotVel > 0f ) ||
			(	beforeRotEulerZ > 330f && afterRotEulerZ <= 330f && rotVel < 0f	)      )
		{
			CSoundMan.Instance.Play("SE_Swing00", false, null, false, Mathf.Abs(rotVel) / rotVelMax);
		}
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

		if ( isSwingingObject )
		{
			speed = 0.4f;
			direction = (swingMassCenter.position - transform.position).normalized;
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
