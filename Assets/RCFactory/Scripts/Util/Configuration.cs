using UnityEngine;
using System.Collections;

public class Configuration : MonoBehaviour 
{
	public bool showBtn = false; // 퀄리티 상승, 하강에 대한 버튼 표시여부
	public Light light = null;

	void Awake()
	{
		QualitySettings.SetQualityLevel(5);
		QualitySettings.antiAliasing = 8;

		Application.runInBackground = true;

#if !UNITY_EDITOR
		InitLight();
#endif
	}

	void InitLight()
	{
		if ( null == light )
			return;

		light.color = Color.white;
		
		light.shadows = LightShadows.Soft;
		// light.shadows = LightShadows.Hard;

		light.shadowStrength = 0.6f;
		light.shadowBias = 0.05f;
		light.shadowNormalBias = 0.4f;
		light.shadowNearPlane = 0.2f;
	}


	Rect rtInc = new Rect(10,10, 200, 200);
	Rect rtDec = new Rect(10,300, 200, 200);

    public void OnGUI() 
    {
		if (!showBtn)
			return;

        if ( GUI.Button(rtInc, "Increase Quality Level") ) 
        {
            QualitySettings.IncreaseLevel(true);
        }

        if ( GUI.Button(rtDec, "Decrease Quality Level") ) 
        {
            QualitySettings.DecreaseLevel(true);
        }
    }



}
