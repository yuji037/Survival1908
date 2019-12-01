using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFollowCamera : MonoBehaviour
{
	[SerializeField]
	GameObject follow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		var newPos = follow.transform.position;

		transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
