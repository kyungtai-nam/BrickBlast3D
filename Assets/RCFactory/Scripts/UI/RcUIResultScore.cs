using UnityEngine;
using System.Collections;

// 결과 스코어, 베스트 스코어 
public class RcUIResultScore : RcUI
{
	UILabel lbLastScore;
	UILabel lbBestScore;

	int cacheLastScore = -1;
	int cacheBestScore = -1;


	protected override void Awake()
	{		
		base.Awake ();

		SetMoveDirection(EMoveDir.Top);

		movePos = Global.Inst.UI_MOVE_TOP_FAR; 
		moveDelay = 0.1f;	

		lbLastScore = transform.Find ("LastScore").GetComponent<UILabel>();
		lbBestScore = transform.Find ("BestScore").GetComponent<UILabel>();
	}

	void FixedUpdate()
	{
		if (cacheLastScore != RcSaveData.Inst.lastScore) {
			cacheLastScore = RcSaveData.Inst.lastScore;
			lbLastScore.text = string.Format ("[i]{0}[/i]", cacheLastScore);
		}

		if (cacheBestScore != RcSaveData.Inst.bestScore) {
			cacheBestScore = RcSaveData.Inst.bestScore;
			lbBestScore.text = string.Format ("[i]{0}[/i]", cacheBestScore);
		}
	}
}