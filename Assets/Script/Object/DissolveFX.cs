using UnityEngine;
using System.Collections;

public class DissolveFX : MonoBehaviour {
	BoxCollider m_coll = null;
	MeshCollider m_collMesh = null;

	Renderer m_render = null;
	float delay = 1.5f;

	BoxCollider coll {
		get {
			if ( null == m_coll )
				m_coll = GetComponent<BoxCollider> ();
			return m_coll;
		}
	}

	MeshCollider collMesh {
		get {
			if ( null == m_collMesh )
				m_collMesh = GetComponent<MeshCollider> ();
			return m_collMesh;
		}
	}


	public Renderer render {
		get {
			if (null == m_render)
				m_render = GetComponent<Renderer> ();
			return m_render;
		}
	}

	void Awake()
	{
		Reset ();
	}

	void SetCollider(bool enable)
	{
		if (null != coll) {
			coll.enabled = enable;
			return;
		}

		if (null != collMesh) {			
			collMesh.enabled = enable;
			return;
		}

		Debug.Assert (false, "not found collider");
	}

	public void Reset()
	{
		SetCollider (true);
		render.material.SetFloat ("_DisVal", 0f);
	}

	public void Play(System.Action cb)
	{
		SetCollider (false);
		StartCoroutine (PlayDissolve (cb));
	}

	IEnumerator PlayDissolve(System.Action cb)
	{
		float t = 0f;

		while (true) {
			t += Time.deltaTime;

			float ratio = t / delay;
			render.material.SetFloat ("_DisVal", Mathf.Lerp (0f, 1f, ratio));

			if (ratio >= 1.0f)
				break;

			yield return null;
		}
	
		if (null != cb)
			cb ();
	}
}