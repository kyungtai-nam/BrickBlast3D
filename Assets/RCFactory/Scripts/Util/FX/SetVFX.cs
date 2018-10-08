using UnityEngine;
using System.Collections;

// not used
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(ParticleSystem))]
public class SetVFX : MonoBehaviour{

	// Use this for initialization
	void Start ()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

		if ( null != spriteRenderer )
		{
			GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerID = spriteRenderer.sortingLayerID;
			GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = spriteRenderer.sortingOrder;
			return;
		}

		Debug.LogError("SetVFX null == spriteRenderer");
		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerID = 4000;
		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = 3;
	}

}
