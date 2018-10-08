using UnityEngine;
using System.Collections;


// not used
public class SetRenderQ : MonoBehaviour 
{
	[SerializeField]
	int rQueue = 4000;

	void Start()
	{
		GetComponent<Renderer>().material.renderQueue = rQueue;

		Transform trans = transform;

		for ( int i = 0 ; i <trans.childCount; i ++) 
		{
			trans.GetChild(i).gameObject.GetComponent<Renderer>().material.renderQueue = rQueue;
		}
	}
}
