using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using VoxelBusters.NativePlugins;

public class HomeScene : MonoBehaviour 
{	
	public bool forcePlay = true;

	void Awake()
	{
#if UNITY_EDITOR		
		Application.runInBackground = true;
		Localization.language = "English";
#else
		SetLanguage ();
#endif
		RcBI.Inst.Init ();
		RcBI.Inst.NewUserSession ("Home");

		RcShare.Inst.Init ();

		// reset 하려고 하면!
		if ( ConfigManager.Inst.resetSave ) 
			RcSaveData.Inst.Save();	
		RcSaveData.Inst.Load();
			

		RcAdManager.Inst.Init ();

		if ( RcSaveData.Inst.showAd ) 
			RcAdManager.Inst.ShowBanner ();
		else
			RcAdManager.Inst.CloseBanner ();

		RcBilling.Inst.Init(cbBillingInit);

		UIManager.Inst.uiPlay.cbTouched = OnChangeGameScene;

		SoundManager.Inst.Play ("Sound/AppIntro");
	}


	void SetLanguage() {
		//Debug.Log("Application.systemLanguage : "+Application.systemLanguage);
		//Debug.Log("System.Language : "+SystemLanguage.Korean);

		switch (Application.systemLanguage){
			case SystemLanguage.Korean: Localization.language = "Korean"; break;
			case SystemLanguage.English: Localization.language = "English"; break;
			case SystemLanguage.French: Localization.language = "French"; break;
			case SystemLanguage.Italian: Localization.language = "Italian"; break;
			case SystemLanguage.Dutch: Localization.language = "Dutch"; break;
			case SystemLanguage.Spanish: Localization.language = "Spanish"; break;
			case SystemLanguage.Japanese: Localization.language = "Japanese"; break;
			case SystemLanguage.ChineseSimplified: Localization.language = "ChineseSimplified"; break;
			case SystemLanguage.ChineseTraditional: Localization.language = "ChineseTraditional"; break;
			case SystemLanguage.Portuguese: Localization.language = "Portuguese"; break;
			case SystemLanguage.Russian: Localization.language = "Russian"; break;
			case SystemLanguage.Turkish: Localization.language = "Turkish"; break;
			case SystemLanguage.Arabic: Localization.language = "Arabic"; break;
			default :
				Localization.language = "English";
				break;
		}
	}


	void OnEnable()
	{
		Debug.Log ("HomeScene:OnEnable()");
		Camera.main.backgroundColor = ConfigManager.Inst.clrCam;

		ShowUI (true);

		GameManager.Inst.tmCameraTopView.gameObject.SetActive (false);
		GameManager.Inst.demoBall.SetActive (true);
	}

	void OnDisable()
	{
		Debug.Log ("HomeScene:OnDisable()");

		if (!this.enabled)
			return;

		ShowUI (false);
		GameManager.Inst.demoBall.SetActive (false);
	}


	/*
	void OnApplicationQuit()
	{
		// 어플리케이션을 종료하는 순간에 처리할 행동들
	}
	*/


	void cbBillingInit(bool result)
	{
		if (!RcSaveData.Inst.showAd) 
			return;

		// PC에서 HasProduct 제대로 파악 안됨 ㅡㅡ;

		// UIManager.Inst.Alert("cbBilling=" + result);
		
		//bool hasProduct = RcBilling.Inst.HasProduct ("remove_banner");
		//Debug.Log("HomeScene:cbBillingInit() remove_banner has=" + hasProduct);	
	}


	void Start()
	{
		Debug.Log ("HomeScene:Start()");
		GGoogleAnalyticsManager.Inst.Log("Iso Brick", "HomeScene");
	
		if (!RcGameService.Inst.IsMac ())
			UIManager.Inst.uiSign.Sign ();
		
		if (forcePlay)
			UIManager.Inst.uiPlay.OnTouched ();	

		// UIManager.Inst.PopupMessage ("ad=" + RcSaveData.Inst.showAd, 5f);
	}

