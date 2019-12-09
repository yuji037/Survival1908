using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CActor : MonoBehaviour
{
	protected Rigidbody2D rigidbdy2D;

	protected Animator animator;

	protected Vector2 direction;

	public Vector2 Direction
	{
		get
		{
			if ( direction == Vector2.zero )
				direction = new Vector2(0f, -0.01f);
			return direction;
		}
	}

	public Vector2 DirectionRightAngle
	{
		get
		{
			var dir = Direction;
			if(dir.x != 0f && dir.y != 0f )
			{
				if ( Mathf.Abs(dir.x) >= Mathf.Abs(dir.y) )
					dir.y = 0f;
				else
					dir.x = 0f;
			}
			return dir;
		}
	}
	

	protected virtual void Awake()
	{
		rigidbdy2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

	// Update is called once per frame
	protected virtual void Update()
    {
        
    }
}
