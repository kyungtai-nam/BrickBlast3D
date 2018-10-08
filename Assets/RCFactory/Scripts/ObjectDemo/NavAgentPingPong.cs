using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentPingPong : MonoBehaviour 
{
	NavMeshAgent agent;

	List<Vector3> path = new List<Vector3>();
	int dstPos = 0;

	public float pauseDelayMin = 0.3f;
	public float pauseDelayMax = 1.5f;
	float randomDelay = 0f;

	void Awake()
	{
		agent = GetComponent<NavMeshAgent> ();
	}

	void OnEnable()
	{
		randomDelay = Random.Range(pauseDelayMin, pauseDelayMax);

		agent.updatePosition = true;
		agent.updateRotation = false;

		Vector3 dst = transform.position;

		if ( yAxis )
			dst.y += Random.Range((float)-dist, (float)dist);

		if ( xAxis )
			dst.x += Random.Range((float)-dist, (float)dist);

		if ( zAxis ) 
			dst.z += Random.Range((float)-dist, (float)dist);

		path.Clear ();
		path.Add (dst);
		path.Add (transform.position);

		// Debug.LogError(string.Format("NavAgentPingPong:OnEnable() src={0} dst={1}", transform.position, dst));
		dstPos = 0;

		agent.autoBraking = true;
		GotoNextPoint();
	}

	public float dist = 1f;

	public bool yAxis = true;
	public bool xAxis = true;
	public bool zAxis = true;


	float delay = 0f;

	void GotoNextPoint() {
		if (path.Count == 0)
			return;

		delay = 0f;
		agent.destination = path[dstPos];
		dstPos = (dstPos + 1) % path.Count;
	}

	void Update()
	{
		if (agent.remainingDistance < 0.01f) {
			if (delay >= randomDelay) {
				GotoNextPoint ();
				// Debug.Log ("NavAgentPingPong:Update() GotoNextPoint() now pos=" + agent.transform.position);
			} else {
				SendMessage ("PlayAniMove", false, SendMessageOptions.DontRequireReceiver);

				float delta = Time.deltaTime;
				delay += delta;
				// Debug.Log ("NavAgentPingPong:Update() delay=" + delay + " del=" + delta);
			}
		} else {
			SendMessage ("PlayAniMove", true, SendMessageOptions.DontRequireReceiver);
		}
	}
}