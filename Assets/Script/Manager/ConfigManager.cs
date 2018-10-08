using UnityEngine;
using System.Collections;

// global configuration
public class ConfigManager : GSingletonMono<ConfigManager>
{
	Config m_config = null;

	public Config config
	{
		get { 
			if (null == m_config) {
				GameObject go = GameObject.Find ("Config");

				if (null == go)
					return null;
				
				m_config = go.GetComponent<Config> ();
			}
			return m_config;
		}
	}
		
	public bool CompanyMode {
		get {
			#if UNITY_EDITOR	
			return config.CompanyMode;
			#else
			return false;
			#endif
		}
	}

	// FPS 출력 여부 
	public bool showFPS = true;

	// 시작시 게임씬으로 바로 전환
	public bool forceSceneGame = false;

	// 실행시 세이브 리셋
	public bool resetSave = false;

	// 일정 시간 지나면 진행 속도 증가
	public float maxBallElpasedTime = 1.0f;
	public float fastForwardTimeScale = 1.5f;

	// 블럭과 충돌 없음
	public float maxNoCollBlock = 8f;

	public bool sound = true;

	public int maxBallCount = 50;


	public Color clrCamBlack = new Color (0f, 0f, 0f, 1f);
	public Color clrCam = new Color (52f / 255f, 52f / 255f, 52f / 255f, 1f);

}