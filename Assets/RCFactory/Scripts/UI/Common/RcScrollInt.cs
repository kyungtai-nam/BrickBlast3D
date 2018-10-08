using UnityEngine;
using System.Collections;

/*
	usage>


using UnityEngine;
using System.Collections;

// HUD UI - 골드 
public class HudGold : MonoBehaviour 
{
	UILabel txt;
	RcScrollInt sn;

	void Awake()
	{
		txt = transform.Find("Count").gameObject.GetComponent<UILabel>();

		sn = gameObject.AddComponent<ScrollNumber>();
		sn.Init(txt);
	}

	void Start()
	{
		sn.SetInt(Atts.Inst.account.gold, true);
	}
	
	void FixedUpdate()
	{
		sn.SetInt(Atts.Inst.account.gold);
	}
}


*/

// HUD UI - Scroll int 
public class RcScrollInt : MonoBehaviour 
{
	// NGUI Label 레퍼런스 
	UILabel label;

	// 현재 애니메이션 중인가?
	bool animating = false;

	// 두 수의 차이가 임계치 이상이면 가속(skip)
	public int accThreshold = 20;

	// 두 수 계산용
	int val = 0;
	int newVal = 0;
	int diff = 0;

	// 초기화 여부 
	bool init = false;

	string fmt = "{0}";

	public bool debug = false;

	// 변경시 딜레이 
	public float delay = 0.05f;

	// 누적된 시간(임시변수)
	float time = 0f;

	//Tween을 위한 값 변경시 콜백 
	System.Action cbChangeValue = null;

	// 값 감소시 애니 없이 즉시 	
	public bool isDecreaseNoAni = true;

	public void Init(UILabel label, string fmt="{0}", System.Action cbChangeValue=null)
	{
		this.cbChangeValue = cbChangeValue;

		this.label = label;
		this.fmt = fmt;
		
		val = newVal = diff = 0;
		init = true;
	}

	void FixedUpdate()
	{
		if ( !init )
			return;
	
		time += Time.deltaTime;
		if ( time < delay ) 
			return;
		time = 0f;

		UpdateNumber();
	}

	bool UpdateNumber()
	{
		if ( !animating )
			return false;

		if ( isDecreaseNoAni ) 
		{
			if ( newVal <= val ) 
			{
				animating = false;
				val = newVal;
				DrawLabel();

				return true;
			}
		}

		if (accThreshold >= Mathf.Abs(newVal - val) )
		{
			// 얼마 차이 안나면 1씩 
			diff = (newVal - val > 0 ? 1 : -1);
		}
		else
		{
			diff = (newVal - val ) / accThreshold;
			
			if ( 0 == diff )
				diff = (newVal - val > 0 ? 1 : -1);

			val += (newVal - val) % accThreshold;
		}

		if ( 0 == newVal - val )
			animating = false;
		else
			val += diff;

		DrawLabel();
		return true;
	}

	void DrawLabel()
	{
		label.text = string.Format(fmt, val);

		if ( null != cbChangeValue ) 
			cbChangeValue ();
	}


	public bool SetInt(int _val, bool noAni=false)
	{
		// 초기화시 애니 없이 출력
		if ( true == noAni ) 
		{
			newVal = _val;
			val = _val;

			diff = 0;
			animating = false;
			DrawLabel();

			if ( debug )
			{
				Debug.Log(string.Format("ScrollNumber:SetInt() #1 newVal={0},val={1},_val={2}",
					newVal, val, _val));
			}

			return false;
		}

		if ( val == _val ) 
			return false;

		if ( debug )
		{
			Debug.Log(string.Format("ScrollNumber:SetInt() #2 newVal={0},val={1},_val={2}",
				newVal, val, _val));
		}

		newVal = _val;
		animating = true;
		return true;
	}
}