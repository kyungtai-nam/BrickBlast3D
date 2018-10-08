using UnityEngine;
using System.Collections;

// turn
public class UIStep : RcUI
{
	UILabel lbCount;

	TweenScale twScale = null;	
	RcScrollInt sn;

	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Left);
		moveDelay = 0.2f;

		lbCount = transform.Find("Count").GetComponent<UILabel>();
		twScale = GetComponent<TweenScale> ();

		sn = gameObject.AddComponent<RcScrollInt>();
		sn.Init(lbCount, "{0}", UpdateVFX);
	}

	void Start()
	{
		sn.SetInt(Mathf.Max(0, PlayManager.Inst.step-1), true);
	}

	void FixedUpdate()
	{
		sn.SetInt(Mathf.Max(0, PlayManager.Inst.step-1));
	}

	void UpdateVFX()
	{
		twScale.ResetToBeginning();
		twScale.PlayForward();
	}
}
