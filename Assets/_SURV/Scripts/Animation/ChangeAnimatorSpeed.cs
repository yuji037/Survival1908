using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimatorSpeed : MonoBehaviour
{
	[SerializeField] private float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
		GetComponent<Animator>().speed = speed;
    }
}
