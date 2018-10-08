Shader "RetroCell/LowPolyPlatClrUnlit" {
	// Lowpoly + Color - Light

	// simple cube shader : https://forum.unity3d.com/threads/simple-cube-shader.313644/
	Properties
    {
    	// solid color
    	_TopColor("Top Color", Color) = (0.431, 0.976, 0, 1)
		_LeftColor("Left Color", Color) = (0, 0.647, 0.643, 1)
		_RightColor("Right Color", Color) = (0.329, 0.584, 1, 1)        
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

        Lighting Off
       
        Pass
        {        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "UnityCG.cginc"


			fixed4 _TopColor;
			fixed4 _LeftColor;
			fixed4 _RightColor;

			struct appdata_t {
				float4 vertex : POSITION;
				float3 normal : TEXCOORD1;
			};

            struct v2f {
                 fixed4 pos : SV_POSITION;
                 float3 norm : TEXCOORD0;
            };

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.norm = abs(v.normal);
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 clr;

				if (i.norm.x == 1)
				{
					clr = _LeftColor;
				}
				else if (i.norm.y == 1)
				{
					clr = _TopColor;
				}
				else
				{
					clr = _RightColor;
				}

				return clr;
			}
			ENDCG
        }
    }
    FallBack "Diffuse"


}
