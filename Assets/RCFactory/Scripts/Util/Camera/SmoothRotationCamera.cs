using UnityEngine;
using System.Collections;



public class SmoothRotationCamera : MonoBehaviour
{
	public bool isSmooth = true;


	Vector3 src;
	Vector3 dst;

	public Vector3 Dst
	{
		set {
			dst = value;
		}
	}

	// 낮을수록 천천히 카메라 회전. 높으면 끊겨서 회전 
	public float speed = 10f;

	void Awake()
	{
		src = transform.rotation.eulerAngles;
		dst = transform.rotation.eulerAngles;
	}

	void Update()
	{
		if ( isSmooth ) 
			src = Vector3.Lerp (src, dst, speed * Time.deltaTime);
		else
			src = Vector3.Lerp (src, dst, 1);

		this.transform.eulerAngles = src;
	}
}