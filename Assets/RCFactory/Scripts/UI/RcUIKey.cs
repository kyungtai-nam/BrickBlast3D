using UnityEngine;
using System.Collections;


// Key
public class RcUIKey : RcUI
{
	UILabel lbCount;
	TweenScale twScale = null;	
	RcScrollInt sn;

	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Top);
		moveDelay = 0.1f;
		
		lbCount = transform.Find("Count").GetComponent<UILabel>();
		twScale = GetComponent<TweenScale> ();

		sn = gameObject.AddComponent<RcScrollInt>();
		sn.Init(lbCount, "{0}", UpdateVFX);
	}

	void Start()
	{
		sn.SetInt(RcSaveData.Inst.key, true);
	}

	void FixedUpdate()
	{
		sn.SetInt(RcSaveData.Inst.key);
	}

	void UpdateVFX()
	{
		twScale.ResetToBeginning();
		twScale.PlayForward();
	}
}