using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlaySoundDelay : MonoBehaviour
{
	[SerializeField]
	private string soundId;

	[SerializeField]
	private float delayTime = 0f;

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
			CSoundMan.Instance.Play(soundId);
			played = true;
		}
    }
}
