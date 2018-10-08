using UnityEngine;
using System.Collections;

public class DemoBall : MonoBehaviour {
	Ball ball = null;

	void OnEnable()
	{
		// Debug.Log ("DemoBall:OnEnable()");

		ball = Ball.Create (Vector3.zero, null);

		Vector3 dir = new Vector3(Random.Range(-1f,1f), 0f, Random.Range(-1f,1f));
		ball.dir = dir.normalized;
		ball.state = Ball.E_STATE.Moving;

		ball.Reset ();
	}

	void OnDisable()
	{
		// Debug.Log ("DemoBall:OnDisable()");

		GPoolManager.Inst.Delete (ball.gameObject);
		ball = null;
	}
}
