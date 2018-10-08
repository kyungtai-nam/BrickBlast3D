using UnityEngine;
using System.Collections;

public class RcAd
{
	protected System.Action<bool> cbInit = null;
	protected System.Action<bool> cbVideo = null;

	protected bool isReadyVideo = false;

	public virtual bool Init()
	{
		
		return false;
	}

	public virtual bool ShowBanner()
	{
		return false;
	}

	public virtual void CloseBanner()
	{
		
	}

	public virtual bool ShowFullBanner()
	{
		return false;
	}

	public virtual bool ShowVideo(System.Action<bool> cbVideo)
	{
		this.cbVideo = cbVideo;
		return false;
	}

	public virtual bool IsReadyVideo()
	{
		return isReadyVideo;
	}
}
