using UnityEngine;
using System.Collections;

public class GValueParse
{	
	public static bool BoolParse(string value)
	{
		bool	number = false;
		bool 	result = bool.TryParse(value, out number);

		return (true == result)?number:false;
	}
		
	public static int IntParse(string value)
	{
		int 	number = 0;
		bool 	result = int.TryParse(value, out number);

		return (true == result)?number:0;
	}

	public static System.Int16 Int16Parse(string value)
	{
		System.Int16 	number = 0;
		bool 			result = System.Int16.TryParse(value, out number);
		
		return (true == result)?number:(System.Int16)0;
	}

	public static System.Int64 Int64Parse(string value)
	{
		System.Int64 	number = 0;
		bool 			result = System.Int64.TryParse(value, out number);
		
		return (true == result)?number:0;
	}

	public static System.UInt16 UInt16Parse(string value)
	{
		System.UInt16 	number = 0;
		bool 			result = System.UInt16.TryParse(value, out number);
		
		return (true == result)?number:(System.UInt16)0;
	}

	public static System.UInt32 UInt32Parse(string value)
	{
		System.UInt32 	number = 0;
		bool 			result = System.UInt32.TryParse(value, out number);
		
		return (true == result)?number:0;
	}

	public static System.UInt64 UInt64Parse(string value)
	{
		System.UInt64 	number = 0;
		bool 			result = System.UInt64.TryParse(value, out number);
		
		return (true == result)?number:0;
	}
	
	public static float FloatParse(string value)
	{
		float 	number = 0;
		bool 	result = float.TryParse(value, out number);
		
		return (true == result)?number:0.0f;
	}
		
	public static System.DateTime DateParse(string value)
	{
		System.DateTime dt;
		try
		{
			dt = System.Convert.ToDateTime(value);
		}
		catch(System.FormatException e)
		{
			dt = new System.DateTime(0);
#if UNITY_EDITOR
			Debug.Log("TryToParseDate error="+e);
#endif
		}
		return dt;
	}
}

