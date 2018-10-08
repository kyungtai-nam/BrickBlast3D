Shader "RetroCell/TrianglePlat" {
	// Lowpoly + Color + Texture - Light

	// simple cube shader : https://forum.unity3d.com/threads/simple-cube-shader.313644/
	Properties
    {
    	// solid color
    	_TopColor("Top Color", Color) = (0.431, 0.976, 0, 1)
		_LeftColor("Left Color", Color) = (0, 0.647, 0.643, 1)
		_RightColor("Right Color", Color) = (0.329, 0.584, 1, 1)        

		// texture
		_TopTex ("Top Albedo (RGB)", 2D) = "white" {}
		_TopTexColor ("Up Color", Color) = (1,1,1,0)
		_LeftTex ("Left Albedo (RGB)", 2D) = "white" {}
		_LeftTexColor ("Left Color", Color) = (1,1,1,0)
		_RightTex ("Right Albedo (RGB)", 2D) = "white" {}
		_RightTexColor ("Right Color", Color) = (1,1,1,0)
    }
    SubShader
    {
    	// isometric 에서 Transparent 쓰면 z-order 문제로 색이 변하는 경우가 생김
    	/*
        Tags
        {
        	"Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
        */
        Tags { "RenderType"="Opaque" }

        Lighting Off
       	//Blend SrcAlpha OneMinusSrcAlpha
       	Blend One OneMinusSrcAlpha

        Pass
        {        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "UnityCG.cginc"


			fixed4 _TopColor;
			fixed4 _LeftColor;
			fixed4 _RightColor;

			sampler2D _TopTex;
			sampler2D _LeftTex;
			sampler2D _RightTex;

			fixed4 _TopTexColor;
			fixed4 _LeftTexColor;
			fixed4 _RightTexColor;

			float4 _TopTex_ST;
			float4 _LeftTex_ST;
			float4 _RightTex_ST;


			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : TEXCOORD1;
			};

            struct v2f {
                 fixed4 pos : SV_POSITION;
                 float3 norm : TEXCOORD0;
                 // is this right?
                 fixed2 uvT : TEXCOORD1;
                 fixed2 uvL : TEXCOORD2;
                 fixed2 uvR : TEXCOORD3;
            };

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.norm = normalize(abs(v.normal));

				o.uvT = TRANSFORM_TEX(v.texcoord, _TopTex);
				o.uvL = TRANSFORM_TEX(v.texcoord, _LeftTex);
				o.uvR = TRANSFORM_TEX(v.texcoord, _RightTex);
				return o;
			}

			fixed4 frag (v2f i) : COLOR
			{
				fixed4 clr;
				fixed4 tex;

				if ( i.norm.y > 0.5 ) 
				{
					clr = _TopColor;
					tex = tex2D(_TopTex, i.uvT) * _TopTexColor;
				}
				else if (i.norm.x > 0.5 )
				{
					clr = _LeftColor;
					tex = tex2D(_LeftTex, i.uvL) * _LeftTexColor;
				}
				else 
				{
					clr = _RightColor;
					tex = tex2D(_RightTex, i.uvR) * _RightTexColor;
				}

				fixed4 res = clr.rgba + (tex * tex.a);
				return res;
			}
			ENDCG
        }
    }
    FallBack "Diffuse"
}
