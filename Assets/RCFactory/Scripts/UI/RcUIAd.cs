using UnityEngine;
using System.Collections;

// 광고 보면 보상 주기 
public class RcUIAd : RcUI
{
	public UIButton btn;
	public TweenScale twScale;
	public UILabel lbDesc;
	public UILabel lbCount;

	int rewardCount = 0;

	protected override void Awake()
	{		
		base.Awake ();

		//SetMoveDirection(EMoveDir.Right);
		movePos = Global.Inst.UI_MOVE_BOTTOM_FAR; 
		moveDelay = 0.1f;

		if ( SystemLanguage.Korean == Application.systemLanguage ) 
			lbDesc.text = "광고보상";
		else
			lbDesc.text = "EARN TO ";	
	}

	void OnEnable()
	{
		/*
		if (Global.CHEST_PRICE > RcSaveData.Inst.gem) {
			rewardCount = Mathf.Abs (Global.CHEST_PRICE - RcSaveData.Inst.gem) + Random.Range (1, 10);
		} else {
			rewardCount = Random.Range (Global.CHEST_PRICE/2, Global.CHEST_PRICE);
		}*/

		rewardCount = Global.AD_REWARD_GEM;

		lbCount.text = rewardCount.ToString();

		twScale.ResetToBeginning ();
		twScale.PlayForward ();
	}

	public override void OnTouched()
	{
		Debug.Log ("RcUIAd:OnTouched()");

		// TODO : disable touch and hide sprite

		showAd = true;
		needToReward = false;
		RcAdManager.Inst.ShowVideo(cbVideo);

		#if UNITY_EDITOR
		Invoke("EditorTest", 0.5f);
		#endif
	}

	void EditorTest()
	{
		OnApplicationPause (false);
	}

	void OnApplicationPause( bool pauseStatus )
	{
		Debug.Log ("RcUIAD:OnApplicationPause() status=" + pauseStatus + " showAd=" + showAd);

		if (pauseStatus)
			return;

		if (!showAd)
			return;

		Invoke ("Reward", 0.1f);	
	}

	bool showAd = false;
	bool needToReward = false;

	void cbVideo(bool isSuccess)
	{
		Debug.Log ("RcUIAd:cbVideo() isSucces=" + isSuccess);
		needToReward = isShow;
	}

	void Reward()
	{
		if ( !needToReward ) 
		{
			Debug.Log ("RcUIAd:Reward() cancel");
			UIManager.Inst.PopupMessage ("CANCEL GEM +0", 3.0f);
			return;
		}
		Debug.Log ("RcUIAd:Reward() reward");

		needToReward = false;

		UIManager.Inst.PopupMessage (string.Format("REWARD GEM +{0}", rewardCount), 3.0f);
		RcSaveData.Inst.gem += rewardCount;
		RcSaveData.Inst.SaveGem ();

		rewardCount = 0;

		Hide();
		base.OnTouched ();
	}
}