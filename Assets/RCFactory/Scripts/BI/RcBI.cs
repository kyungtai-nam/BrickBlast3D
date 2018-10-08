using UnityEngine;
using System.Collections;
using IgaworksUnityAOS;

// 지표분석을 위한 wrapper
public class RcBI : GSingletonMono<RcBI>
{

	public void Init()
	{
		#if !UNITY_EDITOR		
		//유니티 엔진이 초기화될 때, IGAW 플러그인을 초기화 합니다.
		IgaworksUnityPluginAOS.InitPlugin ();
		//네이티브 SDK를 초기화 합니다.
		IgaworksUnityPluginAOS.Common.startApplication();
		#endif
	}

	//앱 시작
	void Start()
	{
		// 앱이 최초로 실행 될 때는 OnApplicationPause를 리턴하지 않기 때문에 직접 startSession 을 호출.
		#if !UNITY_EDITOR		
		IgaworksUnityPluginAOS.Common.startSession();
		#endif
	}


	//앱 상태 체크
	void OnApplicationPause(bool pauseStatus) 
	{
		#if !UNITY_EDITOR
		if (pauseStatus) 
		{
			Debug.Log ("go to Background");
			IgaworksUnityPluginAOS.Common.endSession();
		} 
		else 
		{
			Debug.Log ("go to Foreground");
			IgaworksUnityPluginAOS.Common.startSession();
		}
		#endif
	}

	/*
		In App Activity
		retention API를 이용하여 유저의 모든 일반적인 행동을 추적합니다.
	*/	
	public void Tracking(string desc)
	{
		#if !UNITY_EDITOR		
		// IGAWorksManager.Inst.Tracking("Ad_NoMoreVideo");
		IgaworksUnityPluginAOS.Adbrix.retention(desc);
		#endif
	}


	/*
		최초이용활동 분석(New User Session)은 최초로 앱을 실행한 유저의 행동 패턴을 추적합니다.
	 	이 API를 이용하여 최초로 유입된 유저의 이탈 시점을 파악할 수 있습니다.

		firstTimeExperience API를 호출하여 유저의 행동 패턴을 추적하며 최초이용활동 분석에서는 최초로 유입된 당일의 데이터만을 제공합니다.
	*/
	public void NewUserSession(string sceneName)
	{
		#if !UNITY_EDITOR
		string val = GCryptPlayerPrefs.GetString(sceneName, "0");

		if ( 0 != string.Compare(val, "0", true) )
			return;

		IgaworksUnityPluginAOS.Adbrix.firstTimeExperience(sceneName);
		GCryptPlayerPrefs.SetString(sceneName, "1");
		GCryptPlayerPrefs.Save();		

		#endif
	}

	// In App Purchasing
	public void Buy(string itemName)
	{
		#if !UNITY_EDITOR
		IgaworksUnityPluginAOS.Adbrix.buy(itemName);
		#endif
	}


	public void Scene(string projectName, string sceneName)
	{
		#if !UNITY_EDITOR
		//GGoogleAnalyticsManager.Inst.Log("Storm Bow", "Home");
		GGoogleAnalyticsManager.Inst.Log(projectName, sceneName);
		#endif
	}
}