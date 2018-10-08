using UnityEngine;
using System.Collections;

public class GSingleton<T> where T: new()
{
	public static T g_Instance = default(T);

	static public T Inst
	{
		get
		{
			if( null == g_Instance )
			{
				g_Instance = new T();
#if UNITY_EDITOR
				string strName = typeof(T).ToString();
				Debug.Log(string.Format("Could not locate an {0} object. {0} was Generated Automaticly.",strName));
#endif
			}
			return g_Instance;
		}
	}
	
	static public bool IsValid()
	{
		return (null != g_Instance);
	}
}

