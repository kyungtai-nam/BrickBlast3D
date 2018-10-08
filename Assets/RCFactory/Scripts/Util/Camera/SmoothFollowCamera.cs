using UnityEngine;
using System.Collections;



public class SmoothFollowCamera : MonoBehaviour
{
	public Transform target;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
//	private Player player;
//	private CharacterController2D _playerController;
	private Vector3 _smoothDampVelocity;
	
	public bool active = true;

	// fast re position without damp
	public bool fastTracing = true;
	
	void Awake()
	{
		transform = gameObject.transform;
//		transform.position.z = -10f;
//		_playerController = target.GetComponent<CharacterController2D>();
	}
	
	public void SetActive(bool _active)
	{
		active = _active;
	}
	
	void LateUpdate()
	{
		if( !useFixedUpdate )
			updateCameraPosition();
	}


	void FixedUpdate()
	{
		if( useFixedUpdate )
			updateCameraPosition();
	}


	void updateCameraPosition()
	{
		if ( !active ) 
			return;

		if (fastTracing) {
			transform.position = target.position - cameraOffset;
			return;
		}

		Vector3 tempPos = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
		// tempPos.z = -0.01f;
		transform.position = tempPos;



/*
		if( _playerController == null )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
			return;
		}
		
		if( _playerController.velocity.x > 0 )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
		}
		else
		{
			var leftOffset = cameraOffset;
			leftOffset.x *= -1;
			transform.position = Vector3.SmoothDamp( transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime );
		}
*/
	}
	
}
