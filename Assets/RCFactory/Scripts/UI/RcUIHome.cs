using UnityEngine;
using System.Collections;

// 홈으로 이동
public class RcUIHome : RcUI
{
	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Right);
		moveDelay = 0.0f;

		// movePos = Global.Inst.UI_MOVE_TOP_FAR;
		// moveDelay = 0.0f;
	}

	public override void OnTouched()
	{
		Debug.Log ("RcUIHome:OnTouched()");

		Time.timeScale = 1f;

//		UIManager.Inst.ShowGameUI (false);
//		UIManager.Inst.ShowHomeUI (true);

		UIManager.Inst.uiPlayResume.Hide ();

		Hide ();

		base.OnTouched ();
	}
}


