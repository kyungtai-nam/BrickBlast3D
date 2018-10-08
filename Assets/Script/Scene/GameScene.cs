using UnityEngine;
using System.Collections;

public class GameScene : MonoBehaviour {	
	void Awake()
	{
		PlayManager.Inst.cbGameResult = ResultGame;
		UIManager.Inst.uiReplay.cbTouched = ReplayGame;
		UIManager.Inst.uiHome.cbTouched = GoHome;
		UIManager.Inst.uiPlayResume.cbTouched = ResumeGame;
		UIManager.Inst.uiPause.cbTouched = PauseGame;
		UIManager.Inst.uiPause.cbResume = ResumeGame;
		UIManager.Inst.uiHome.cbTouched = GoHome;

		UIManager.Inst.uiAdToContinue.cbTouched = ContinueGame;
	}

	void OnEnable()
	{		
		Debug.Log ("GameScene:OnEnable()");

		UIManager.Inst.uiPause.isPaused = false;

		InitGame ();
	}

	void InitGame(bool isNew = true)
	{
		Debug.Log ("GameScene:InitGame()");

		ShowResultUI (false);
		ShowGameUI (true);
		PlayManager.Inst.Init (isNew);
	}

	void OnApplicationPause( bool pauseStatus )
	{
		Debug.Log ("GameScene:OnApplicationPause() " + pauseStatus);

		// 안드로이드에서 Home버튼을 눌렀을 때처럼, 앱이 soft close 되면 호출된다. 
		// pauseStatus의 값에 따라 true는 soft close, false는 soft close 후에 다시 앱이 화면으로 올라왔을 때를 구분할 수 있다.
	}


	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Debug.Log ("GameScene:Escape");
			UIManager.Inst.uiPause.OnTouched ();
			return;
		}
	}

	void ShowResultUI(bool isShow)
	{
		UIManager.Inst.touchBeginPoint.SetActive (false);
		UIManager.Inst.touchDragPoint.SetActive (false);
		UIManager.Inst.forwardIcon.SetActive (false);

		UIManager.Inst.goScreenLocker.SetActive (isShow);
		GameManager.Inst.fireworks.gameObject.SetActive (isShow);

		if (isShow) {
			UIManager.Inst.uiResultScore.gameObject.SetActive (true);
			UIManager.Inst.uiHome.gameObject.SetActive (true);
			UIManager.Inst.uiReplay.gameObject.SetActive (true);
			UIManager.Inst.uiAdToContinue.gameObject.SetActive (true);

			UIManager.Inst.uiResultScore.Show ();
			UIManager.Inst.uiHome.Show ();
			UIManager.Inst.uiReplay.Show ();
			UIManager.Inst.uiAdToContinue.Show ();

			return;
		}

		UIManager.Inst.uiResultScore.Hide ();
		UIManager.Inst.uiHome.Hide ();
		UIManager.Inst.uiReplay.Hide ();
		UIManager.Inst.uiAdToContinue.Hide ();

	}

	void ShowGameUI(bool isShow)
	{
		if (isShow) {
			UIManager.Inst.uiPause.gameObject.SetActive (true);
			UIManager.Inst.uiPause.Show();

			UIManager.Inst.uiStep.gameObject.SetActive (true);
			UIManager.Inst.uiStep.Show ();

			// UIManager.Inst.uiStar.gameObject.SetActive (true);
			// UIManager.Inst.uiStar.Show ();

			UIManager.Inst.uiScore.gameObject.SetActive (true);
			UIManager.Inst.uiScore.Show ();
			return;
		}

		UIManager.Inst.uiPause.Hide();
		UIManager.Inst.uiStep.Hide();
		//UIManager.Inst.uiStar.Hide();
		UIManager.Inst.uiScore.Hide();
	}



	void PauseGame()
	{
		Debug.Log ("GameScene:PauseGame()");

		UIManager.Inst.goScreenLocker.SetActive (true);
		// UIManager.Inst.uiPlayResume.gameObject.SetActive (true);
		// UIManager.Inst.uiPlayResume.Show();
	}

	void ResumeGame()
	{
		Debug.Log ("GameScene:ResumeGame()");

		UIManager.Inst.goScreenLocker.SetActive (false);
		// UIManager.Inst.uiPlayResume.Hide();
	}
		
	void GoHome()
	{
		PlayManager.Inst.Release ();
		ShowGameUI (false);
		ShowResultUI (false);

		RcSceneManager.Inst.ChangeScene("HomeScene");
	}

	void ReplayGame()
	{
		// Debug.Log ("PopupResult:ReplayGame()");
		PlayManager.Inst.Release ();

		ShowResultUI (false);
		InitGame ();
	}


	void ResultGame()
	{
		Debug.Log("GameScene:ResultGame()");

		// update score
		RcGameService.Inst.SaveScore(PlayManager.Inst.score, null);
		RcSaveData.Inst.lastScore = PlayManager.Inst.score;
		RcSaveData.Inst.CheckBestScore ();
		RcSaveData.Inst.Save ();

		// ui
		ShowGameUI(false);
		ShowResultUI (true);

		// full banner
		bool showAd = false;

		if (RcAdManager.Inst.IsShowFullBanner()) {
			showAd = true;
			RcAdManager.Inst.ShowFullBanner ();	
		} 

		// rate
		if ( !showAd )
			RcShare.Inst.RateRequstSometimes ();
	}

	void ContinueGame()
	{
		Debug.Log ("GameScene:ContinueGame()");

		UIManager.Inst.uiPause.isPaused = false;
		InitGame (false);
	}
}