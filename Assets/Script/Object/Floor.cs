using UnityEngine;
using System.Collections;

// reflection mirror
public class Floor : MonoBehaviour {

	Renderer render = null;

	void Awake()
	{
		render = GetComponent<Renderer> ();
		render.enabled = !ConfigManager.Inst.CompanyMode;		
	}
}