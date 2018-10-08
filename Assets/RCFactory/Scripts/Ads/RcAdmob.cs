using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GoogleMobileAds;
using GoogleMobileAds.Api; // 구글 애드몹 API 네임 스페이스
using System;

public class RcAdmob : RcAd 
{
	// 하단 베너
	bool showAdMobBanner = false;
	BannerView bannerView = null; // 배너 출력

	// 비상형 비디오
	System.Action<bool> onVideo = null;

	// 보상형 비디오
	RewardBasedVideoAd rewardBasedVideo;


    public override bool Init()
    {
        RequestBanner();
        RequestFullBanner();

        InitVideo();
        RequestRewardBasedVideo();
        return true;
    }

    public override bool ShowBanner()
    {  
        bannerView.Show();  // 배너 광고 출력  
        return true;
    }

    public override void CloseBanner()
    {
        bannerView.Hide();
        /*
        if ( null != bannerView )
        {
            bannerView.Hide();
            bannerView.Destroy();            
            bannerView = null;
        }

        showAdMobBanner = false;
        */
    }

    public override bool ShowFullBanner()
    {
        ShowInterstitial();
        return true;
    }

    public override bool ShowVideo(System.Action<bool> cbVideo)
    {
        this.cbVideo = cbVideo;
        ShowRewardBasedVideo();
        return true;
    }


	public override bool IsReadyVideo()
	{
		return rewardBasedVideo.IsLoaded ();
	}

    

   
    void RequestBanner()
    {
        // BannerView(애드몹 사이트에 등록된 아이디, 크기, 위치) / AdSize.SmartBanner : 화면 해상도에 맞게 늘임, AdPosition.Bottom : 화면 바닥에 붙음
        bannerView = new BannerView(Global.ADMOB_BANNER, AdSize.SmartBanner, AdPosition.Bottom);        
        // Called when an ad request has successfully loaded.
        bannerView.OnAdLoaded += HandleAdLoaded;
        // Called when an ad request failed to load.
        bannerView.OnAdFailedToLoad += HandleAdFailedToLoad;
        // Called when an ad is clicked.
        bannerView.OnAdOpening += HandleAdOpening;
        // Called when the user returned from the app after an ad click.
        bannerView.OnAdClosed += HandleAdClosed;
        // Called when the ad click caused the user to leave the application.
        bannerView.OnAdLeavingApplication += HandleAdLeftApplication;

        bannerView.LoadAd(createAdRequest()); //배너 광고 요청
    }
    

    // Returns an ad request with custom ad targeting.
    private AdRequest createAdRequest()
    {
        return new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)            
            //.AddTestDevice(Global.ADMOB_TEST_DEVICE_ID_GPRO)
            //.AddTestDevice(Global.ADMOB_TEST_DEVICE_ID_NEXUS)
            .Build();
    }


	InterstitialAd fullBanner = null;
    
    void RequestFullBanner()
    {
		if (null != fullBanner)
			return;
			

        // Create an interstitial.
        fullBanner = new InterstitialAd(Global.ADMOB_FULL_BANNER);
        // Register for ad events.
        fullBanner.OnAdLoaded += HandleInterstitialLoaded;
        fullBanner.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
        fullBanner.OnAdOpening += HandleInterstitialOpened;
        fullBanner.OnAdClosed += HandleInterstitialClosed;
        fullBanner.OnAdLeavingApplication += HandleInterstitialLeftApplication;
        // Load an interstitial ad.
        fullBanner.LoadAd(createAdRequest());
    }

    bool ShowInterstitial()
    {
		if (null == fullBanner)
			return false;

        if (fullBanner.IsLoaded())
        {
            RcBI.Inst.Tracking("Ad_AdmobFullBanner_Success");
            fullBanner.Show();
            return true;
        }
        else
        {
            RcBI.Inst.Tracking("Ad_AdmobFullBanner_Failed");
            Debug.Log("RcAdmob:ShowInterstitial() not ready yet");
            // print("Interstitial is not ready yet.");
            return false;
        }
    }


#region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("RcAdmob:HandleAdLoaded() event received.");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("RcAdmob:HandleAdFailedToLoad() event received.");
        showAdMobBanner = false;
    } 
    
    public void HandleAdOpening(object sender, EventArgs args)
    {
        Debug.Log("RcAdmob:HandleAdOpening() event received.");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        Debug.Log("RcAdmob:HandleAdClosed() event received.");

        bannerView.LoadAd(createAdRequest()); //배너 광고 요청
        // bannerView.Destroy();
        //RequestBanner();

        // showAdMobBanner = false;
    } 

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        Debug.Log("RcAdmob:HandleAdLeftApplication() event received.");
    } 

#endregion


#region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialLoaded event received.");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleInterstitialFailedToLoad event received with message: " + args.Message);
		/*
		if (null == fullBanner) {
			return;
		}

        fullBanner.Destroy();
		fullBanner = null;
        RequestFullBanner();

        if ( null != onVideo )
            onVideo(false);
        */
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialOpened event received");
    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialClosing event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialClosed event received");

        fullBanner.Destroy();
        RequestFullBanner();

        if ( null != onVideo )
            onVideo(true);
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialLeftApplication event received");
        /*
        돌아오면 다시 Closed 이벤트가 들어오게 되니 별도로 필요 없음

        interstitial.Destroy();
        RequestAdMobFull();

        if ( null != onVideo )
            onVideo(true);
        */
    }

#endregion





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
		
        #if UNITY_EDITOR
        string adUnitId = "unused";
        #elif UNITY_ANDROID
		string adUnitId = Global.ADMOB_REWARD_VIDEO;
        #elif UNITY_IPHONE
		string adUnitId = Global.ADMOB_REWARD_VIDEO;
        #else
        string adUnitId = "unexpected_platform";
        #endif

        rewardBasedVideo.LoadAd(createAdRequest(), adUnitId);
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
			// UIManager.Inst.PopupMessage("Reward based video ad is not ready yet");
			UIManager.Inst.PopupMessage(Localization.Get("ad_ready"));
        }
    }



    #region RewardBasedVideo callback handlers

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
		Debug.Log("cbVideoOpened event received");
    }

    public void cbVideoStarted(object sender, EventArgs args)
    {
		Debug.Log("cbVideoStarted event received");
    }

    public void cbVideoClosed(object sender, EventArgs args)
    {
		Debug.Log("cbVideoClosed event received");

        RequestRewardBasedVideo();

		if ( null != cbVideo )
			cbVideo(false);
    }

    public void cbVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("cbVideoRewarded event received for " + amount.ToString() + " " + type);

		RcBI.Inst.Tracking (string.Format ("Ad_RewardVideo_{0}", type));

		if ( null != cbVideo )
			cbVideo(true);
    }

    public void cbVideoLeftApplication(object sender, EventArgs args)
    {
		Debug.Log("cbVideoLeftApplication event received");
    }

    #endregion


}
