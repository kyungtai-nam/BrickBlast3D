using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 


public class Global : GSingleton<Global> 
{    
    public const string MAC_DEV_PC_ID = "7C899970-EC1C-529C-8F8C-32930CDE66B6";


	/////////////////////////////////////////////////////////////////
	// Admob (배너, 전면)

	// 홈화면 배너 
	public const string ADMOB_BANNER 		= "ca-app-pub-9246891224753926/9808998774";

    // 결과창 나오고 다음으로 넘어갈때 랜덤하게
	public const string ADMOB_FULL_BANNER 	= "ca-app-pub-9246891224753926/3762465177";

	// 애드몹은 보상형 video (for service)
	public const string ADMOB_REWARD_VIDEO = "ca-app-pub-9246891224753926/5239198373";


	// only adcolony
	public const string ADMOB_REWARD_VIDEO_ADCOLONY = "ca-app-pub-9246891224753926/9883587179";

	// adcolony vcvc
	public const string ADMOB_REWARD_VIDEO_ADCOLONY_V4VC = "ca-app-pub-9246891224753926/5813314370";

	// vungle
	public const string ADMOB_REWARD_VIDEO_VUNGLE = "ca-app-pub-9246891224753926/3998503972";

	// unity ads
	public const string ADMOB_REWARD_VIDEO_UNITY_ADS = "ca-app-pub-9246891224753926/2360320372";



    // 테스트용 디바이스 설정 (GPRO)
    public const string ADMOB_TEST_DEVICE_ID_GPRO 	= "661849E5D9F2FD6CCAA43F1531ABCD9B";

    // 테스트용 디바이스 설정 (넥서스)
    public const string ADMOB_TEST_DEVICE_ID_NEXUS	= "DEE11C43007B90BE4D3D8531DB94622A";

	/////////////////////////////////////////////////////////////////
	// AdColony (Test)
	public const string ADCOLONY_APP_ID = "app2080dc3c8aa94ba1a9";
	public const string ADCOLONY_ZONE = "vz79405a097542462e8b";



	/////////////////////////////////////////////////////////////////
	// Rate

	public const string PRJ_TITLE_KR = "블럭블럭해"; 
	public const string PRJ_TITLE_EN = "Iso Brick"; 
	public const string PRJ_MESSAGE_FMT = "잠시만 시간을 내서 {0} 평가를 남겨주세요.";

	/////////////////////////////////////////////////////////////////
	// Share

	public const string SHARE_URL = "https://play.google.com/store/apps/details?id=com.retrocellstudio.bricks";



	/////////////////////////////////////////////////////////////////
	// UI

	// easing
	public const float UI_MOVE_DISTANCE = 300f;
	public const float UI_MOVE_TIME = 0.5f;


	public readonly Vector3 UI_MOVE_TOP = new Vector3 (0f, 400f, 0f);
	public readonly Vector3 UI_MOVE_TOP_FAR = new Vector3 (0f, 600f, 0f);
    public readonly Vector3 UI_MOVE_BOTTOM = new Vector3 (0f, -600f, 0f);
    public readonly Vector3 UI_MOVE_BOTTOM_FAR = new Vector3 (0f, -600f, 0f);
    public readonly Vector3 UI_MOVE_LEFT = new Vector3 (-400f, 0f, 0f);
    public readonly Vector3 UI_MOVE_RIGHT = new Vector3 (400f, 0f, 0f);


    // button BG Color

	public readonly Color BTN_COLOR_BG = new Color(245f/255f, 102f/255f, 43f/255f); // orange
    // public readonly Color BTN_COLOR_BG = new Color(103f/255f, 136f/255f, 255f/255f); // blue
	//public readonly Color BTN_COLOR_BG = new Color(39f/255f, 210f/255f, 255f/255f); // white blue
	//public readonly Color BTN_COLOR_BG = new Color(255f/255f, 10f/255f, 10f/255f); // red


	// 상자 까는데 필요한 다이아
	public const int CHEST_PRICE = 100;

    // 광고 보상 다이아
    public const int AD_REWARD_GEM = 30;

    // 아이콘 중복일 때 보상 다이아
    public const int ICON_DUP_GEM = 20;

}