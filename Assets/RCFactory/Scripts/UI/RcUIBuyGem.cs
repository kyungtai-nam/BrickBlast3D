using UnityEngine;
using System.Collections;
using VoxelBusters.NativePlugins;

// 캐쉬 구입
public class RcUIBuyGem : RcUI
{
	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Right);
		moveDelay = 0.0f;	

		// TODO : 미리 전역으로 해놔야 한다!
		//RcBilling.Inst.Init(cbBillingInit);
	}

	void cbBillingInit(bool result)
	{
		Debug.Log ("RcUIBuyGem:cbBillingInit=" + result);
		// UIManager.Inst.Alert("cbBillingInit=" + result);
	}


	public override void OnTouched()
	{
		Debug.Log ("RcUIBuyGem:OnTouched()");

		RcBilling.Inst.Buy("gem_500", cbBuy);
	}

	void cbBuy(RcBilling.E_BILLING_RESULT eResult, BillingTransaction billing)
	{
		Debug.Log("RCUIBuyGem:cbBuy()" + eResult);
		// UIManager.Inst.Alert("cbBuy=" + eResult);
		// DebugPanel.Log("cbBuy=", eResult);

		switch(eResult)
		{
		case RcBilling.E_BILLING_RESULT.Cancel:
			// 취소.
			break;

		case RcBilling.E_BILLING_RESULT.Success:
			
			// 성공. 캐시 증가
			RcSaveData.Inst.gem += 500;
			RcSaveData.Inst.SaveGem ();

			UIManager.Inst.PopupMessage ("GEM +500", 2.0f);

			RcBI.Inst.Buy ("gem_500");
					
			// 구입하고 초기화 안하면 구입 목록에 안나옴!
			RcBilling.Inst.Init(cbBillingInit);
			break;

		case RcBilling.E_BILLING_RESULT.Failed:
			// 실패.
			break;
		}

		base.OnTouched ();
	}
}

