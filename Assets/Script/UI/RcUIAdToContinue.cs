using UnityEngine;
using System.Collections;

// 광고 보면 보상 주기 
public class RcUIAdToContinue : RcUI
{
	public UIButton btn;
	public TweenScale twScale;
	public UILabel lbDesc;

	protected override void Awake()
	{		
		base.Awake ();

		//SetMoveDirection(EMoveDir.Right);
		movePos = Global.Inst.UI_MOVE_BOTTOM_FAR; 
		moveDelay = 0.1f;

		/*
		if ( SystemLanguage.Korean == Application.systemLanguage ) 
			lbDesc.text = "watch a video\nto continue";	// 광고보고 이어하기
		else
			lbDesc.text = "watch a video\nto continue";	
		*/
	}

	void OnEnable()
	{
		twScale.ResetToBeginning ();
		twScale.PlayForward ();
	}

	public override void OnTouched()
	{
		Debug.Log ("RcUIAd:OnTouched()");

		showAd = true;
		needToReward = false;
		RcAdManager.Inst.ShowVideo(cbVideo);

		#if UNITY_EDITOR
		needToReward = true;
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
			//UIManager.Inst.PopupMessage ("CANCEL", 2.0f);
			UIManager.Inst.PopupMessage (Localization.Get("cancel"), 2.0f);
			return;
		}
		Debug.Log ("RcUIAd:Reward() reward");

		needToReward = false;

		//UIManager.Inst.PopupMessage ("READY TO PLAY", 2.0f);
		UIManager.Inst.PopupMessage (Localization.Get("ready"), 2.0f);

		base.OnTouched ();
		Hide();
	}
}