using UnityEngine;
using System.Collections;
using VoxelBusters.NativePlugins;

// 광고 제거
public class RcUIRemoveAd : RcUI
{
	public System.Action cbBillingDone = null;

	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Right);
		moveDelay = 0.0f;	
	}

	void cbBillingInit(bool result)
	{
		bool hasProduct = RcBilling.Inst.HasProduct ("remove_banner");
		Debug.Log ("RcUIRemoveAd:cbBillingInit() remove_banner has=" + hasProduct);
	}

	public override void OnTouched()
	{
		Debug.Log ("RcUIRemoveAd:OnTouched()");
		// DebugPanel.Log("RcUIRemoveAd:OnTouched()", true);

		// RcBilling.Inst.RestorePurchases (cbBuy);	
		RcBilling.Inst.Buy("remove_banner", cbBuy);
	}



	void ProcessNoAds()
	{
		RcSaveData.Inst.showAd = false;
		RcSaveData.Inst.SaveAd(true);

		// 배너 끄기!
		RcAdManager.Inst.CloseBanner();

		// 구입하고 초기화 안하면 구입 목록에 안나옴!
		RcBilling.Inst.Init(cbBillingInit);

		Hide();
	}


	void cbBuy(RcBilling.E_BILLING_RESULT eResult, BillingTransaction billing)
	{
		Debug.Log("RcUIRemoveAd:cbBuy=" + eResult);

		switch(eResult)
		{
		case RcBilling.E_BILLING_RESULT.Cancel:
			// 취소.
			break;

		case RcBilling.E_BILLING_RESULT.Already:			
			ProcessNoAds ();
			break;

		case RcBilling.E_BILLING_RESULT.Success:	
			RcBI.Inst.Buy ("remove_banner");
			UIManager.Inst.PopupMessage (Localization.Get("remove_ad"), 2.0f);

			ProcessNoAds ();
			break;

		case RcBilling.E_BILLING_RESULT.Failed:
			// 실패.
			break;
		}

		base.OnTouched ();
	}
}