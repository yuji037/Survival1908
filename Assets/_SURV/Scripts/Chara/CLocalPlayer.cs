using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLocalPlayer : CPartyChara
{
	public static CLocalPlayer Instance { get; private set; }

	[SerializeField]
	private float moveSpeed = 1.0f;

	private CAttackModule[] attackModules;

	private Vector2 input;
	private Vector2 movePad;
	private Vector2 inputTouchMove;

	private CSwingWeapon swingWeapon;

	private Transform locatorSwingWeapon;

	[SerializeField]
	private float walkLinearDrag = 8f;


	protected override void Awake()
	{
		base.Awake();
		Instance = this;

		locatorSwingWeapon = transform.Find("Locator_SwingWeapon");
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

		if ( swingWeapon != null )
		{
			//var moveVelocity = movePad * swingingMoveSpeed;
			var moveVelocity = movePad * 1.6f;
			//moveVelocity += (Vector2)( swingWeapon.MassCenter.position - transform.position ).normalized * Mathf.Abs(rotVel) * swingingMoveSpeedFactor;
			moveVelocity += (Vector2)( swingWeapon.MassCenter.position - transform.position ).normalized * 
				Mathf.Abs(swingWeapon.RotVel) * 
				0.005f;

			rigidbdy2D.velocity = moveVelocity;
		}
		else
		{
			var moveVelocity = movePad * moveSpeed;

			rigidbdy2D.drag = walkLinearDrag;
			rigidbdy2D.velocity = moveVelocity;
		}
	}

	void UpdateSwing()
	{
		if(swingWeapon != null )
		{
			swingWeapon.UpdateSwing(input);
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

		if ( swingWeapon != null )
		{
			speed = 0.4f;
			direction = (swingWeapon.MassCenter.position - transform.position).normalized;
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

	bool TryGrabSwingWeapon()
	{
		var hits = Physics2D.CircleCastAll(
							transform.position + (Vector3)Direction.normalized,
							0.5f,
							Vector3.up,
							0f);

		foreach(var hit in hits )
		{
			if(hit.collider.name == "WeaponBody" )
			{
				swingWeapon = hit.transform.GetComponentInParent<CSwingWeapon>();
				swingWeapon.Grab(this);
				swingWeapon.transform.SetParent(locatorSwingWeapon, true);
				swingWeapon.transform.localPosition = Vector3.zero;

				rigidbdy2D.mass += swingWeapon.Mass;
				return true;
			}
		}

		return false;
	}

	void ReleaseSwingWeapon()
	{
		swingWeapon.transform.SetParent(null);
		swingWeapon.Releace();
		rigidbdy2D.mass -= swingWeapon.Mass;

		swingWeapon = null;
	}

	public void MeleeAttack()
	{
		if ( swingWeapon == null && TryGrabSwingWeapon() )
		{
			return;
		}
		else if ( swingWeapon != null )
		{
			ReleaseSwingWeapon();
			return;
		}

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
