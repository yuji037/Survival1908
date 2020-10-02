using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	[SerializeField] private float elasticity = 2f;
	[SerializeField] private float dampFactor = 0.98f;

	private Transform followTarget;

	public void SetFollowTarget(Transform target)
	{
		followTarget = target;
	}

	// Start is called before the first frame update
	void Start()
    {
		CameraShakeManager.Instance.Init(this);
    }

    // Update is called once per frame
    void Update()
    {
		if(null == followTarget) { return; }

		CameraShakeManager.Instance.elasticity = elasticity;
		CameraShakeManager.Instance.dampFactor = dampFactor;

		CameraShakeManager.Instance.OnUpdate();

		var targetPos = followTarget.position + CameraShakeManager.Instance.shakeOffset;

		transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
    }
}
