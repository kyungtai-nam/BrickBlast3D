using UnityEngine;

public class FPSGUIText : MonoBehaviour
{
	public  float updateInterval = 0.5F;

	private float accum   = 0;
	private int   frames  = 0;
	private float timeleft;

	void Awake()
	{
		QualitySettings.vSyncCount = 0;
		// Application.targetFrameRate = 60;
	}

	void Start()
	{
		if (!GetComponent<GUIText>())
		{
			Debug.LogError("UtilityFramesPerSecond needs a GUIText component!");
			enabled = false;
			return;
		}

		timeleft = updateInterval;
	}

	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;

		// Interval ended - update GUI text and start new interval
		if (timeleft <= 0.0)
		{
			// display two fractional digits (f2 format)
			float fps = accum / frames;
			string format = System.String.Format("{0:F2} FPS", fps);
			GetComponent<GUIText>().text = format;

			Color clr = Color.white;

			if ( 10 > fps )
				clr = Color.red;
			else if ( 30 > fps )
				clr = Color.yellow;
			else
				clr = Color.green;

			GetComponent<GUIText>().material.color = clr;

			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
}