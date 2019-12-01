using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CProjectile : MonoBehaviour
{
	CCaster attacker;

	[SerializeField]
	private GameObject hitEffect;

	[SerializeField]
	private float knockbackPower = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetUp(CCaster _attacker)
	{
		attacker = _attacker;

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("あたった");

		var cBody = collision.gameObject.GetComponentInChildren<CBody>();
		if ( cBody == null )
			return;

		if ( false == CCaster.IsOppositeTeam(attacker, cBody) )
			return;

		Destroy(gameObject);

		cBody.ReceiveDamage();

		var distance = cBody.transform.position - transform.position;
		if ( distance == Vector3.zero ) distance = new Vector3(0f, 0f, -1f);
		cBody.SetVelocity(distance.normalized * knockbackPower);
		

		var hitEff = Instantiate(hitEffect, transform.position, Quaternion.identity);
		Destroy(hitEff, 3f);

		CSoundMan.Instance.Play("SE_Hit00");
	}
}
