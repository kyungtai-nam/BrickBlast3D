using UnityEngine;

public static class VectorExtension 
{
	public static Vector2 SetX(this Vector2 v2, float v = 0f)
	{
		v2.x = v;
		return v2;	
	}

	public static Vector2 SetY(this Vector2 v2, float v = 0f)
	{
		v2.y = v;
		return v2;	
	}


	public static Vector3 SetX(this Vector3 v3, float v = 0f)
	{
		v3.x = v;
		return v3;	
	}

	public static Vector3 SetY(this Vector3 v3, float v = 0f)
	{
		v3.y = v;
		return v3;	
	}

	public static Vector3 SetZ(this Vector3 v3, float v = 0f)
	{
		v3.z = v;
		return v3;
	}
}