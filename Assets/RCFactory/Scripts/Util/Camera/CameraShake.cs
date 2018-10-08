using UnityEngine;
using System.Collections;


public class CameraShake : MonoBehaviour {


/*
http://newbquest.com/2014/06/the-art-of-screenshake-with-unity-2d-script/

To use this script:

Assets > Create > C# Script
Name the new script “CamShakeSimple” (as in the class name)
Add CamShakeSimple to the game object that will cause the shake, in this case it’s setup to shake the camera when this 2D object collides with something.
Drag your main camera into the newly visible slot in the inspector called “Main Camera”
Now when this object hits another it will use the relative velocity of that collision to shake the camera.  Other ideas might include shaking the cam when you fire a bullet, fall from high up, or any number of things.
Note that if you’re using this in 3D you’ll want to remove the two instances of “2D” from the collision portion.


*/

/*
    Vector3 originalCameraPosition;

    float shakeAmt = 0;

    public Camera mainCamera;

    void OnCollisionEnter2D(Collision2D coll) 
    {
        shakeAmt = 1f ;//coll.relativeVelocity.magnitude * 10.5f;
        InvokeRepeating("CameraShaking", 0, .01f);
        Invoke("StopShaking", 0.3f);

    }

    void CameraShaking()
    {
        if(shakeAmt>0) 
        {
            float quakeAmt = 2 - 0; //Random.value*shakeAmt*0.1f - shakeAmt;
            Vector3 pp = mainCamera.transform.localPosition;
            pp.y+= quakeAmt; // can also add to x and/or z
            mainCamera.transform.localPosition = pp;
        }
    }

    void StopShaking()
    {
        CancelInvoke("CameraShake");
        mainCamera.transform.localPosition = originalCameraPosition;
    }
*/

// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	
	// How long the object should shake for.
	public float shake = 0f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
	
	public Vector3 originalPos;
	
	/*
	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
	*/

	// 피격시 흔들기
	public void OnShake()
	{
		if ( shake > 0.0f )
			return;

		originalPos = camTransform.localPosition;
		shake = 1.0f;

		shakeAmount = 6f;
		decreaseFactor = 3f;	
	}

	// 강하게 흔들기
	public void OnPowerShake()
	{
		originalPos = camTransform.localPosition;
		shake = 1.0f;

		shakeAmount = 6f;
		decreaseFactor = 3f;
	}

	void Start()
	{
		originalPos = camTransform.localPosition;
	}

	void FixedUpdate()
	{
		if (shake > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			shake -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shake = 0f;
			camTransform.localPosition = originalPos;
		}
	}

}
