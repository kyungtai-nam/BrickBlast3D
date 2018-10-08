using UnityEngine;
using System.Collections;

public class RcAdManager : GSingletonMono<RcAdManager>
{
	RcAdmob admob = null;
	int remainFullAd = 0; // 횟수가 0이 되면 광고 출력


	public void ResetRemainFullBanner()
	{
		remainFullAd = Random.Range (1, 3);
	}

	public bool IsShowFullBanner()
	{
		if (!RcSaveData.Inst.showAd)
			return false;

		if (0 < --remainFullAd)
			return false;

		ResetRemainFullBanner ();
		return true;
	}

	public void Init()
	{
		Debug.Log ("RcAdManager:Init");

		ResetRemainFullBanner ();

		admob = new RcAdmob();
		admob.Init();
	}


	public void ShowBanner()
	{
		Debug.Log ("RcAdManager:ShowBanner()");
		admob.ShowBanner();
	}

	public void CloseBanner()
	{
		Debug.Log ("RcAdManager:CloseBanner()");
		admob.CloseBanner();
	}

	public void ShowFullBanner()
	{
		Debug.Log ("RcAdManager:ShowFullBanner()");
		admob.ShowFullBanner();
	}

	public void ShowVideo(System.Action<bool> cbVideo)
	{
		#if UNITY_EDITOR
			cbVideo(true);
			return;
		#endif

		Debug.Log ("RcAdManager:ShowVideo()");
		
		admob.ShowVideo(cbVideo);
		//RcBI.Inst.Tracking("Ad_NoMoreVideo");
	}

	public bool IsReadyVideo()
	{
		#if UNITY_EDITOR
			return true;
		#endif

		return admob.IsReadyVideo ();
	}

}
