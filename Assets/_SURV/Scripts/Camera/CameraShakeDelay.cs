using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeDelay : MonoBehaviour
{
	[SerializeField] private CameraShakeManager.StartDirectionType directionType = CameraShakeManager.StartDirectionType.DOWN;
	[SerializeField] private float delay = 0f;
	[SerializeField] private float strength = 0.1f;

	private float executeTime;
	private bool executed = false;

	// Start is called before the first frame update
	void Start()
    {
		executeTime = IngameTime.Time + delay;
	}

    // Update is called once per frame
    void Update()
    {
		if (executed) { return; }

		CheckExecute();
	}

	private void CheckExecute()
	{
		if (IngameTime.Time < executeTime) { return; }

		executed = true;
		CameraShakeManager.Instance.Shake(directionType, strength);
	}
}
