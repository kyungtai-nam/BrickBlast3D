using UnityEngine;
using System.Collections.Generic;

public static class ListExtension 
{
	public static T First<T>(this List<T> list)
	{
		return list [0];
	}

	public static T Last<T>(this List<T> list)
	{
		return list [list.Count - 1];
	}

	public static T Random<T>(this List<T> list)
	{		
		return list [UnityEngine.Random.Range (0, list.Count)];
	}

	public static T RemoveAndGetRandom<T>(this List<T> list) 
	{
		int pos = UnityEngine.Random.Range (0, list.Count);

		T item = list [pos];
		list.RemoveAt (pos);
		return item;
	}

	public static void Shuffle<T>(this List<T> list)
	{		
		for(int i = 0; i < list.Count; i++) {			
			T temp = list [i];
			int randomIndex = UnityEngine.Random.Range(0, list.Count);
			list [i] = list [randomIndex];
			list[randomIndex] = temp;
		}
	}

	public static void ReleaseToPool(this List<GameObject> list)
	{
		for (int n = 0; n < list.Count; n++) {
			GPoolManager.Inst.Delete (list[n]);
		}
		list.Clear ();
	}

	public static void ReleaseToPool<T>(this List<T> list) where T : MonoBehaviour
	{
		for (int n = 0; n < list.Count; n++) {
			GPoolManager.Inst.Delete (list [n].gameObject);
		}
		list.Clear ();
	}
}