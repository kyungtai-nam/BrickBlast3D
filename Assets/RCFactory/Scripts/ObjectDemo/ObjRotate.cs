using UnityEngine;
using System.Collections;

public class ObjRotate : MonoBehaviour 
{
	Quaternion rot;
	float timer;
	public float rotateSpeed = 1f;
	public float speed = 1f;
	//public float delay;

	void Awake()
	{
		
	}

	void Start() 
	{
		// rot = Quaternion.LookRotation(-transform.forward, Vector3.up);

		rot = Quaternion.Euler(new Vector3(Random.Range(-180.0f, 180.0f),Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f)));
	}

	void Update() 
	{
		timer += Time.deltaTime;

		if(timer > 2) 
		{ // timer resets at 2, allowing .5 s to do the rotating
			rot = Quaternion.Euler(new Vector3(Random.Range(-180.0f, 180.0f),Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f)));
			timer = 0.0f;
		}
		transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, Time.deltaTime * rotateSpeed);
		// transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
}

