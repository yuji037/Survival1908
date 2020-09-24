using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound3DDelay : MonoBehaviour
{
	[SerializeField] private string soundId = default;
	[SerializeField] private float delayTime = 0f;

	private float elapsedTime = 0f;
	private bool played = false;

    // Update is called once per frame
    void Update()
    {
		if ( played )
			return;

		elapsedTime += Time.deltaTime;
		if ( elapsedTime >= delayTime )
		{
			SoundManager.Instance.Play(soundId, false, transform.position, true);
			played = true;
		}
    }
}
