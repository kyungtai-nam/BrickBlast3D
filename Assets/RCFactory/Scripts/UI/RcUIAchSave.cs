using UnityEngine;
using System.Collections;

// Debug : Achievement Save
public class RcUIAchSave : RcUI
{	
	protected override void Awake()
	{		
		base.Awake ();

		//SetMoveDirection(EMoveDir.Right);
		movePos = Global.Inst.UI_MOVE_BOTTOM_FAR; 
		moveDelay = 0.1f;
	}

	public override void OnTouched()
	{
		Debug.Log ("RcUIAchSave:OnTouched()");
		UIManager.Inst.PopupMessage ("RcUIAchSave");

		// success
		// RcGameService.Inst.SaveAchievement (RcGameService.ACH_GID_NEWBIE);
		RcGameService.Inst.SaveAchievement (RcGameService.ACH_GID_BRONZE);

	}
}