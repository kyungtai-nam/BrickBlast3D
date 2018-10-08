using UnityEngine;
using System.Collections;


// 시작 버튼 
public class RcUIPlay : RcUI
{

	protected override void Awake()
	{		
		base.Awake ();

		// SetMoveDirection(EMoveDir.Bottom);
		movePos = Global.Inst.UI_MOVE_BOTTOM_FAR; 
	}

	public override void OnTouched()
	{
		// Debug.Log (string.Format("RcUIPlay({0}):OnTouched()", name));

		base.OnTouched ();
	}
}