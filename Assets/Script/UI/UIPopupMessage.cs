using UnityEngine;
using System.Collections;

public class UIPopupMessage : MonoBehaviour {
	public UI2DSprite bg;
	public UILabel lb;
	public TweenHeight twHeight;
	public TweenScale twScale;

	public void Show(string msg, float duration=0.6f)
	{
		lb.text = msg;

		//twHeight.ResetToBeginning ();
		// twHeight.PlayForward();

		twScale.duration = duration;
		twScale.ResetToBeginning ();
		twScale.PlayForward ();
	}

	public void Done()
	{
		GPoolManager.Inst.Delete(gameObject);
	}
}
