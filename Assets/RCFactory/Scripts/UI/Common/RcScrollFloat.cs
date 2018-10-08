
using UnityEngine;
using System.Collections;

// HUD UI - Scroll float Label
public class RcScrollFloat : MonoBehaviour 
{
	public UILabel label;

	// 현재 애니메이션 중인가?
	bool animating = false;

	float val = 0;
	float newVal = 0;
	float diff = 0;

	const float aniCount = 7.5f;

	bool init = false;

	int fontSize = 0;

	bool fontResize = false;

	string fmt = "{0:F1}";

	const float epsilon = 0.1f;

	float minStep = 0.1f;

	public float Val
	{
		get
		{
			return val;
		}
	}

	void Awake()
	{

	}

	public void Init(UILabel _label, bool _fontResize=true, string _fmt="{0:F1}")
	{
		label = _label;// GetComponent<UILabel>();
		fmt = _fmt;

		fontResize = _fontResize;
		fontSize = label.fontSize; 
		val = newVal = diff = 0.0f;
		init = true;
	}

	void FixedUpdate()
	{
		if ( !init )
			return;

		UpdateNumber();
	}

	bool UpdateNumber()
	{
		if ( !animating )
			return false;

		if (aniCount > Mathf.Abs(newVal - val) )
		{
			// 얼마 차이 안나면 1씩 
			diff = (newVal - val > 0 ? minStep : -minStep);
		}
		else
		{
			diff = (newVal - val ) / aniCount;

			val += (newVal - val) % aniCount;
		}

		if ( Mathf.Abs(newVal - val) < epsilon ) 
		{
			if ( true == fontResize ) 
				label.fontSize = fontSize; 

			animating = false;
			val = newVal;
		}	
		else
		{
			val += diff;

			if ( fontResize && label.fontSize == fontSize ) 
				label.fontSize += 4;
		}

		DrawLabel();		
		return true;
	}

	void DrawLabel()
	{
		label.text = string.Format(fmt, val);
	}

	public bool SetInt(float _val, bool noAni=false)
	{
		// 초기화시 애니 없이 출력
		if ( true == noAni ) 
		{
			newVal = val = _val;
			diff = 0;
			animating = false;
			DrawLabel();
			return false;
		}

		if ( Mathf.Abs(val - _val) < epsilon ) 
			return false;

		newVal = _val;
		animating = true;

		return true;
	}
}