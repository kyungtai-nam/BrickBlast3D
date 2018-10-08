using UnityEngine;
using System.Collections;


// 스코어
public class RcUILeaderBoard : RcUI
{
	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Right);
		moveDelay = 0.2f;
	}

	public override void OnTouched()
	{
		// Debug.Log ("RcUILeaderBoard:OnTouched()");

		// 점수 저장
		// RcGameService.Inst.SaveScore(1, cbSaveScore);
		if ( !RcGameService.Inst.available ) 
		{
			//UIManager.Inst.PopupMessage ("Network is NOT available", 1.5f);
			UIManager.Inst.PopupMessage (Localization.Get("network"), 1.5f);
			return;
		}

		if (!RcGameService.Inst.authenticated) {
			RcGameService.Inst.Login (cbSign, 10f);
			return;
		}

		RcGameService.Inst.ShowLeaderBoardUI(cbLeaderBoardClose);

		base.OnTouched ();
	}

	void cbSign(bool success, string error)
	{
		UIManager.Inst.PopupMessage (Localization.Get(success? "sign_success" : "sign_failed") , 1.5f);		

		if (!success)
			return;
		
		RcGameService.Inst.ShowLeaderBoardUI(cbLeaderBoardClose);
	}

	
	void cbLeaderBoardClose(string error)
	{
		// Alert ("LBClose=" + error);
	}


	void cbSaveScore(bool success, string error)
	{
		// Alert (string.Format ("SaveScore={0} err={1}", success, error));
	}


}