using UnityEngine;
using System.Collections;
using IgaworksUnityAOS;

public class TestScene : MonoBehaviour {
	
	Rect rectangle_1 = new Rect(10,10,100,100);
	Rect rectangle_2 = new Rect(120,10,100,100);
	Rect rectangle_3 = new Rect(10,120,100,100);
	Rect rectangle_4 = new Rect(120,120,100,100);
	Rect rectangle_5 = new Rect(10,240,100,100);
	Rect rectangle_6 = new Rect(120,240,100,100);
	Rect rectangle_7 = new Rect(10,360,100,100);
	Rect rectangle_8 = new Rect(120,360,100,100);
	Rect rectangle_9 = new Rect(10,480,100,100);
	Rect rectangle_10 = new Rect(120,480,100,100);
	Rect rectangle_11 = new Rect(10,600,100,100);
	Rect rectangle_12 = new Rect(120,600,100,100);
	
	bool pushSwitch = false;
	
	Vector2 scrollPosition = Vector2.zero;
	
	// Use this for initialization
	
	void Awake(){
		Debug.Log ("AWAKE - AWAKE - AWAKE");
		
		IgaworksUnityPluginAOS.InitPlugin();
	}
	
	
	
	void Start () {
		Debug.Log ("START - START - START");
		
		IgaworksUnityPluginAOS.Common.startSession ();
		IgaworksUnityPluginAOS.Common.setUserId ("testusn12345");
		IgaworksUnityPluginAOS.Adbrix.setAge (11);
		IgaworksUnityPluginAOS.Adbrix.setGender (IgaworksUnityPluginAOS.Gender.FEMALE);
		// 오퍼월 이벤트 리스너 등록
		IgaworksUnityPluginAOS.Common.setClientRewardEventListener ();
		IgaworksUnityPluginAOS.Adpopcorn.setAdpopcornOfferwallEventListener ();
		IgaworksUnityPluginAOS.LiveOps.initialize ();
		//IgaworksUnityPluginAOS.LiveOps.setLiveOpsNotificationCallback ();
		
		Debug.Log ("#################     setTargetingData(\"unity_group_1\", \"unity_user_1\")     ###################\n" );
		//IgaworksUnityPluginAOS.LiveOps.setTargetingData("unity_group_1", "unity_user_1");

		IgaworksUnityPluginAOS.OnGetRewardInfo = mOnGetRewardInfo;
		IgaworksUnityPluginAOS.OnDidGiveRewardItemRequestResult = mOnDidGiveRewardItemRequestResult;   

		setDelegate();
	}
	
	
	void setDelegate() {
		
		// 오퍼월 이벤트 델리게이트 등록
		IgaworksUnityPluginAOS.OnClosedOfferwallPage = mOnClosedOfferwallPage;

		IgaworksUnityPluginAOS.OnPlayBtnClickListener = mOnPlayBtnClickListener;
		IgaworksUnityPluginAOS.OnOpenDialogListener = mOnOpenDialogListener;
		IgaworksUnityPluginAOS.OnNoADAvailableListener = mOnNoADAvailableListener;
		IgaworksUnityPluginAOS.OnHideDialogListener = mOnHideDialogListener;
		
		IgaworksUnityPluginAOS.OnSendCouponSucceed = mOnSendCouponSucceed;
		IgaworksUnityPluginAOS.OnSendCouponFailed = mOnSendCouponFailed;
		
		//IgaworksUnityPluginAOS.OnLiveOpsNotification = mOnLiveOpsNotification;
		
		IgaworksUnityPluginAOS.OnOpenNanooFanPage = mOnOpenNanooFanPage;

		IgaworksUnityPluginAOS.OnLoadVideoAdFailure = mOnLoadVideoAdFailure;
		IgaworksUnityPluginAOS.OnLoadVideoAdSuccess = OnLoadVideoAdSuccess;
		IgaworksUnityPluginAOS.OnShowVideoAdFailure = OnShowVideoAdFailure;
		IgaworksUnityPluginAOS.OnShowVideoAdSuccess = OnShowVideoAdSuccess;
		IgaworksUnityPluginAOS.OnVideoAdClose = OnVideoAdClose;
	}
	
