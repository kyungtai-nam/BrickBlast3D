using UnityEngine;
using System.Collections;
using System.IO;

public class PreLoaderVfx : MonoBehaviour 
{
	void Start()
	{
		// without resources prefix
		// real
		/*
		Load ("Effect");
		Load ("Effect/VFX");
		*/

		LoadPrefab ("Effect/GG_Skill_01");
		LoadPrefab ("Effect/Hit");
	}
		
	void Load(string path)
	{
		// TODO : list up filename

		string[] files = null;

		for (int n = 0; n < files.Length; n++) {
			LoadPrefab (path + files [n]);
		}
	}
		
	void LoadPrefab(string fullpath)
	{
		GameObject go = Resources.Load<GameObject> (fullpath);

		// find child of particles
		ParticleSystem[] ps = go.GetComponentsInChildren<ParticleSystem> ();
	
		//ParticleSystem ps = go.GetComponent<ParticleSystem>();
		//if (null == ps)
		//	return;

		// TODO : show out of screen

	}
		

	void LoadMaterial()
	{

	}

	void LoadTexture()
	{

	}
	/*
	GameObject obj = Instantiate(prefab) as GameObject;
	obj.SetActive(false);


	ParticleSystem ps = obj.GetComponent<ParticleSystem>();
	if ( null == ps ) 
		continue;

	*/

	// 이펙트에서 사용하는 메터리얼 프리로딩
	void PreloadVFX(GameObject obj)
	{
		Renderer[] rens = obj.GetComponentsInChildren<Renderer>(true);

		for ( int n=0 ; n < rens.Length ; n++ )
		{
			Renderer ren = rens[n];

			if ( ren.sharedMaterials == null )
				continue;

			if ( ren.sharedMaterials.Length <= 0 )
				continue;

			for ( int k=0; k < ren.sharedMaterials.Length ; k++ )
			{
				Material mat = ren.sharedMaterials[k];   
				Texture tex = mat.mainTexture;

				// Debug.Log(string.Format("PreloadVfx={0} tex={1}", obj.name, tex.name));
			}
		}
	}

}
