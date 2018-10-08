using UnityEngine;
using System.Collections;

public class SoundFX : MonoBehaviour
{
	// 사운크 클립 파일
	AudioClip clip = null;

	// 반복 여주
	bool loop = false;

	// 시작 딜레이
	float predelay = 0f;

	// 실제 플레이될 오디오소스
	AudioSource	audioSrc = null;

	// 플레이 중인가?
	bool isPlay = false;

	// 2D=0 , 3D=1
	float spatialBlend = 0f;

	// 볼륨 
	float volumn = 1f;

	public AudioSource AudioSrc
	{
		get
		{
			if ( null == audioSrc )
				audioSrc = gameObject.AddComponent<AudioSource>();

			return audioSrc;
		}
	}

	public bool PlaySFX
	{
		set
		{
			if ( isPlay == value )
				return;

			isPlay = value;

			if ( value ) 
			{
				StartCoroutine(OnPlay());
				return;
			}

			StopCoroutine(OnPlay());
			audioSrc.Stop();

			SoundManager.Inst.Remove(this);
		}

	}

	void OnEnable()
	{
		if ( null == clip )
		{
			Debug.LogError("SoundFX:OnEnable() null == clip");
			return;
		}

		audioSrc.clip			= clip;
	
		audioSrc.mute = false;
		audioSrc.bypassEffects	= false;
		audioSrc.playOnAwake	= false;
		audioSrc.loop			= loop;
		audioSrc.spread			= 0f;
		audioSrc.minDistance	= 1f;
		audioSrc.panStereo		= 0;
		audioSrc.volume			= volumn;
		audioSrc.spatialBlend	= spatialBlend;
		PlaySFX					= true;
	}

	void OnDisable()
	{
		PlaySFX = false;
	}
	
	IEnumerator OnPlay()
	{
		audioSrc.mute = !RcSaveData.Inst.sound;

		if( 0f < predelay )
			yield return new WaitForSeconds(predelay);
		
		audioSrc.Play();
		
		if( false == audioSrc.loop )
		{
			while(audioSrc.isPlaying)
			{
				yield return null;
			}
			PlaySFX = false;
		}
	}
	

	static public SoundFX Create(AudioClip _clip, bool _loop, Transform tmParent, float _predelay=0f, bool _3d = false, float _volumn=1f)
	{
		if( null == _clip )
		{
			Debug.LogError("SoundFX:Create() null == _clip");
			return null;
		}	

		GameObject go = new GameObject();
		go.SetActive(false);

		SoundFX sound = go.GetComponent<SoundFX>();
		if ( null == sound )
			sound = go.AddComponent<SoundFX>();

		AudioSource src = go.GetComponent<AudioSource>();
		if ( null == src )
			src = go.AddComponent<AudioSource>();

		go.name = _clip.name;
		go.transform.position = (null == tmParent) ? Vector3.zero : tmParent.position;

		sound.clip = _clip;
		sound.loop = _loop;
		sound.predelay = _predelay;
		sound.volumn = _volumn;
		sound.spatialBlend = (true == _3d) ? 1f : 0f;
		sound.audioSrc = src;

		go.SetActive(true);
		return sound;
	}
}