	void mOnClosedOfferwallPage ()
	{
		// 오퍼월 종료 이벤트 처리
		
		Debug.Log ("#################     오퍼월 종료     ###################\n" );
		IgaworksUnityPluginAOS.Common.getClientPendingRewardItems();
	}

	void mOnGetRewardInfo(string campaignkey, string campaignname, string quantity, string cv, string rewardkey){    
		string ck = campaignkey;
		string cn = campaignname;
		string qt = quantity;
		// 위 정보를 이용하여 유저에게 리워드를 지급합니다.
		// {리워드 지급 처리}
		
		// didGiveRewardItem API를 호출하여 리워드 지급 처리 완료를 IGAW리워드 서버에 통지합니다.
		Debug.Log ("################# mOnGetRewardInfo   campaignkey    ###################" + campaignkey);
		Debug.Log ("################# mOnGetRewardInfo   rewardkey    ###################" + rewardkey);
		IgaworksUnityPluginAOS.Common.didGiveRewardItem (cv, rewardkey);
	}
	void mOnDidGiveRewardItemRequestResult (bool isSuccess, string rewardkey) {
		// didGiveRewardItem 함수의 처리 결과가 리턴됩니다.
		// 동일한 rewardkey에 대해서 중복지급방지 처리를 합니다
		Debug.Log ("################# mOnDidGiveRewardItemRequestResult   rewardkey    ###################" + rewardkey);
	}
	
	
	void mOnPlayBtnClickListener ()
	{
		Debug.Log ("#################     Promotion.hideAD     ###################\n" );
		IgaworksUnityPluginAOS.Promotion.hideAD ();
	}
	
	void mOnOpenDialogListener ()
	{
		Debug.Log ("#################     OnOpenDialogListener     ###################\n" );
	}
	
	void mOnNoADAvailableListener ()
	{
		Debug.Log ("#################     OnNoADAvailableListener     ###################\n" );
	}
	
	void mOnHideDialogListener ()
	{
		Debug.Log ("#################     OnHideDialogListener     ###################\n" );
	}
	
	void mOnSendCouponSucceed (string msg, int itemKey, string itemName, long quantity)
	{
		Debug.Log (string.Format("#################     mOnSendCouponSucceed : {0} - {1} - {2} - {3}     ###################\n", msg, itemKey, itemName, quantity) );
	}
	
	void mOnSendCouponFailed (string msg)
	{
		Debug.Log (string.Format("#################     mOnSendCouponFailed : {0}     ###################\n", msg) );
	}
	
	/*void mOnLiveOpsNotification (string data)
	{
		Debug.Log (string.Format("#################     mOnLiveOpsNotification : {0}     ###################\n", data) );
	}*/
	
	void mOnOpenNanooFanPage (string url)
	{
		Debug.Log (string.Format("#################     mOnOpenNanooFanPage : {0}     ###################\n", url) );
	}

	void mOnLoadVideoAdFailure (string apErrorMessage)
	{
		Debug.Log (string.Format("#################     mOnLoadVideoAdFailure : {0}     ###################\n", apErrorMessage) );
	}

	void OnLoadVideoAdSuccess ()
	{
		Debug.Log (string.Format("#################     OnLoadVideoAdSuccess ###################\n") );
	}

	void OnShowVideoAdFailure (string apErrorMessage)
	{
		Debug.Log (string.Format("#################     OnShowVideoAdFailure : {0}     ###################\n", apErrorMessage) );
	}

	void OnShowVideoAdSuccess ()
	{
		Debug.Log (string.Format("#################     OnShowVideoAdSuccess  ###################\n") );
	}

