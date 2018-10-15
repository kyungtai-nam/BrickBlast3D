using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RcSceneManager : GSingletonMono<RcSceneManager> {
	
	Transform m_tmScene = null;
	Transform m_tmCurrentScene = null;

	string m_strScene = null;

	public Transform tmScene {
		get {
			if ( null == m_tmScene )
				m_tmScene = GameObject.Find("Scene").transform;
			return m_tmScene;
		}
	}

	public Transform tmCurrent {
		get {
			return m_tmCurrentScene;
		}
	}

	public string SceneName {
		get {
			return m_strScene;
		}
	}

	public bool ChangeScene(string sceneName)
	{
		if ( null != m_tmCurrentScene ) 
			m_tmCurrentScene.gameObject.SetActive(false);

		m_tmCurrentScene = tmScene.Find(sceneName).transform;

		if ( null == m_tmCurrentScene ) 
		{
			Debug.LogError("RcSceneManager:ChangeScene not found " + sceneName);
			return false;
		}	

		m_strScene = sceneName;
		//GGoogleAnalyticsManager.Inst.Log(Application.productName, sceneName);

		Debug.LogFormat("RcSceneManager:ChangeScene {0} to {1}", Application.productName, sceneName);
		m_tmCurrentScene.gameObject.SetActive(true);
		return true;
	}

}
