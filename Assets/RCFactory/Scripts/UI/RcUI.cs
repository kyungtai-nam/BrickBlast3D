using UnityEngine;
using System.Collections;


// UI 기본 클래스 
public class RcUI : MonoBehaviour
{
	protected TweenPosition twPos = null;	
	public System.Action cbTouched = null;
	public System.Action cbHided = null;	
	public System.Action cbShown = null;

	// TweenPosition 이동 거리 
	protected Vector3 movePos = Vector3.zero;

	// TweenPosition 이동 시간
	protected float moveDuration = Global.UI_MOVE_TIME;

	// TweenPosition 시작 딜레이
	protected float moveDelay = 0f;

	public enum EMoveDir 
	{
		Top = 0,
		Bottom = 1,
		Left = 2,
		Right =3,
		Total
	}

	protected bool isShow = false;

	protected virtual void Awake()
	{
		SetColorBG ();

		SetMoveDirection(EMoveDir.Top);

		twPos = GetComponent<TweenPosition>();

		if ( null == twPos )
		{
			Debug.LogWarning("RcUI:Awake() null == twPos"); 
			twPos = gameObject.AddComponent<TweenPosition>();
		}
			
		twPos.enabled = false;
	}

	void SetColorBG()
	{
		Transform tm = transform.Find ("BG");
		if (null == tm)
			return;

		UI2DSprite sprBG = tm.GetComponent<UI2DSprite> ();
		if ( null != sprBG )
			sprBG.color = Global.Inst.BTN_COLOR_BG;
	}

	protected void SetMoveDirection(EMoveDir dir)
	{
		switch ( dir ) 
		{
		case EMoveDir.Top: movePos = Global.Inst.UI_MOVE_TOP; break;
		case EMoveDir.Bottom: movePos = Global.Inst.UI_MOVE_BOTTOM; break;
		case EMoveDir.Left: movePos = Global.Inst.UI_MOVE_LEFT; break;
		case EMoveDir.Right: movePos = Global.Inst.UI_MOVE_RIGHT; break;
		}
	}

	public virtual void OnTouched()
	{
		SoundManager.Inst.Play ("Sound/UISelect");
		if ( null != cbTouched ) 
			cbTouched();
	}

	public virtual void Show()
	{
		if ( isShow ) 
			return;

		isShow = true;
		moveIn();
	}

	public virtual void Hide()
	{
		if ( !isShow ) 
			return;

		isShow = false;
		moveOut();
	}

	void moveIn()
	{
		twPos.from = movePos;
		twPos.to = Vector3.zero;

		twPos.duration = moveDuration;
		twPos.delay = moveDelay;

		twPos.method = UITweener.Method.EaseInOut;
		twPos.style = UITweener.Style.Once;

		twPos.eventReceiver = gameObject;
		twPos.callWhenFinished = "OnFinishShow";

		twPos.ResetToBeginning();
		twPos.PlayForward();
	}

	void moveOut()
	{
		twPos.from = Vector3.zero;
		twPos.to = movePos;
		
		twPos.duration = moveDuration;
		twPos.delay = moveDelay;

		twPos.method = UITweener.Method.EaseInOut;
		twPos.style = UITweener.Style.Once;

		twPos.eventReceiver = gameObject;  //이동이 끝나면 호출되는 콜백 - 
		twPos.callWhenFinished = "OnFinishHide"; 
	
		twPos.ResetToBeginning();
		twPos.PlayForward();


	}

	protected void OnFinishHide()
	{
		// transform.localPosition = Vector3.zero;
		if (null != cbHided)
			cbHided ();

		gameObject.SetActive(false);
	}

	protected void OnFinishShow()
	{
		if (null != cbShown)
			cbShown ();		
	}
}