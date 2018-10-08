using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallTrailTest : MonoBehaviour {
	public int count = 2;

	public float VEL = 15f;
	public float MAX_VEL = 25f;

	List<Ball> list = new List<Ball>();

	void Start () {
		for (int n = 0; n < count; n++) {
			MakeBall (n % 2 == 0 ? VEL : MAX_VEL);
		}
	}

	void MakeBall(float vel)
	{
		Vector2 initPos = Random.insideUnitCircle * 3f;
		initPos.y = 0f;

		Ball ball = Ball.Create (initPos, null);

		Vector3 pos = Random.insideUnitSphere;
		pos.y = 0f;

		ball.VEL_DEFAULT = vel;
		ball.dir = pos.normalized;
		ball.state = Ball.E_STATE.Moving;

		list.Add (ball);
	}
}
