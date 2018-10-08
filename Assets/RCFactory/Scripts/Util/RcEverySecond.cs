using UnityEngine;
using System.Collections;

// call per every second
public class RcEverySecond {
	public int interval = 1;
	float nextTime = 0f;

	int count = 0;
	System.Action<int> cbOnTime = null;

	void Update()
	{
		if (Time.time < nextTime)
			return;

		nextTime += interval;

		if (null != cbOnTime)
			cbOnTime (++count);
	}
}
