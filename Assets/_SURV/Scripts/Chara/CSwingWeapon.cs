using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CSwingWeapon : MonoBehaviour
{

	[SerializeField]
	private float mass = 1f;
	public float Mass { get { return mass; } }

	[SerializeField]
	private Transform massCenter;
	public Transform MassCenter { get { return massCenter; } }

	// 重量に反比例
	[SerializeField, Tooltip("パッド入力方向に移動できる速度")]
	private float moveSpeedControllable = 1.6f;
	public float MoveSpeedControllable{ get { return moveSpeedControllable; }	}


	// 重量に比例
	[SerializeField, Tooltip("武器と反対方向に所有者が振られる速度係数（武器の回転速度に比例）")]
	private float moveSpeedUncontrollableFactor = 0.005f;
	public float MoveSpeedUncontrollableFactor { get { return moveSpeedUncontrollableFactor; } }

	// 重量に比例
	[SerializeField]
	private float swingingLinearDrag = 3f;

	private float rotVel = 0f;

	public float RotVel { get { return rotVel; } }

	// 重量に反比例
	[SerializeField, Tooltip("回転しやすさ（武器と反対方向成分）")]
	private float rotForceRate = 70f;

	[SerializeField, Tooltip("回転しやすさ（回転方向成分）")]
	private float swingForceRate = 40f;

	[SerializeField]
	private float rotVelMax = 600f;

	[SerializeField]
	private float rotDampFactor = 1f;

	[SerializeField]
	private float stoppingRotDampFactor = 20f;

	private Transform[] grabPoints;

	CCaster owner;

	[SerializeField]
	private float attackPower = 3f;

	[SerializeField]
	private float knockbackPowerRate = 4f;

	[SerializeField]
	private string hitEffect = "ef_hit_melee02";

	[SerializeField]
	private string hitSE = "SE_Hit02";

	// Start is called before the first frame update
	void Start()
    {
		var grabPointParent = transform.Find("WeaponBody/GrabPoints");
		grabPoints = grabPointParent.GetComponentsInChildren<Transform>().
			Where(tr => tr != grabPointParent).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void UpdateSwing(Vector2 input)
	{

		var vec = transform.position - massCenter.position;
		var rotForce = Vector3.Dot(input, vec) * rotForceRate;
		var swingForce = Vector3.Cross(input, vec).z * swingForceRate;

		if ( rotVel < 0f )
		{
			rotForce = -rotForce;
		}

		rotVel += rotForce * Time.deltaTime + swingForce * Time.deltaTime;
		var dampF = rotDampFactor;
		if ( input.sqrMagnitude < 0.1f )
			dampF = stoppingRotDampFactor;
		rotVel *= 1f - dampF * Time.deltaTime;
		rotVel = Mathf.Clamp(rotVel, -rotVelMax, rotVelMax);

		var beforeRotEulerZ = transform.rotation.eulerAngles.z;

		transform.rotation *= Quaternion.Euler(0, 0, rotVel * Time.deltaTime);

		var afterRotEulerZ = transform.rotation.eulerAngles.z;

		if ( ( beforeRotEulerZ <= 330f && afterRotEulerZ > 330f && rotVel > 0f ) ||
			( beforeRotEulerZ > 330f && afterRotEulerZ <= 330f && rotVel < 0f ) )
		{
			CSoundMan.Instance.Play("SE_Swing00", false, null, false, Mathf.Abs(rotVel) / 600f);
		}
	}

	public void Grab(CCaster _owner)
	{
		owner = _owner;

		var grabPoint = FindNearestGrabPoint(_owner.transform);

		var weaponBody = transform.Find("WeaponBody");

		var offset = -grabPoint.localPosition;

		weaponBody.localPosition = offset;

		GetComponentInChildren<Collider2D>().isTrigger = true;
	}

	public void Releace()
	{
		owner = null;

		var weaponBody = transform.Find("WeaponBody");

		transform.position = weaponBody.transform.position;
		weaponBody.localPosition = Vector3.zero;

		GetComponentInChildren<Collider2D>().isTrigger = false;
	}

	Transform FindNearestGrabPoint(Transform _owner)
	{
		Transform grabPoint = null;
		float minDistance = 999f;
		foreach ( var gp in grabPoints )
		{
			var distance = ( gp.transform.position - _owner.transform.position ).sqrMagnitude;
			if ( minDistance > distance )
			{
				minDistance = distance;
				grabPoint = gp;
			}
		}
		if ( grabPoint == null )
		{
			Debug.LogError("GrabPointが取得できません");
		}
		return grabPoint;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var body = collision.transform.GetComponentInChildren<CBody>();
		if ( body == null )
			return;

		if ( false == CLocalPlayer.Instance.IsEnemy(body) )
			return;

		var hitRadius = ( collision.transform.position - transform.position ).magnitude;

		// 現実はhitRadiusの2乗な気がする
		var damage = attackPower * hitRadius * (Mathf.Abs(rotVel) / rotVelMax);
		var knockbackPower = knockbackPowerRate * hitRadius;

		var hitPosition = collision.ClosestPoint(transform.position);

		CSoundMan.Instance.Play(hitSE);

		CEffectMan.Instance.PlayOneEffect(hitEffect, hitPosition, Quaternion.identity);

		CLocalPlayer.Instance.SendDamage(
			body,
			new CAttackInfo(CLocalPlayer.Instance, damage, hitPosition, knockbackPower));
	}
}
