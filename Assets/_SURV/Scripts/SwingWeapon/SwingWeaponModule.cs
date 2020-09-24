using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingWeaponModule
{
	PartyChara owner;

	SwingWeapon swingWeapon;

	public SwingWeaponModule(PartyChara owner)
	{
		this.owner = owner;
	}

    public void Update(Vector2 input)
    {
		if ( IsGrabbingSwingWeapon() )
		{
			swingWeapon.UpdateSwing(input);
		}
	}

	public bool IsGrabbingSwingWeapon() { return swingWeapon != null; }

	public Vector2 GetDirectionToWeapon()
	{
		return ( swingWeapon.MassCenter.position - owner.transform.position ).normalized;
	}

	public bool TryGrabSwingWeapon(Transform locatorSwingWeapon)
	{
		var hits = Physics2D.CircleCastAll(
							owner.transform.position + (Vector3)owner.Direction.normalized,
							0.5f,
							Vector3.up,
							0f);

		foreach ( var hit in hits )
		{
			if ( hit.collider.name == "WeaponBody" )
			{
				swingWeapon = hit.transform.GetComponentInParent<SwingWeapon>();
				swingWeapon.Grab(owner.transform);
				swingWeapon.transform.SetParent(locatorSwingWeapon, true);
				swingWeapon.transform.localPosition = Vector3.zero;

				owner.ChangeMass(+swingWeapon.Mass);
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// 向心力
	/// </summary>
	public Vector2 GetCentrifugalVelocity()
	{
		var vel =
			(Vector2)( swingWeapon.MassCenter.position - owner.transform.position ).normalized *
			Mathf.Abs( swingWeapon.RotVel ) *
			0.005f;

		return vel;
	}

	public void ReleaseSwingWeapon()
	{
		swingWeapon.transform.SetParent(null);
		swingWeapon.Releace();
		owner.ChangeMass(-swingWeapon.Mass);

		swingWeapon = null;
	}
}
