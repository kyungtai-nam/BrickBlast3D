using UnityEngine;
using System.Collections;

public class UI2DSpriteAni : MonoBehaviour {
    public Sprite[] raw;
    public UI2DSprite spr;

    int pos = 0;

	public bool forward = true;
    public float delay = 0.05f;

	public bool isPlaying = false;

	public System.Action cbDone = null;

	public void Play(System.Action cbDone=null)
    {
		isPlaying = true;
		this.cbDone = cbDone;

        if ( forward )
            pos = 0;
        else
            pos = raw.Length - 1;


		StartCoroutine ("PlayAnimation");
    }

    public void Stop()
    {
		StopCoroutine ("PlayAnimation");       

		if ( forward ) 
			spr.sprite2D = raw [0];
		else
			spr.sprite2D = raw [raw.Length-1];
    }

    IEnumerator PlayAnimation()
    {
        while(true)
        {
			spr.sprite2D = raw [pos];            

            if ( forward ) 
            {
                if ( pos++ >= raw.Length-1 )
                    break;
            }
            else
            {
                if ( pos-- < 0 )
                    break;
            }

            yield return new WaitForSeconds(delay);
        }

		isPlaying = false;

        if ( null != cbDone ) 
            cbDone();
		
    }
}
