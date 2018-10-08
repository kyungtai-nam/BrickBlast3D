using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class FXAutoRelease : MonoBehaviour
{
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}
	
	IEnumerator CheckIfAlive ()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.5f);
			if(!GetComponent<ParticleSystem>().IsAlive(true))
			{
				//Debug.Log("FXAutoRelease:CheckIfAlive() Release!");
				GPoolManager.Inst.Delete(this.gameObject);
				// ObjectPoolManager.Release(this.gameObject);
				break;
			}
		}
	}
}
