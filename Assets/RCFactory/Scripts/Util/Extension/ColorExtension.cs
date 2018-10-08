using UnityEngine;

public static class ColorExtension {

	public static Color SetR(this Color src, float v=0f)
	{
		src.r = v;
		return src;
	}

	public static Color SetG(this Color src, float v=0f)
	{
		src.g = v;
		return src;
	}

	public static Color SetB(this Color src, float v=0f)
	{
		src.b = v;
		return src;
	}

	public static Color SetA(this Color src, float v=0f)
	{
		src.a = v;
		return src;
	}

}