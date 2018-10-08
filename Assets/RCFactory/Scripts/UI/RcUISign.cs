using UnityEngine;
using System.Collections;


// 인증 
public class RcUISign : RcUI
{
	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Left);
		moveDelay = 0.1f;
	}

	public void Sign()
	{
		if ( !RcGameService.Inst.available ) 
		{
			//UIManager.Inst.Alert("Network is NOT available");
			UIManager.Inst.PopupMessage (Localization.Get("network"), 1.5f);
			return;
		}

		if (RcGameService.Inst.authenticated)
			return;

		RcGameService.Inst.Login (cbSign, 10f);
	}

	public override void OnTouched()
	{
		Debug.Log ("RcUISign:OnTouched()");


		// NPBinding.GameServices.LocalUser.Authenticate(onCallback);

		if ( !RcGameService.Inst.available ) 
		{
			// UIManager.Inst.Alert("Network is NOT available");
			UIManager.Inst.PopupMessage (Localization.Get("network"), 1.5f);
			return;
		}

		if (!RcGameService.Inst.authenticated) {
			RcGameService.Inst.Login (cbSign, 10f);
			return;
		} else {
			OnSignOut ();
		}

		// base.OnTouched ();
	}


	void cbSign(bool success, string error)
	{			
		// when failed, show message
		if (success)
			return;

		//Debug.LogFormat ("RcUISign:cbSign() {0}={1}", success, error);
		UIManager.Inst.PopupMessage (Localization.Get(success? "sign_success" : "sign_failed") , 1.5f);		
		// UIManager.Inst.Alert(string.Format("Sign={0} err={1}",success,error));
	}


	public void OnSignOut()
	{
		// UIManager.Inst.Alert("OnSignOut()");
		RcGameService.Inst.Logout(cbSignOut);
	}

	void cbSignOut(bool success, string error)
	{
		UIManager.Inst.PopupMessage(Localization.Get("signout"), 1.5f);		
		// UIManager.Inst.Alert(string.Format("SignOut={0} err={1}",success,error));
	}
}