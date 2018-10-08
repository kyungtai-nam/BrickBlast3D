using UnityEngine;

public class UIScore: RcUI
{
	UILabel lbCount;

	TweenScale twScale = null;	
	RcScrollInt sn;

	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Top);
		moveDelay = 0.2f;

		lbCount = GetComponent<UILabel>();
		twScale = GetComponent<TweenScale> ();

		sn = gameObject.AddComponent<RcScrollInt>();
		sn.Init(lbCount, "{0}", UpdateVFX);
	}

	void Start()
	{		
		sn.SetInt(PlayManager.Inst.score, true);
	}

	void FixedUpdate()
	{
		sn.SetInt(PlayManager.Inst.score);
	}

	void UpdateVFX()
	{
		twScale.ResetToBeginning();
		twScale.PlayForward();
	}
}
