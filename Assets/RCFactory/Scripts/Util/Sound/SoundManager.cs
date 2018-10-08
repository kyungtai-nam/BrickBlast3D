using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager>
{
	protected SoundManager() {}

	private Dictionary<string, List<SoundFX>> m_hashList = new Dictionary<string, List<SoundFX>>();

	AudioClip[] clips = null;
	bool loaded = false;

	void Awake()
	{
	    clips = Resources.LoadAll<AudioClip>("Sound");
        // Debug.Log("SoundManager:Awake() audio clip=" + clips.Length);

        loaded = true;
    }

    public bool isLoaded()
    {
    	return loaded;
    }

	public void Release()
	{
		// 해제.

		if( 0 < m_hashList.Count )
		{
			foreach( KeyValuePair<string, List<SoundFX>> it in m_hashList )
			{
				for(int i=0,iMax=it.Value.Count; i<iMax; ++i)
				{
					GameObject.Destroy(it.Value[i].gameObject);
				}
				it.Value.Clear();
			}
			m_hashList.Clear();
		}
	}

	public void Stop()
	{
		// 정지.

		if( 0 < m_hashList.Count )
		{
			foreach( KeyValuePair<string, List<SoundFX>> it in m_hashList )
			{
				for(int i=0,iMax=it.Value.Count; i<iMax; ++i)
				{
					if( true == it.Value[i].gameObject.activeSelf )
						it.Value[i].PlaySFX = false;
				}
			}
		}
	}


	IEnumerator WaitToStop(SoundFX sfx, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		sfx.PlaySFX = false;
	}

	public SoundFX Play(string strResID, float volumn=1f, bool loop=false, float waitTime=0f)
	{
		SoundFX fx = Add(strResID, loop, null, 0f, volumn);
		fx.PlaySFX = true;

		if ( waitTime > 0 )
		{
			StartCoroutine(WaitToStop(fx, waitTime));
		}

		return fx;
	}

	public SoundFX Add(string strResID, bool IsLoop, Transform tmParent, float fDelay, float volumn=1f)
	{
		// 추가
		List<SoundFX> listSound = null;
		if( false == m_hashList.TryGetValue(strResID, out listSound) )
		{
			listSound = new List<SoundFX>();
			m_hashList.Add(strResID, listSound);
		}

		for(int i=listSound.Count; i>0;)
		{
			SoundFX sound = listSound[--i];
			if( null == sound )
			{
				listSound.RemoveAt(i);
			}	
			else
			{
				if( false == sound.gameObject.activeSelf )
				{
					sound.gameObject.SetActive(true);
					return sound;
				}
			}
		}

		SoundFX soundCreate = SoundFX.Create(Resources.Load(strResID) as AudioClip, IsLoop, tmParent, fDelay, false, volumn);
		if( null != soundCreate )
		{
			soundCreate.transform.parent = this.transform;
			listSound.Add(soundCreate);
		}
		return soundCreate;
	}

	public void Remove(SoundFX sound)
	{
		// 삭제
		if( null == sound )
			return;

		sound.gameObject.SetActive(false);
	}

}






