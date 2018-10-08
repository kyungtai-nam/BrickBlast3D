using UnityEngine;
using System.Collections;


// 리뷰 요청 
public class RcUIRate : RcUI
{
	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Left);
		moveDelay = 0.2f;
	}

	public override void OnTouched()
	{
		Debug.Log ("RcUIRate:OnTouched()");

		// TODO : 일정 횟수에 따라 평가 창 뜨도록!
		RcShare.Inst.RateRequest();

		base.OnTouched ();
	}
}