using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLocalPlayer : CBody
{
	public static CLocalPlayer Instance { get; private set; }

	[SerializeField]
	private float moveSpeed = 1.0f;

	[SerializeField]
	private GameObject magicPrefab;
	[SerializeField]
	private float magicForceRate = 1.0f;

	protected override void Awake()
	{
		base.Awake();

		Instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
	}

	// Update is called once per frame
	void Update()
    {
		UpdateMove();
		UpdateAction();
    }

	public void UpdateMove()
	{
		var hori = Input.GetAxis("Horizontal");
		var vert = Input.GetAxis("Vertical");
		var input = new Vector2(hori, vert);
		var force = input * moveSpeed;

		rigidbdy2D.AddForce(force, ForceMode2D.Impulse);

		animator.SetFloat("Speed", force.sqrMagnitude);

		if(hori != 0f || vert != 0f )
		{
			Direction = new Vector2(hori, vert);

			animator.SetFloat("Horizontal", Direction.x);
			animator.SetFloat("Vertical", Direction.y);
		}
	}

	public void UpdateAction()
	{
		if ( Input.GetButtonDown("Fire1") )
		{
			var popPos = transform.position;
			var directionNormalized = Direction.normalized;
			var deltaPos = directionNormalized;
			popPos.x += deltaPos.x;
			popPos.y += deltaPos.y;
			var magicObj = Instantiate(magicPrefab, popPos, Quaternion.identity);
			magicObj.GetComponent<Rigidbody2D>().AddForce(directionNormalized * magicForceRate, ForceMode2D.Impulse);

			magicObj.GetComponentInChildren<CProjectile>().SetUp(this);
		}
	}
}
