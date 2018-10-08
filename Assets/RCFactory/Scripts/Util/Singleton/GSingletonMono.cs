using UnityEngine;
using System.Collections;

public class GSingletonMono<T> : MonoBehaviour where T : GSingletonMono<T>
{
	public static T g_Instance	= default(T);

	static public T Inst
	{
		get
		{
			if( null == g_Instance )
			{
				if( null == (g_Instance=GameObject.FindObjectOfType(typeof(T)) as T) )
				{
					string 		strName = typeof(T).ToString();
					GameObject 	go 		= new GameObject(string.Format("[{0}]", strName));
/*
#if UNITY_EDITOR
					Debug.Log(string.Format("Could not locate an {0} object. {0} was Generated Automaticly.",strName));
#endif
*/
					g_Instance = go.AddComponent<T>();
				}
			}
			return g_Instance;
		}
	}
	
	static public bool IsValid()
	{
		return (null != g_Instance);
	}
	
	protected virtual void Awake()
	{
		DontDestroyOnLoad(this);
	}
}