using UnityEngine;
using System.Collections;

public class LayerUtil : MonoBehaviour 
{

	public static void ChangeLayer(int layer, GameObject go, bool applyChild=true)
	{
		go.layer = layer;

		if (!applyChild)
			return;
			
		for ( int n=0 ; n<go.transform.childCount ; n++ )
		{
			go.transform.GetChild(n).gameObject.layer = layer;
		}
	}

	public static void ChangeLayer(string layerName, GameObject go, bool applyChild=true)
	{
		int layer = LayerMask.NameToLayer(layerName);

		go.layer = layer;

		if (!applyChild)
			return;
			
		for ( int n=0 ; n<go.transform.childCount ; n++ )
		{
			go.transform.GetChild(n).gameObject.layer = layer;
		}
	}
}
