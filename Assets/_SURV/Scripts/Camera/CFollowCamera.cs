using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFollowCamera : MonoBehaviour
{
	[SerializeField]
	GameObject follow;

	[SerializeField]
	float elasticity = 2f;
	[SerializeField]
	float dampFactor = 0.98f;

	// Start is called before the first frame update
	void Start()
    {
		CCameraShake.Instance.Init(this);
    }

    // Update is called once per frame
    void Update()
    {
		CCameraShake.Instance.elasticity = elasticity;
		CCameraShake.Instance.dampFactor = dampFactor;

		CCameraShake.Instance.OnUpdate();

		var targetPos = follow.transform.position + CCameraShake.Instance.shakeOffset;

		transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
    }
}
