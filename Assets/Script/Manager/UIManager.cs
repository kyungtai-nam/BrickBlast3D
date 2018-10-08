using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : GSingletonMono<UIManager>
{
	// entry
	Transform m_tmRoot = null;

	// anchors
	Transform m_tmMenu = null;
	Transform m_tmPopup = null;

	Transform m_tmBottomRight = null;
	Transform m_tmBottom = null;
	Transform m_tmTop = null;
	Transform m_tmTopRight = null;
	Transform m_tmTopLeft = null;

	// anchors - to rename
	Transform m_tmAnchorBottomLeft = null;
	Transform m_tmAnchorTop = null;
	Transform m_tmAnchorTopLeft = null;
	Transform m_tmAnchorTopRight = null;
	Transform m_tmAnchorCenter = null;


	// instance
	Camera m_cameraUI = null;


	public Transform tmRoot {
		get {
			if (null == m_tmRoot)
				m_tmRoot = GameObject.Find ("UI Root").transform;			
			return m_tmRoot;
		}
	}

	public Transform tmBottom {
		get {
			if ( null == m_tmBottom )
				m_tmBottom = tmRoot.Find("Bottom").transform;
			return m_tmBottom;
		}	
	}

	public Transform tmTop {
		get {
			if ( null == m_tmTop )
				m_tmTop = tmRoot.Find("Top").transform;
			return m_tmTop;
		}	
	}
		
	public Transform tmTopRight {
		get {
			if ( null == m_tmTopRight )
				m_tmTopRight = tmRoot.Find("TopRight").transform;
			return m_tmTopRight;
		}	
	}		

	public Transform tmTopLeft {
		get {
			if ( null == m_tmTopLeft )
				m_tmTopLeft = tmRoot.Find("TopLeft").transform;
			return m_tmTopLeft;
		}
	}		

	public Transform tmMenu {
		get {
			if ( null == m_tmMenu )
				m_tmMenu = tmRoot.Find("HolderMenu").transform;
			return m_tmMenu;
		}
	}
		
	public Transform tmAnchorBottomRight {
		get {
			if ( null == m_tmBottomRight )
				m_tmBottomRight = tmRoot.Find("BottomRight").transform;
			return m_tmBottomRight;
		}
	}

	public Transform tmAnchorBottomLeft {
		get {
			if ( null == m_tmAnchorBottomLeft )
				m_tmAnchorBottomLeft = tmRoot.Find("BottomLeft").transform;
			return m_tmAnchorBottomLeft;
		}
	}

	public Transform tmAnchorTop {
		get {
			if ( null == m_tmAnchorTop )
				m_tmAnchorTop = tmRoot.Find("Top").transform;
			return m_tmAnchorTop;
		}
	}

	public Transform tmAnchorTopLeft {
		get {
			if ( null == m_tmAnchorTopLeft )
				m_tmAnchorTopLeft = tmRoot.Find("TopLeft").transform;
			return m_tmAnchorTopLeft;
		}
	}

	public Transform tmAnchorTopRight {
		get {
			if ( null == m_tmAnchorTopRight )
				m_tmAnchorTopRight = tmRoot.Find("TopRight").transform;
			return m_tmAnchorTopRight;
		}
	}

	public Transform tmAnchorCenter {
		get {
			if ( null == m_tmAnchorCenter )
				m_tmAnchorCenter = tmRoot.Find("Center").transform;
			return m_tmAnchorCenter;
		}
	}

	public Transform tmPopup {
		get {
			if ( null == m_tmPopup )
				m_tmPopup = tmRoot.Find("Popup").transform;
			return m_tmPopup;
		}
	}


	public Camera cameraUI {
		get {
			if (null == m_cameraUI) {
				m_cameraUI = tmRoot.Find ("Camera").GetComponent<Camera>();
			}
			return m_cameraUI;
		}
	}


	public Transform GetHolder(Transform tmAnchor, int idx)
	{
		return tmAnchor.Find(string.Format ("Holder{0}", idx));
	}
			
	public Transform GetHolder(Transform tmAnchor, string holderName)
	{
		Transform tm = tmAnchor.Find (holderName);

		if ( null == tm ) 
			Debug.LogError(string.Format("UIManager:GetHolder() {0} is null", holderName));
		
		return tm;
	}

	GameObject CreateObj(string pathPrefab, Transform tmParent, bool resetPos=true)
	{
		if (null == tmParent) {
			Debug.LogError (string.Format ("UIManager:CreateObject() {0} null == tmParent", pathPrefab));

		}
		GameObject go = GameObject.Instantiate(Resources.Load(pathPrefab)) as GameObject;
		Transform tm = go.transform;

		go.SetActive(false);

		tm.parent = tmParent;

		if ( true == resetPos ) 
			tm.localPosition = Vector3.zero;
		tm.localScale = Vector3.one;
		return go;
	}


	GameObject FindOrCreateObj(string prefabPath, string prefabName, Transform tmParent)
	{
		if (null == tmParent) {
			Debug.LogWarningFormat ("UIManager:FindOrCreateObj() {0}{1} null == tmParent", prefabPath, prefabName);
		}
				
		Transform tm = tmParent.Find (prefabName);
		if (null == tm) {
			Debug.LogFormat("UIManager:FindOrCreateObj() not found tm {0}{1}. Instantiate!", prefabPath, prefabName);

			GameObject go = GameObject.Instantiate (Resources.Load (prefabPath + prefabName)) as GameObject;
			tm = go.transform;

			if (null == tm) {
				Debug.LogErrorFormat("UIManager:FindOrCreateObj() not found tm {0}{1}.", prefabPath, prefabName);
			}
		}
		else
			Debug.LogFormat("UIManager:FindOrCreateObj() found tm {0}{1}", prefabPath, prefabName);
		
		tm.gameObject.SetActive (false);
		tm.parent = tmParent;
		tm.localScale = Vector3.one;	
		tm.localPosition = Vector3.zero;
		return tm.gameObject;
	}




	////////////////////////////////////////
	// instances

	// top left 
	UIStep m_uiStep = null;

	// top 
	UIScore m_uiScore = null;


	// top right
	RcUIStar m_uiStar = null;
	RcUIPause m_uiPause = null;


	// Center
	GameObject m_goScreenLocker = null;

	RcUITitle m_uiTitle = null;
	RcUIResultScore m_uiResultScore = null;
	RcUIHome m_uiHome = null;
	RcUIPlay m_uiPlay = null;

	RcUIPlay m_uiPlayResume = null;
	RcUIPlay m_uiReplay = null;
	RcUIAdToContinue m_uiAdToContinue = null;

	// Bottom left
	RcUISign m_uiSign = null;
	RcUIRate m_uiRate = null;
	RcUISound m_uiSound = null;

	// Bottom Right
	RcUIRemoveAd m_uiRemoveAd = null;
	RcUIAchievement m_uiAchievement = null;
	RcUILeaderBoard m_uiLeaderBoard = null;
	RcUIShare m_uiShare = null;

	// Bottom
	UIVersion m_uiVersion = null;



	public GameObject goScreenLocker {
		get {
			if (null == m_goScreenLocker) {
				m_goScreenLocker = FindOrCreateObj ("Prefab/UI/", "ScreenLocker", tmAnchorCenter);
			}
			return m_goScreenLocker;
		}
	}
		
	public RcUITitle uiTitle {
		get {
			if (null == m_uiTitle) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "Title", GetHolder (tmAnchorCenter, "Title"));
				m_uiTitle = go.GetComponent<RcUITitle> ();
			}
			return m_uiTitle;
		}
	}
		
	public RcUIResultScore uiResultScore {
		get {
			if (null == m_uiResultScore) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "ResultScore", GetHolder (tmAnchorCenter, "ResultScore"));
				m_uiResultScore = go.GetComponent<RcUIResultScore> ();
			}
			return m_uiResultScore;
		}
	}

	public RcUIHome uiHome {
		get {
			if (null == m_uiHome) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "Home", GetHolder (tmAnchorBottomRight, "Home"));
				m_uiHome = go.GetComponent<RcUIHome> ();
			}
			return m_uiHome;
		}
	}

	public RcUIPlay uiPlay {
		get {
			if (null == m_uiPlay) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "Play", GetHolder (tmAnchorCenter, "Play"));
				m_uiPlay = go.GetComponent<RcUIPlay> ();
			}
			return m_uiPlay;
		}
	}

	public RcUIPlay uiPlayResume {
		get {
			if (null == m_uiPlayResume) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "PlayResume", GetHolder (tmAnchorCenter, "PlayResume"));
				m_uiPlayResume = go.GetComponent<RcUIPlay> ();
			}
			return m_uiPlayResume;
		}
	}

	public RcUIPlay uiReplay {
		get {
			if (null == m_uiReplay) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "Replay", GetHolder (tmAnchorBottomLeft, "Replay"));
				m_uiReplay = go.GetComponent<RcUIPlay> ();
			}
			return m_uiReplay;
		}
	}

	public RcUIAdToContinue uiAdToContinue {
		get {
			if (null == m_uiAdToContinue) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "AdToContinue", GetHolder (tmAnchorCenter, "AdToContinue"));
				m_uiAdToContinue = go.GetComponent<RcUIAdToContinue> ();
			}
			return m_uiAdToContinue;
		}
	}
		
	public RcUISign uiSign {
		get {
			if (null == m_uiSign) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "Sign", GetHolder (tmAnchorBottomLeft, "Sign"));
				m_uiSign = go.GetComponent<RcUISign> ();
			}
			return m_uiSign;
		}
	}

	public RcUIRate uiRate {
		get {
			if (null == m_uiRate) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "Rate", GetHolder (tmAnchorBottomLeft, "Rate"));
				m_uiRate = go.GetComponent<RcUIRate> ();
			}
			return m_uiRate;
		}
	}

	public RcUIAchievement uiAchievement {
		get {
			if (null == m_uiAchievement) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "Achievement", GetHolder (tmAnchorBottomRight, "Achievement"));
				m_uiAchievement = go.GetComponent<RcUIAchievement> ();
			}
			return m_uiAchievement;
		}
	}
		
	public RcUISound uiSound {
		get {
			if (null == m_uiSound) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "Sound", GetHolder (tmAnchorBottomLeft, "Sound"));
				m_uiSound = go.GetComponent<RcUISound> ();
			}
			return m_uiSound;
		}
	}

	public RcUIRemoveAd uiRemoveAd {
		get {
			if (null == m_uiRemoveAd) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "RemoveAd", GetHolder (tmAnchorBottomRight, "RemoveAd"));
				m_uiRemoveAd = go.GetComponent<RcUIRemoveAd> ();
			}
			return m_uiRemoveAd;
		}
	}

	public RcUILeaderBoard uiLeaderBoard {
		get {
			if (null == m_uiLeaderBoard) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "Leaderboard", GetHolder (tmAnchorBottomRight, "Leaderboard"));
				m_uiLeaderBoard = go.GetComponent<RcUILeaderBoard> ();
			}
			return m_uiLeaderBoard;
		}
	}

	public RcUIShare uiShare {
		get {
			if (null == m_uiShare) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "Share", GetHolder (tmAnchorBottomRight, "Share"));
				m_uiShare = go.GetComponent<RcUIShare> ();
			}
			return m_uiShare;
		}
	}

	public RcUIPause uiPause {

		get {
			if (null == m_uiPause) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "Pause", GetHolder (tmAnchorTopRight, "Pause"));
				m_uiPause = go.GetComponent<RcUIPause>();
			}

			return m_uiPause;
		}
	}

	public UIVersion uiVersion {
		get {
			if (null == m_uiVersion) {
				GameObject go = FindOrCreateObj("Prefab/UI/", "UIVersion", GetHolder(tmAnchorBottomRight, "Version"));
				m_uiVersion = go.GetComponent<UIVersion>();
			}
			return m_uiVersion;
		}
	}




	public void Alert(string msg) {
		GameObject obj = GPoolManager.Inst.Add("Prefab/AlertLabel", tmRoot);
		UIAlertLabel lb = obj.GetComponent<UIAlertLabel> ();
		lb.Init (msg);
		obj.SetActive (true);
	}


	public void PopupMessage(string msg, float duration=0.6f) {
		GameObject obj = GPoolManager.Inst.Add("Prefab/UI/PopupMessage", tmPopup);
		UIPopupMessage ui = obj.GetComponent<UIPopupMessage> ();
		ui.Show (msg, duration);
		obj.SetActive (true);
	}




	FPSNgui m_uiFPS = null;

	public FPSNgui uiFPS {
		get {
			if (null == m_uiFPS) {
				//GameObject go = FindOrCreateObj("Prefab/UI/", "FPS", GetHolder (tmAnchorTopLeft, "HolderFPS"));
				GameObject go = FindOrCreateObj("Prefab/UI/", "FPS", tmRoot);
				m_uiFPS = go.GetComponent<FPSNgui> ();
			}
			return m_uiFPS;
		}
	}




	///////////////////////////////
	/// In Game UI


	// center
	GameObject m_BannerArea = null;

	public GameObject bannerArea {
		get {
			if ( null == m_BannerArea ) {
				m_BannerArea = FindOrCreateObj ("Prefab/UI/", "BannerArea", tmRoot);// GetHolder (tmAnchorBottom, "HolderBanner"));
			}	
			return m_BannerArea;
		}
	}
	



	Transform m_popupOption = null;

	public Transform popupOption {
		get {
			if (null == m_popupOption) {
				GameObject go = CreateObj ("Prefab/Popup/PopupOption", tmPopup);
				m_popupOption = go.transform;
			}
			return m_popupOption;
		}
	}


	Transform m_popupGem = null;

	public Transform popupGem {
		get {
			if ( null == m_popupGem ) {
				GameObject go = CreateObj ("Prefab/Popup/PopupGem", tmPopup);
				m_popupGem = go.transform;
			}
			return m_popupGem;
		}
	}

	Transform m_popupQuest = null;

	public Transform popupQuest {
		get {
			if ( null == m_popupQuest ) {
				GameObject go = CreateObj ("Prefab/Popup/PopupQuest", tmPopup);
				m_popupQuest = go.transform;
			}
			return m_popupQuest;
		}
	}

	Transform m_popupPotion = null;

	public Transform popupPotion {
		get {
			if ( null == m_popupPotion ) {
				GameObject go = CreateObj ("Prefab/Popup/PopupPotion", tmPopup);
				m_popupPotion = go.transform;
			}
			return m_popupPotion;
		}
	}


	Transform m_popupGamePass = null;

	public Transform popupGamePass {
		get {
			if ( null == m_popupGamePass ) {
				GameObject go = CreateObj ("Prefab/Popup/PopupGamePass", tmPopup);
				m_popupGamePass = go.transform;
			}
			return m_popupGamePass;
		}
	}

	Transform m_popupGameFail = null;

	public Transform popupGameFail {
		get {
			if ( null == m_popupGameFail ) {
				GameObject go = CreateObj ("Prefab/Popup/PopupGameFail", tmPopup);
				m_popupGameFail = go.transform;
			}
			return m_popupGameFail;
		}
	}











	public UIScore uiScore {
		get {
			if (null == m_uiScore) {				
				GameObject go = FindOrCreateObj ("Prefab/UI/", "UIScore", GetHolder (tmAnchorTop, "UIScore"));
				m_uiScore = go.GetComponent<UIScore> ();
			}
			return m_uiScore;
		}
	}
		
	public UIStep uiStep {
		get {
			if (null == m_uiStep) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "UIStep", GetHolder (tmTopLeft, "UIStep"));
				m_uiStep = go.GetComponent<UIStep> ();
			}
			return m_uiStep;
		}
	}

	public RcUIStar uiStar {
		get {
			if (null == m_uiStar) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "UIStar", GetHolder (tmTopRight, "UIStar"));
				m_uiStar = go.GetComponent<RcUIStar> ();
			}
			return m_uiStar;
		}

	}


	PopupResult m_popupResult = null;

	public PopupResult popupResult {
		get {
			if (null == m_popupResult) {
				GameObject go = FindOrCreateObj ("Prefab/Popup/", "PopupResult", tmPopup);
				m_popupResult = go.GetComponent<PopupResult> ();
			}
			return m_popupResult;				
		}
	}




	// Top Left 
	UIGold m_uiGold = null;
	UIGem m_uiGem = null;
	UIKey m_uiKey = null;

	public UIGold uiGold {
		get {
			if (null == m_uiGold) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "UIGold", tmRoot);
				// GameObject go = CreateObj ("Prefab/UI/Gold", GetHolder (tmAnchorTopLeft, "HolderGold"));
				m_uiGold = go.GetComponent<UIGold> ();
			}
			return m_uiGold;
		}
	}

	public UIGem uiGem {
		get {
			if (null == m_uiGem) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "UIGem", tmRoot);
				//GameObject go = CreateObj ("Prefab/UI/Gem", GetHolder (tmAnchorTopLeft, "HolderGem"));
				m_uiGem = go.GetComponent<UIGem> ();
			}
			return m_uiGem;
		}
	}

	public UIKey uiKey {
		get {
			if (null == m_uiKey) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "UIKey", tmRoot);
				// GameObject go = CreateObj ("Prefab/UI/Key", GetHolder (tmAnchorTopLeft, "HolderKey"));
				m_uiKey = go.GetComponent<UIKey> ();
			}
			return m_uiKey;
		}
	}


	GameObject m_touchBeginPoint;
	GameObject m_touchDragPoint;

	public GameObject touchBeginPoint {
		get {
			if (null == m_touchBeginPoint) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "TouchBeginPoint", tmRoot);
				m_touchBeginPoint = go;
			}
			return m_touchBeginPoint;
		}
	}

	public GameObject touchDragPoint {
		get {
			if (null == m_touchDragPoint) {
				GameObject go = FindOrCreateObj ("Prefab/UI/", "TouchDragPoint", tmRoot);
				m_touchDragPoint = go;
			}
			return m_touchDragPoint;
		}
	}


	GameObject m_forwardIcon = null;

	public GameObject forwardIcon {
		get {
			if (null == m_forwardIcon) {
				m_forwardIcon = FindOrCreateObj ("Prefab/UI/", "Forward", tmBottom);

				Transform tm = m_forwardIcon.transform;
				tm.localPosition = Vector3.zero;
				tm.localScale = Vector3.one;
			}
			return m_forwardIcon;
		}
	}
}