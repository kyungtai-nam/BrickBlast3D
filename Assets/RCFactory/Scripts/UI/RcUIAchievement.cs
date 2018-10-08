using UnityEngine;
using System.Collections;

// 업적 
public class RcUIAchievement : RcUI
{
	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Right);
		moveDelay = 0.2f;
	}

	public override void OnTouched()
	{
		Debug.Log ("RcUIArchievement:OnTouched()");

		RcGameService.Inst.ShowAchievement();
		base.OnTouched ();
	}
	
}