	void OnVideoAdClose ()
	{
		Debug.Log (string.Format("#################     OnVideoAdClose  ###################\n") );
	}
	
	void OnGUI(){		
		
		//scrollPosition = GUI.BeginScrollView (new Rect (0,0,640,2000),
		//                                      scrollPosition, new Rect (0, 0, 220, 200));
		
		if (GUI.Button (rectangle_1, "FTE Default")) {
			IgaworksUnityPluginAOS.Adbrix.firstTimeExperience("testFTE");
		}
		if (GUI.Button (rectangle_2, "RET Default")) {
			IgaworksUnityPluginAOS.Adbrix.retention("testRET");
		}
		if (GUI.Button (rectangle_3, "RET with Param")) {
			IgaworksUnityPluginAOS.Adbrix.retention("testRET","testFTE_param");
		}
		if (GUI.Button (rectangle_4, "BUY Default")) {
			IgaworksUnityPluginAOS.Adbrix.buy("testBUY");
		}
		if (GUI.Button (rectangle_5, "OpenOfferwall")) {
			IgaworksUnityPluginAOS.Adpopcorn.loadVideoAd ();
			IgaworksUnityPluginAOS.Adpopcorn.openOfferwall();
		}
		if (GUI.Button (rectangle_6, "ShowAD")) {
			//IgaworksUnityPluginAOS.Promotion.showAD("game_end");
			IgaworksUnityPluginAOS.Adpopcorn.showVideoAd ();
		}
		if (GUI.Button (rectangle_7, "CouponDialog")) {
			IgaworksUnityPluginAOS.Coupon.showCouponDialog(true);
			//Application.LoadLevel(1);
		}
		if (GUI.Button (rectangle_8, "CheckCoupon")) {
			//IgaworksUnityPluginAOS.Coupon.checkCoupon("EVENPBSPM6X6");
			//Application.LoadLevel(1);
		}
		if (GUI.Button (rectangle_9, "NormalClientPushEvent")) {
			//IgaworksUnityPluginAOS.Coupon.checkCoupon("EVENPBSPM6X6");
			//IgaworksUnityPluginAOS.Nanoo.openNanooFanPage(true);
			IgaworksUnityPluginAOS.LiveOps.setNormalClientPushEvent(1, "Unity Normal Push", 1, true);
			IgaworksUnityPluginAOS.LiveOps.setTargetingData("int", 10);
		}
		if (GUI.Button (rectangle_10, "BigTextClientPush")) {
			//IgaworksUnityPluginAOS.Coupon.checkCoupon("EVENPBSPM6X6");
			IgaworksUnityPluginAOS.LiveOps.setBigTextClientPushEvent(1, "Unity Big Push", "Big title", "Bit text", "summary text", 1, true);
		}
		if (GUI.Button (rectangle_11, "enableService")) {
			//IgaworksUnityPluginAOS.Coupon.checkCoupon("EVENPBSPM6X6");
			pushSwitch = !pushSwitch;
			IgaworksUnityPluginAOS.LiveOps.enableService(pushSwitch);
			Debug.Log ("enableService : " + pushSwitch);
		}
		
		if (GUI.Button (rectangle_12, "setCustomCohort")) {
			//IgaworksUnityPluginAOS.Coupon.checkCoupon("EVENPBSPM6X6");
			IgaworksUnityPluginAOS.Adbrix.setCustomCohort(IgaworksUnityPluginAOS.CohortVariable.COHORT_1, "address_1");
		}
		
		//GUI.EndScrollView ();
	}
	
	void OnApplicationPause(bool pauseStatus){
		
		if (pauseStatus) {
			Debug.Log ("go to Background");
			IgaworksUnityPluginAOS.Common.endSession();
		} else {
			Debug.Log ("go to Foreground");
			IgaworksUnityPluginAOS.Common.startSession();
		}
	}
	
	
	// Update is called once per frame
	void Update (){
		
	}
}