using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.Utility;
using VoxelBusters.NativePlugins;
using VoxelBusters.NativePlugins.Internal;


/*
	앱 시작시 Login() 호출 

*/
public class RcGameService : GSingletonMono<RcGameService>
{
	public const string LEADER_BOARD_GID = "Score";

	public const string ACH_GID_NEWBIE = "NEWBIE";
	public const string ACH_GID_BRONZE = "BRONZE";
	public const string ACH_GID_SILVER = "SILVER";
	public const string ACH_GID_GOLD = "GOLD";
	public const string ACH_GID_PLATINUM = "PLATINUM";

	public delegate void func(bool error);
	float m_fTimeOut = 0f;

	LocalUser.AuthenticationCompletion m_onLoginCallback = null;

	// can network?
	public bool available
	{
		get { return NPBinding.GameServices.IsAvailable(); }
	}

	// is signed
	public bool authenticated
	{
		get { return NPBinding.GameServices.LocalUser.IsAuthenticated; }
	}

	protected override void Awake()
	{
		base.Awake();

/*
		IDContainer[] _leaderboardGIDCollection	= NPSettings.GameServicesSettings.LeaderboardIDCollection;

		for(int i=0; i<_leaderboardGIDCollection.Length; ++i)
			m_listLeaderboard.Add(_leaderboardGIDCollection[i].GlobalID);
*/

/*
		IDContainer[] _achievementGIDCollection	= NPSettings.GameServicesSettings.AchievementIDCollection;

		for(int i=0; i<_achievementGIDCollection.Length; ++i)
			m_listAchievement.Add(_achievementGIDCollection[i].GlobalID);
*/
	}


	public bool IsMac()
	{
		string systemId = SystemInfo.deviceUniqueIdentifier;
	
		if ( 0 == string.Compare(Global.MAC_DEV_PC_ID, systemId, true) )
			return true;
		
		return false;
	}


	void OnEnable()
	{
/*		
		if( 0 == m_listLeaderboard.Count )
		{
#if UNITY_EDITOR
			Debug.Log("Could not find leaderboard id information. Please configure it.");
#endif
		}

		if( 0 == m_listAchievement.Count )
		{
#if UNITY_EDITOR
			Debug.Log("Could not find achievement id information. Please configure it.");
#endif
		}
*/
	}


	public void Reset()
	{
		GameServices gs = NPBinding.Instance.GetComponent<GameServices>();
		if( null != gs )
		{
//#if DEBUG_MODE
			Debug.Log("RcGameService::Reset GameServices="+gs);
//#endif
			GameObject.Destroy(gs);
		}
	}


	// 인증
	public void Login(LocalUser.AuthenticationCompletion onCallback, float fTimeOut=0f)
	{
		m_fTimeOut = fTimeOut;

		if( false == this.available )
		{
			if( null != onCallback )
				onCallback(false, "error");

			return;
		}

		m_onLoginCallback = onCallback;
		this.CancelInvoke("OnLoginTimeOut");

		if( 0f < m_fTimeOut )
			this.Invoke("OnLoginTimeOut", m_fTimeOut);
		
		NPBinding.GameServices.LocalUser.Authenticate(OnGameServiceLoginCB);
	}

	void OnGameServiceLoginCB(bool success, string error)
	{
		this.CancelInvoke("OnLoginTimeOut");

		if( null != m_onLoginCallback )
			m_onLoginCallback(success, error);
	}

	void OnLoginTimeOut()
	{
#if UNITY_EDITOR
#else
		OnGameServiceLoginCB(false, "Game service login time out");
#endif
	}



	public void Logout(LocalUser.SignOutCompletion onCallback)
	{
		// 로그아웃 (GooglePlay만 사용가능).

		if( false == this.available )
		{
			if( null != onCallback )
				onCallback(false, "error");
			return;
		}
		
		NPBinding.GameServices.LocalUser.SignOut(onCallback);
	}

	static public void OnLoginCB(bool _success, string _error)
	{
		if( !string.IsNullOrEmpty(_error) )
		{
			Debug.LogError(_error);
			return;
		}

		if( true == _success )
			Debug.Log(string.Format("Local user details= {0}.", NPBinding.GameServices.LocalUser));
	}

	static public void OnLogoutCB(bool _success, string _error)
	{
		if( !string.IsNullOrEmpty(_error) )
		{
			Debug.LogError(_error);
			return;
		}

		if( true == _success )
			Debug.Log("Local user is signed out successfully!");
	}





//
// Leader Board
//

	// 보기
	public void ShowLeaderBoardUI(GameServices.GameServiceViewClosed cbClose)
	{
		NPBinding.GameServices.ShowLeaderboardUIWithGlobalID (
			LEADER_BOARD_GID,
			eLeaderboardTimeScope.ALL_TIME,
			cbClose);
		/*
			(string _err) => {
				Debug.Log("Close leaderboard UI." + err);
			});
		*/
	}

	// 점수 저장
	public void SaveScore(long score, Score.ReportScoreCompletion onCallback)
	{
		if( false == this.available )
		{
			if( null != onCallback )
				onCallback(false, "error");
			return;
		}

		NPBinding.GameServices.ReportScoreWithGlobalID(LEADER_BOARD_GID, score, onCallback);
	}

	// 업적 UI
	public void ShowAchievement()
	{
	//	NPBinding.GameServices.ShowAchievementsUI ();

		NPBinding.GameServices.ShowAchievementsUI((string _error)=>{
//			AddNewResult("Achievements view dismissed.");
//			AppendResult(string.Format("Error= {0}.", _error.GetPrintableString()));
		});

	}

	// 업적 저장 
	public void SaveAchievement(string ach_gid)
	{
		int 	_noOfSteps	= NPBinding.GameServices.GetNoOfStepsForCompletingAchievement(ach_gid);
		double	_progress	= ((double)1/_noOfSteps) * 100d;

		NPBinding.GameServices.ReportProgressWithGlobalID(ach_gid, _progress, (bool _status, string _error)=>{

			// debug
			//string msg = string.Format("{0}={1}", _status == true ? "T":"F", _error);
			//UIManager.Inst.PopupMessage(msg, 5f);
		});
	}


	// not tested
	/*
	void LoadAchievement()
	{
		NPBinding.GameServices.LoadAchievements((Achievement[] _achievements, string _error)=>{

			if (_achievements == null)
			{
				Debug.Log("Couldn't load achievement list with error = " + _error);
				return;
			}

			int        _achievementCount    = _achievements.Length;
			Debug.Log(string.Format("Successfully loaded achievement list. Count={0}.", _achievementCount));

			for (int _iter = 0; _iter < _achievementCount; _iter++)
			{
				Debug.Log(string.Format("[Index {0}]: {1}", _iter, _achievements[_iter]));
			}
		});
	}
*/
}
