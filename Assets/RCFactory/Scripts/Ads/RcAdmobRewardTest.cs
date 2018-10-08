using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GoogleMobileAds;
using GoogleMobileAds.Api; // 구글 애드몹 API 네임 스페이스
using System;

public class RcAdmobRewardTest : MonoBehaviour
{
	public enum TestAd
	{
		AdMobService,
		AdColony,
		AdColonyV4VC,
		Vungle,
		UnityAds,
		Empty
	}

	public TestAd testCase = TestAd.Empty;

	bool init = false;

	void Awake()
	{
		switch (testCase) {

		case TestAd.AdColony:
			ADMOB_REWARD_VIDEO = Global.ADMOB_REWARD_VIDEO_ADCOLONY;
			break;

		case TestAd.AdColonyV4VC:
			ADMOB_REWARD_VIDEO = Global.ADMOB_REWARD_VIDEO_ADCOLONY_V4VC;
			break;

		case TestAd.AdMobService:
			ADMOB_REWARD_VIDEO = Global.ADMOB_REWARD_VIDEO;
			break;

		case TestAd.UnityAds:
			ADMOB_REWARD_VIDEO = Global.ADMOB_REWARD_VIDEO_UNITY_ADS;
			break;

		case TestAd.Vungle:
			ADMOB_REWARD_VIDEO = Global.ADMOB_REWARD_VIDEO_VUNGLE;
			break;
		}
	}

	public string ADMOB_REWARD_VIDEO;

	// 보상형 비디오
	RewardBasedVideoAd rewardBasedVideo;

	void Start () {
		Debug.Log ("RcAdmobRewardTest:Start()");
		Init();
	}

	bool Init()
	{
		InitVideo();
		return true;
	}
		
	void InitVideo()
	{
		// Get singleton reward based video ad reference.
		rewardBasedVideo = RewardBasedVideoAd.Instance;

		// RewardBasedVideoAd is a singleton, so handlers should only be registered once.
		rewardBasedVideo.OnAdLoaded += cbVideoLoaded;
		rewardBasedVideo.OnAdFailedToLoad += cbVideoFailedToLoad;
		rewardBasedVideo.OnAdOpening += cbVideoOpened;
		rewardBasedVideo.OnAdStarted += cbVideoStarted;
		rewardBasedVideo.OnAdRewarded += cbVideoRewarded;
		rewardBasedVideo.OnAdClosed += cbVideoClosed;
		rewardBasedVideo.OnAdLeavingApplication += cbVideoLeftApplication;
	}
		

	void RequestRewardBasedVideo()
	{
		Debug.Log ("RcAdmobRewardTest:RequestRewardBasedVideo() id=" + ADMOB_REWARD_VIDEO);
		rewardBasedVideo.LoadAd(createAdRequest(), ADMOB_REWARD_VIDEO);
	}
		
	private AdRequest createAdRequest()
	{
		return new AdRequest.Builder()
			.AddTestDevice(AdRequest.TestDeviceSimulator)            
			.Build();
	}


	void ShowRewardBasedVideo()
	{
		if (rewardBasedVideo.IsLoaded())
		{
			rewardBasedVideo.Show();
		}
		else
		{
			RequestRewardBasedVideo ();
			Debug.Log("Reward based video ad is not ready yet");			
		}
	}




	public void ShowAd()
	{
		if (init) {
			Debug.Log ("RcAdmobRewardTest:ShowAd()=" + testCase);
			ShowRewardBasedVideo ();
			return;
		}

		init = true;
		RequestRewardBasedVideo();
	}




	public void cbVideoLoaded(object sender, EventArgs args)
	{
		Debug.Log("cbVideoLoaded event received"); 
	}

	public void cbVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		Debug.Log("cbVideoFailedToLoad event received with message: " + args.Message);
	}

	public void cbVideoOpened(object sender, EventArgs args)
	{
		Debug.Log("cbVideoOpened="+args.ToString ());
	}

	public void cbVideoStarted(object sender, EventArgs args)
	{
		Debug.Log("cbVideoStarted="+args.ToString ());
	}

	public void cbVideoClosed(object sender, EventArgs args)
	{
		Debug.Log("cbVideoClosed="+args.ToString ());

		RequestRewardBasedVideo();
	}

	public void cbVideoRewarded(object sender, Reward args)
	{
		string type = args.Type;
		double amount = args.Amount;
		Debug.Log("cbVideoRewarded" + amount.ToString() + " " + type);
	}

	public void cbVideoLeftApplication(object sender, EventArgs args)
	{
		Debug.Log("cbVideoLeftApplication=" + args.ToString());
	}
}