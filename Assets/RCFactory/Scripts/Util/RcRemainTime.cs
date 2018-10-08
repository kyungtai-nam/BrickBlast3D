using UnityEngine;
using System.Collections;

// 남은 시간 출력용
public class RcRemainTime
{
	System.DateTime expiredTime;
	System.TimeSpan span;

	public override string ToString()
	{
		return expiredTime.ToString();
	}

	public bool Attach(string timeString)
	{
		if ( string.IsNullOrEmpty(timeString) ) 
			return false;

		expiredTime = System.DateTime.Parse(timeString);
		return true;
	}

	public void Reset(int addHour=0, int addMin=0, int addSec=0)
	{
		expiredTime = System.DateTime.Now;

		if ( 0 < addHour ) 
			expiredTime = expiredTime.AddHours(addHour);

		if ( 0 < addMin )	
			expiredTime = expiredTime.AddMinutes(addMin);

		if ( 0 < addSec )
			expiredTime = expiredTime.AddSeconds(addSec);
	}

	
	public bool IsExpired()
	{
		return GetRemainSeconds() <= 0;
	}

	public int GetRemainSeconds()
	{
		span = expiredTime - System.DateTime.Now;
		return Mathf.Max(0, (int)span.TotalSeconds);
	}

	public string ToFormatString(bool hour=false, bool min=true, bool sec=true)
	{
		string msg = "";

		span = expiredTime - System.DateTime.Now;

		if ( hour ) 
			msg += string.Format("{0:00}", span.Hours);

		if ( min )
		{
			if ( hour ) 
				msg += ":";
			
			msg += string.Format("{0:00}", span.Minutes);
		}	

		if ( sec ) 
		{
			if ( min ) 
				msg += ":";

			msg += string.Format("{0:00}", span.Seconds);	
		}

		return msg;
	}
}
