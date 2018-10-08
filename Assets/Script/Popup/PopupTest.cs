using UnityEngine;
using System.Collections;
using VoxelBusters.NativePlugins;

public class PopupTest : MonoBehaviour {

	public void OnNoAds()
	{
		UIManager.Inst.PopupMessage ("NoAds", 3f);

		RcBilling.Inst.Buy("remove_banner", cbBuy);
	}


	void ProcessNoAds()
	{
		RcSaveData.Inst.showAd = false;
		RcSaveData.Inst.SaveAd();

		// 배너 끄기!
		RcAdManager.Inst.CloseBanner();

		UIManager.Inst.PopupMessage("REMOVE BANNER", 2.0f);

		// 구입하고 초기화 안하면 구입 목록에 안나옴!
		RcBilling.Inst.Init(null);
	}

	void cbBuy(RcBilling.E_BILLING_RESULT eResult, BillingTransaction billing)
	{
		Debug.Log("RcUIRemoveAd:cbBuy=" + eResult);

		switch(eResult)
		{
		case RcBilling.E_BILLING_RESULT.Cancel:
			// 취소.
			UIManager.Inst.PopupMessage ("noads Cancel", 3f);
			break;

		case RcBilling.E_BILLING_RESULT.Success:
			UIManager.Inst.PopupMessage ("noads success", 3f);
			// 성공.
			RcBI.Inst.Buy ("remove_banner");
			ProcessNoAds ();
			break;

		case RcBilling.E_BILLING_RESULT.Already:
			UIManager.Inst.PopupMessage ("noads already", 3f);
			ProcessNoAds ();
			break;

		case RcBilling.E_BILLING_RESULT.Failed:
			// 실패.
			UIManager.Inst.PopupMessage ("noads failed", 3f);
			break;
		}
	}

	public void OnFullBanner()
	{
		UIManager.Inst.PopupMessage ("OnFullBanner", 3f);
		RcAdManager.Inst.ShowFullBanner ();
	}

	public void OnRewardVideo()
	{
		UIManager.Inst.PopupMessage ("OnRewardVideo", 3f);

		RcAdManager.Inst.ShowVideo(cbVideo);

	}

	void cbVideo(bool isSuccess)
	{
		// Debug.Log ("RcUIAd:cbVideo() isSucces=" + isSuccess);
		string msg = "cbVideo=" + isSuccess;
		UIManager.Inst.PopupMessage (msg, 3f);
	}


	public void OnSaveScore()
	{
		UIManager.Inst.PopupMessage ("OnSaveScore", 3f);

		RcGameService.Inst.SaveScore (1, null);
	}

	public void OnLeaderBoard()
	{
		UIManager.Inst.PopupMessage ("OnLeaderBoard", 3f);

		RcGameService.Inst.ShowLeaderBoardUI (null);
	}

	public void OnAchievement()
	{
		UIManager.Inst.PopupMessage ("OnAchievement", 3f);
		RcGameService.Inst.ShowAchievement();
	}

}
