using UnityEngine;
using System.Collections;

// Debug : Full Banner 
public class RcUIFullBanner : RcUI
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
		Debug.Log ("RcUIFullBanner:OnTouched()");

		// TODO : disable touch and hide sprite

		RcAdManager.Inst.ShowFullBanner ();

		#if UNITY_EDITOR
		Invoke("EditorTest", 0.5f);
		#endif
	}

	void EditorTest()
	{
		Hide();
		base.OnTouched ();
	}
}