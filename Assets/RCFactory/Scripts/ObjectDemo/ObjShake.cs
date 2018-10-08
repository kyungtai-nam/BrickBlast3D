using UnityEngine;
using System.Collections;

public class ObjShake : MonoBehaviour {
	
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform tm;

	// How long the object should shake for.
	public float shake = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	public Vector3 originalPos;
	public float shake_init = 0.2f;

	// 피격시 흔들기
	public void OnShake()
	{
		if ( shake > 0.0f )
			return;

		// originalPos = tm.localPosition;
		shake = shake_init;
		//shakeAmount = 5f;
		// decreaseFactor = 2f;
	}

	// 강하게 흔들기
	public void OnPowerShake()
	{
		originalPos = tm.localPosition;
		shake = 1.0f;

		shakeAmount = 6f;
		decreaseFactor = 3f;
	}

	void Start()
	{
		originalPos = tm.localPosition;
	}

	void FixedUpdate()
	{
		if (shake > 0)
		{
			tm.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			shake -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shake = 0f;
			tm.localPosition = originalPos;
		}
	}

}

