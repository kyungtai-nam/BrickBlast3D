using UnityEngine;
using System.Collections;

public class ObjPingPong : MonoBehaviour 
{

	Vector3 src;
	Vector3 dst;

	public float dist = 0.5f;
	float startTime;

	public float timeTakenDuringLerp = 2f;
	public bool yAxis = true;

	bool toggle = false;

	public float pauseDelay = 0.5f;

	void Awake()
	{
		src = transform.localPosition;
		dst = transform.localPosition;

		if ( yAxis )
		{	
			src.y -= dist/2;
			dst.y += dist/2;
		}
		else
		{
			src.x -= dist/2;
			dst.x += dist/2;
		}	
	}

	IEnumerator Start()
	{
		while (true) 
		{ 
			float timeSinceStarted = Time.time - startTime;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;
 
 			if ( toggle ) 
				transform.localPosition = Vector3.Lerp(src, dst, percentageComplete);
			else	
				transform.localPosition = Vector3.Lerp(dst, src, percentageComplete);

			yield return null;

			if (percentageComplete > 1.0f)
			{
				startTime = Time.time;
				toggle = !toggle;

				yield return new WaitForSeconds(pauseDelay);
			}
		}
	}
}
