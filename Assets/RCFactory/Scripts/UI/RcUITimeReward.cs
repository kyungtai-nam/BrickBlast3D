using UnityEngine;
using System.Collections;

// 시간에 따른 보상
public class RcUITimeReward : RcUI
{
	public UILabel lbTimer;
	public UIButton btn;

	RcRemainTime rtTime = new RcRemainTime();

	public int Minutes = 3;
	public int Seconds = 30;

	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Left);
		moveDelay = 0.0f;

		if ( !rtTime.Attach(RcSaveData.Inst.TimeReward) )
		{
			ResetTimer();
		}

		// Debug.Log("RcUITimeReward:Awake() " + rtTime.ToString() + " " + rtTime.ToFormatString(false, true, true));
	}

	void FixedUpdate()
	{
		btn.isEnabled = rtTime.IsExpired();

		if ( !rtTime.IsExpired() )
			lbTimer.text = rtTime.ToFormatString(false, true, true);
		else
			lbTimer.text = "FREE";
	}

	void ResetTimer()
	{
		rtTime.Reset(0, Minutes, Seconds);
		RcSaveData.Inst.TimeReward = rtTime.ToString ();
		RcSaveData.Inst.SaveTimeReward ();		
	}


	public override void OnTouched()
	{
		Debug.Log ("RcUITimeReward:OnTouched()");

		ResetTimer();
		Reward();
		
		base.OnTouched ();
	}

	void Reward()
	{
		int cnt = Random.Range(10, 51);
		RcSaveData.Inst.gem += cnt;

		UIManager.Inst.PopupMessage("REWARD GEM +" + cnt, 2.0f);
	}
}