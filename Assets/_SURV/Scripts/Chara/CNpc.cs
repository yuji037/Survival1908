using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNpc : CBody
{
	private CActor target;

	[SerializeField]
	private float moveSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
		target = CLocalPlayer.Instance;
    }

    // Update is called once per frame
    void Update()
    {
		if ( target == null ) return;

		var distance = target.transform.position - transform.position;
		var force = distance.normalized * moveSpeed;
		rigidbdy2D.AddForce(force, ForceMode2D.Force);

		animator.SetFloat("Horizontal", force.x);
		animator.SetFloat("Vertical", force.y);
	}
}