	public void ShowUI(bool isShow)
	{
		SetUI (true);

		if (isShow) {
			UIManager.Inst.uiTitle.Show();
			UIManager.Inst.uiPlay.Show();

			UIManager.Inst.uiSign.Show();
			UIManager.Inst.uiRate.Show();
			UIManager.Inst.uiSound.Show();

			UIManager.Inst.uiAchievement.Show();
			UIManager.Inst.uiLeaderBoard.Show();
			UIManager.Inst.uiShare.Show();

			if ( RcSaveData.Inst.showAd ) 
				UIManager.Inst.uiRemoveAd.Show();
			return;
		}

		UIManager.Inst.uiTitle.Hide();
		UIManager.Inst.uiPlay.Hide();

		UIManager.Inst.uiSign.Hide();
		UIManager.Inst.uiRate.Hide();
		UIManager.Inst.uiSound.Hide();

		UIManager.Inst.uiAchievement.Hide();
		UIManager.Inst.uiLeaderBoard.Hide();
		UIManager.Inst.uiShare.Hide();

		if ( RcSaveData.Inst.showAd ) 
			UIManager.Inst.uiRemoveAd.Hide();
	}

	public void SetUI(bool isShow)
	{
		if ( ConfigManager.Inst.showFPS ) 
			UIManager.Inst.uiFPS.gameObject.SetActive (true);

		UIManager.Inst.uiVersion.gameObject.SetActive(true);


/*
#if UNITY_EDITOR
		UIManager.Inst.bannerArea.SetActive(true);
#else
		UIManager.Inst.bannerArea.SetActive(false);
#endif
*/
		// if (ConfigManager.Inst.CompanyMode)
		//	isShow = false;

		UIManager.Inst.uiTitle.gameObject.SetActive (isShow);
		UIManager.Inst.uiPlay.gameObject.SetActive (isShow);

		UIManager.Inst.uiSign.gameObject.SetActive (isShow);
		UIManager.Inst.uiRate.gameObject.SetActive (isShow);
		UIManager.Inst.uiSound.gameObject.SetActive (isShow);

		UIManager.Inst.uiAchievement.gameObject.SetActive (isShow);
		UIManager.Inst.uiLeaderBoard.gameObject.SetActive (isShow);
		UIManager.Inst.uiShare.gameObject.SetActive (isShow);

		if ( RcSaveData.Inst.showAd ) 
			UIManager.Inst.uiRemoveAd.gameObject.SetActive (isShow);
		else
			UIManager.Inst.uiRemoveAd.gameObject.SetActive (false);
	}

	void Update()
	{
		if ( Input.GetKeyDown(KeyCode.Escape) )
		{
			Application.Quit();
			return;
		}

		UpdateKey();
	}
		
	void UpdateKey()
	{		
#if UNITY_EDITOR		
		if (Input.GetKeyDown (KeyCode.C)) {
			UIManager.Inst.PopupMessage ("SET ENG");
			Localization.language = "English";
			return;
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			UIManager.Inst.PopupMessage ("HEADSHOT");
			return;
		}

		if (Input.GetKeyDown (KeyCode.G)) {
			Debug.Log("HomeScene:KeyHome() G gold+=10");
			RcSaveData.Inst.gold += 10;
			return;
		}

		if (Input.GetKeyDown (KeyCode.J)) {
			Debug.Log("HomeScene:KeyHome() J gem+=50");
			RcSaveData.Inst.gem += 100;
			return;
		}

		if (Input.GetKeyDown (KeyCode.L)) { 
			GGoogleAnalyticsManager.Inst.Log(Application.productName, "HomeScene");
			return;
		}
#endif
	}


	void OnChangeGameScene()
	{
		Debug.Log ("HomeScene:OnChangeGameScene()");
		RcSceneManager.Inst.ChangeScene("GameScene");
	}
		
}