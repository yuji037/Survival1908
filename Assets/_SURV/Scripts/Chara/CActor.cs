using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CActor : MonoBehaviour
{
	protected Rigidbody2D rigidbdy2D;

	protected Animator animator;

	private Vector2 direction;

	protected Vector2 Direction
	{
		get
		{
			if ( direction == Vector2.zero )
				direction = new Vector2(0f, -0.01f);
			return direction;
		}
		set
		{
			direction = value;
		}
	}

	protected virtual void Awake()
	{
		rigidbdy2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
