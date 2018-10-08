using UnityEngine;
using System.Collections;

/*
	usage>

	RcScrollSlider ss;

	Awake()
		ss = gameObject.AddComponent<RcScrollSlider>();
		ss.Init(slider);

	FixedUpdate()
		ss.SetValue(now, max);

*/

// HUD UI - Scroll number for slider
public class RcScrollSlider : MonoBehaviour 
{
	UISlider slider;

	// 현재 애니메이션 중인가?
	bool animating = false;

	int val = -1;
	int newVal = -1;
	int diff = 0;

	int maxVal = 0;

	int multiple = 1;

	public int accThreshold = 40;
	bool init = false;

	void Awake()
	{
		
	}

	public void Init(UISlider slider, int multiple=1)
	{
		this.multiple = multiple;
		
		this.slider =  slider;
		val = newVal = diff = maxVal = 0;
		init = true;
	}

	void Update()
	{
		if ( !init )
			return;
			
		UpdateNumber();
	}

	bool UpdateNumber()
	{
		if ( !animating )
			return false;

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

		DrawSlider();		
		return true;
	}

	float normVal = 0f;

	void DrawSlider()
	{
		normVal = (float)val/(float)maxVal;

		/*
		Debug.Log(string.Format("ScrollSlider:DrawSlider() {0}/{1} = {2:F2}",
			val, maxVal, normval));
		*/
		slider.value = normVal;
	}

	public bool SetValue(int _val, int _maxVal, bool noAni=false)
	{
		if (0 >= _val)
			noAni = true;
		
		_val *= multiple;
		_maxVal *= multiple;

		maxVal = _maxVal;

		// 초기화시 애니 없이 출력
		if ( true == noAni ) 
		{
			newVal = val = _val;
			diff = 0;
			animating = false;
			
			DrawSlider();
			return false;
		}

		if ( val == _val ) 
			return false;

		newVal = _val;
		animating = true;

		return true;
	}
}