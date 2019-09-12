using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class CSoundClip{
	public string 		id;
	public AudioClip 	audioClip;
	[Range(0f, 1f)]
	public float		volume = 1f;
}

public class CSoundMan : CSingletonMonoBehaviour<CSoundMan> {
	[SerializeField]
	private 	GameObject 		m_oSoundPlayPrefab;
	private		AudioSource[] 	m_pChannels = new AudioSource[MAX_CHANNEL];

	private const int MAX_CHANNEL = 16;

	private 			CSoundClip[] 	m_pcSoundClips;

	Dictionary<string, CSoundClip> 		m_dicSoundClips = new Dictionary<string, CSoundClip>();

	private string m_sBgmId;
	private AudioSource m_Bgm;

	protected override void Awake()
	{
		base.Awake();
		m_pcSoundClips = Resources.Load<CSoundSettings>("CSoundSettings").m_pcSoundClips;

		for (int i = 0; i < m_pChannels.Length; ++i) {
			var obj = Instantiate(m_oSoundPlayPrefab);
			obj.transform.parent = this.transform;
			var audioSource = obj.GetComponent<AudioSource>();
			m_pChannels [i] = audioSource;
		}

		for (int i = 0; i < m_pcSoundClips.Length; ++i) {
			var sc = m_pcSoundClips [i];
			if (string.IsNullOrEmpty(sc.id)) {
				continue;
			}
			if (m_dicSoundClips.ContainsKey(sc.id)) {
				Debug.LogError("既に登録済みのサウンドIDです。 ID:" + sc.id);
				continue;
			}
			m_dicSoundClips [sc.id] = sc;
		}
	}

	public AudioSource Play(
		string 		sSoundID, 
		bool 		isLoop 		= false, 
		Vector3? 	vPosition 		= null, 
		bool 		bPlayIn3DVolume 	= false)
	{
		if (false == m_dicSoundClips.ContainsKey(sSoundID)) {
			Debug.LogError("登録されていないサウンドIDです ID:" + sSoundID);
			return null;
		}

		var audioSource = GetReadyChannel();
		if (audioSource == null) {
			return null;
		}
		var obj = audioSource.gameObject;
		obj.transform.position = vPosition ?? Vector3.zero;
		var clip = m_dicSoundClips[sSoundID].audioClip;
		if (clip == null) {
			return audioSource;
		}
		audioSource.clip = clip;

		if ( !bPlayIn3DVolume )
		{
			// 3D的なボリューム調節をしない
			audioSource.spatialBlend = 0f;
		}

		audioSource.volume = m_dicSoundClips [sSoundID].volume == 0f ?
			1f : m_dicSoundClips [sSoundID].volume;

		audioSource.loop = isLoop;

		audioSource.Play();

		return audioSource;
	}

	private AudioSource GetReadyChannel(){
		for (int i = 0; i < m_pChannels.Length; ++i) {
			if (false == m_pChannels [i].isPlaying) {
				return m_pChannels [i];
			}
		}

		Debug.LogError("使用可能なチャネルがありませんでした");
		return null;
	}

	public float GetClipLength(string soundID)
	{
		return m_dicSoundClips[soundID].audioClip.length;
	}

	public void Fadeout(AudioSource auSound)
	{
		StartCoroutine( FadeoutCoroutine( auSound ) );
	}

	IEnumerator FadeoutCoroutine(AudioSource se)
	{
		float defVolume = se.volume;
		for(float t = 0; t < 1f; t += Time.deltaTime )
		{
			se.volume =
				defVolume * ( 1f - t );
			yield return null;
		}
		se.volume = 0f;
		se.Stop();
	}

	#region BGM
	public void PlayBGM(string sBgmId){
		if (m_sBgmId == sBgmId) {
			return;
		}
		if(m_Bgm != null){
			m_Bgm.Stop();
		}
		m_Bgm = Play(sBgmId, true);
		m_sBgmId = sBgmId;
	}
	#endregion
}