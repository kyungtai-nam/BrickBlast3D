using UnityEngine;
using System.Collections;


[RequireComponent(typeof(UILabel))]
public class FPSNgui : MonoBehaviour 
{
    UILabel FpsLabel;
    
    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    void Awake()
    {
        FpsLabel = GetComponent<UILabel>();

        QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = 60;
    }
    
    void Start()
    {
        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
        if (timeleft <= 0.0)
        {
            float fps = accum / frames;
            string format = System.String.Format("{0:F0} FPS", fps);

            FpsLabel.text = format;

            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }
}
