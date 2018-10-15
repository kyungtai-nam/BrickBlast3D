using UnityEngine;
using System.Collections;
using VoxelBusters.NativePlugins;
using VoxelBusters.Utility;

public class RcShare : GSingletonMono<RcShare> 
{
/*	eShareOptions m_excludedOptions;

    void Awake()
    {
        m_excludedOptions = eShareOptions.MESSAGE;
    }
*/

    //kyungtai : code changed --> need to check reference if it needs
    //VoxelBusters.NativePlugins.RateMyApp.Settings settings = new VoxelBusters.NativePlugins.RateMyApp.Settings();

    public void Init ()
	{
        /*
		Debug.Log ("1=" + NPBinding.Utility.RateMyApp.rateMyAppSettings.ShowFirstPromptAfterHours); // 2
		Debug.Log ("2=" + NPBinding.Utility.RateMyApp.rateMyAppSettings.SuccessivePromptAfterHours); // 6
		Debug.Log ("3=" + NPBinding.Utility.RateMyApp.rateMyAppSettings.SuccessivePromptAfterLaunches); // 5
		*/

        //kyungtai : code changed --> need to check reference if it needs
        NPSettings.Utility.RateMyApp.Title = Localization.Get ("title");
        NPSettings.Utility.RateMyApp.Message = Localization.Get ("rate_msg");
        NPSettings.Utility.RateMyApp.DontAskButtonText = Localization.Get ("rate_dontask");
        NPSettings.Utility.RateMyApp.RemindMeLaterButtonText = Localization.Get("rate_later");
        NPSettings.Utility.RateMyApp.RateItButtonText = Localization.Get("rate_it");


        NPSettings.Utility.RateMyApp.ShowFirstPromptAfterHours = 1;
        NPSettings.Utility.RateMyApp.SuccessivePromptAfterHours = 2;
        NPSettings.Utility.RateMyApp.SuccessivePromptAfterLaunches = 3;
	    
		//NPBinding.Utility.RateMyApp.rateMyAppSettings = settings;
        
	}

	public void ShareURL(string message, string url)
	{
	    // Create share sheet
	    ShareSheet _shareSheet = new ShareSheet();    
	    _shareSheet.Text = message;
	    _shareSheet.URL = url;

	    // Set this list if you want to exclude any service/application type. Else, ignore.
	    // _shareSheet.ExcludedShareOptions    = m_excludedOptions;

	    // Show composer at last touch point
	    NPBinding.UI.SetPopoverPointAtLastTouchPosition();
		NPBinding.Sharing.ShowView(_shareSheet, OnFinishedSharing);
	}

    void OnFinishedSharing (eShareResult _result)
    {
        // Insert your code
    }


	// right now!
    public void RateRequest()
    {
		Init ();
    	NPBinding.Utility.RateMyApp.AskForReviewNow();
    }

	public void RateRequstSometimes()
	{		
		NPBinding.Utility.RateMyApp.AskForReview ();
	}
}

