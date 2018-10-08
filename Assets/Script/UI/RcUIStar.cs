using UnityEngine;
using System.Collections;

// 모은 별 갯수
public class RcUIStar : RcUI
{
	UILabel lbCount;

	TweenScale twScale = null;	
	RcScrollInt sn;

	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Right);
		moveDelay = 0.2f;
		
		lbCount = transform.Find("Count").GetComponent<UILabel>();
		twScale = GetComponent<TweenScale> ();

		sn = gameObject.AddComponent<RcScrollInt>();
		sn.Init(lbCount, "{0}", UpdateVFX);
	}
		
	void Start()
	{
		sn.SetInt(RcSaveData.Inst.gem, true);
	}

	void FixedUpdate()
	{
		sn.SetInt(RcSaveData.Inst.gem);

	}

	void UpdateVFX()
	{
		twScale.ResetToBeginning();
		twScale.PlayForward();
	}
}