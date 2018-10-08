using UnityEngine;
using System.Collections;

public class RcPixelDialog : MonoBehaviour {

	public string strPrevColor = "";

	// link
	public UILabel lbText;
	public UI2DSprite sprOuter;
	public UI2DSprite sprOutline;
	public UI2DSprite sprShadow;
	public UI2DSprite sprHighlight;
	public UI2DSprite sprBgDark;
	public UI2DSprite sprBg;
	public BoxCollider coll;

	public UI2DSprite sprTouchGuard;


	public GameObject goText;
	public GameObject goOuter;
	public GameObject goOutline;
	public GameObject goShadow;
	public GameObject goHighlight;
	public GameObject goBgDark;
	public GameObject goBg;
	public GameObject goTouchGuard;


	// var
	public int startDepth;

	public int width;
	public int height;

	public bool isOuter;
	public Color outer;

	public bool isOutline;
	public Color outline;

	public bool isShadow;
	public Color shadow;

	public bool isHighlight;
	public Color highlight;

	public bool isBgDark;
	public Color bgDark;

	public bool isBg;
	public Color bg;

	public bool isFont;
	public Color font;

	public bool isFontShadow;
	public Color fontShadow;

	public bool isTouchGuard;
	public Color touchGuardBG;



	public void ApplyObject()
	{
		coll = GetComponent<BoxCollider> ();

		goText = transform.Find("Text").gameObject;
		goOuter = transform.Find("Outer").gameObject;
		goOutline = transform.Find("Outline").gameObject;
		goShadow = transform.Find("Shadow").gameObject;
		goHighlight = transform.Find("Highlight").gameObject;
		goBgDark = transform.Find("BgDark").gameObject;
		goBg = transform.Find("Bg").gameObject;

		lbText = goText.GetComponent<UILabel>();
		sprOuter = goOuter.GetComponent<UI2DSprite>();
		sprOutline = goOutline.GetComponent<UI2DSprite>();
		sprShadow = goShadow.GetComponent<UI2DSprite>();
		sprHighlight = goHighlight.GetComponent<UI2DSprite>();
		sprBgDark = goBgDark.GetComponent<UI2DSprite>();
		sprBg = goBg.GetComponent<UI2DSprite>();


		if (null != transform.Find ("TouchGuard")) {
			goTouchGuard = transform.Find ("TouchGuard").gameObject;
			sprTouchGuard = goTouchGuard.GetComponent<UI2DSprite> ();
		} else {
			goTouchGuard = null;
			sprTouchGuard = null;
		}
	}


	public void ApplySize(int width, int height)
	{
		this.width = width;
		this.height = height;

		coll.size = new Vector3 (width, height);

		lbText.width = width;
		lbText.height = height;

		sprOuter.width = width; 
		sprOuter.height = height;

		sprOutline.width = width; 
		sprOutline.height = height;

		sprShadow.width = width; 
		sprShadow.height = height;

		sprHighlight.width = width; 
		sprHighlight.height = height;


		Vector3 pos = sprBgDark.transform.localPosition;
		pos.y = -height / 2;
		sprBgDark.transform.localPosition = pos;

		sprBgDark.width = width;
		sprBgDark.height = height / 2 + 16;

		sprBg.width = width; 
		sprBg.height = height;
	}

	void SetUsingColor()
	{
		isOuter = IsEnable(outer);
		isOutline = IsEnable(outline);
		isShadow = IsEnable(shadow);
		isHighlight = IsEnable(highlight);
		isBgDark = IsEnable(bgDark);
		isBg = IsEnable(bg);
		isFont = IsEnable(font);
		isFontShadow = IsEnable(fontShadow);
		// isTouchGuard = IsEnable (touchGuardBG);
	}

	public bool SetClrGroup(string[] arr)
	{
		if (8 != arr.Length)
			return false;

		outer = HexToClr (arr [0]);
		outline = HexToClr (arr [1]);
		shadow = HexToClr (arr [2]);
		highlight = HexToClr (arr [3]);
		bgDark = HexToClr (arr [4]);
		bg = HexToClr (arr [5]);
		font = HexToClr (arr [6]);
		fontShadow = HexToClr (arr [7]);

		// touchGuardBG = HexToClr (arr [8]);
		SetUsingColor();
		return true;
	}

	public void ApplyDepth()
	{
		int order = 0;

		if ( null != sprTouchGuard ) 
			sprTouchGuard.depth = startDepth + order++;
		
		sprBg.depth = startDepth + order++;
		sprBgDark.depth = startDepth + order++;
		sprHighlight.depth = startDepth + order++;
		sprShadow.depth = startDepth + order++;
		sprOutline.depth = startDepth + order++;
		sprOuter.depth = startDepth + order++;
		lbText.depth = startDepth + order++;
	}

	/*
	public void SetFontSize(int size)
	{
		lbText.fontSize = size;
	}
	*/

	public void ApplyColor(string prevColor=null)
	{
		if (!string.IsNullOrEmpty (prevColor))
			strPrevColor = prevColor;
			

		if (isFont) {
			lbText.enabled = true;
			lbText.color = font;
		} else {
			lbText.enabled = false;
		}

		if (isFontShadow) {
			lbText.effectStyle = UILabel.Effect.Shadow;
			lbText.effectColor = fontShadow;
		} else {
			lbText.effectStyle = UILabel.Effect.None;
			lbText.effectColor = fontShadow;
		}


		goOuter.SetActive (isOuter);
		sprOuter.enabled = isOuter;
		sprOuter.color = outer;

		goOutline.SetActive (isOutline);
		sprOutline.enabled = isOutline;
		sprOutline.color = outline;

		goShadow.SetActive (isShadow);
		sprShadow.enabled = isShadow;
		sprShadow.color = shadow;

		goHighlight.SetActive (isHighlight);
		sprHighlight.enabled = isHighlight;
		sprHighlight.color = highlight;

		goBgDark.SetActive (isBgDark);
		sprBgDark.enabled = isBgDark;
		sprBgDark.color = bgDark;

		goBg.SetActive (isBg);
		sprBg.enabled = isBg;
		sprBg.color = bg;

		if (null != sprTouchGuard) {
			goTouchGuard.SetActive (isTouchGuard);
			sprTouchGuard.enabled = isTouchGuard;
			sprTouchGuard.color = touchGuardBG;
		}
	}


	public bool IsEnable(Color clr)
	{
		return clr.a > 0;
	}

	// string to Color 
	public Color HexToClr(string hex)
	{
		if (string.IsNullOrEmpty (hex))
			return new Color (0, 0, 0, 0);
				
		hex = hex.Replace ("0x", "");//in case the string is formatted 0xFFFFFF
		hex = hex.Replace ("#", "");//in case the string is formatted #FFFFFF
		byte a = 255;//assume fully visible unless specified in hex
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		//Only use alpha if the string has enough characters
		if(hex.Length == 8){
			a = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		}
		return new Color32(r,g,b,a);
	}

}