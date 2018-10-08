Shader "RetroCell/LowPolyPlatClrTex" {
	// Lowpoly + Color + Texture + Light
	// 조명 각도에 따라 안보일 수도 있음 -130도가 적당

	Properties {
		_TopColor("Top Color", Color) = (0.431, 0.976, 0, 1)
		_LeftColor("Left Color", Color) = (0, 0.647, 0.643, 1)
		_RightColor("Right Color", Color) = (0.329, 0.584, 1, 1)        

		_TopTex ("Top Albedo (RGB)", 2D) = "white" {}
		_TopTexColor ("Up Color", Color) = (1,1,1,0)
		_LeftTex ("Left Albedo (RGB)", 2D) = "white" {}
		_LeftTexColor ("Left Color", Color) = (1,1,1,0)
		_RightTex ("Right Albedo (RGB)", 2D) = "white" {}
		_RightTexColor ("Right Color", Color) = (1,1,1,0)
	}
	SubShader {
		Tags { 
			"RenderType"="Opaque" 
		}

		LOD 200
	
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows alpha
		// #pragma surface surf Standard keepalpha alpha
		#pragma surface surf Standard

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 2.0

		#define TOP float3(0,1,0)
      	#define RIGHT float3(0,0,1)
      	#define LEFT float3(1,0,0)

		struct Input {
			float2 uv_TopTex;
			float2 uv_LeftTex;
			float2 uv_RightTex;
		};

		fixed4 _TopColor;
		fixed4 _LeftColor;
		fixed4 _RightColor;

		sampler2D _TopTex;
		sampler2D _LeftTex;
		sampler2D _RightTex;

		fixed4 _TopTexColor;
		fixed4 _LeftTexColor;
		fixed4 _RightTexColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			fixed4 top = tex2D (_TopTex, IN.uv_TopTex) * _TopTexColor;
			fixed4 left = tex2D(_LeftTex, IN.uv_LeftTex) * _LeftTexColor;
			fixed4 right = tex2D(_RightTex, IN.uv_RightTex) * _RightTexColor;


			half3 finalColor = _TopColor.rgb * max(0,dot(o.Normal, TOP)) * _TopColor*(1-top.a) + max(0,dot(o.Normal, TOP))*(top*(top.a));
			finalColor += _LeftColor.rgb * max(0,dot(o.Normal, LEFT)) * _LeftColor*(1-left.a) + max(0,dot(o.Normal, LEFT))*(left*(left.a));
			finalColor += _RightColor.rgb * max(0,dot(o.Normal, RIGHT)) * _RightColor*(1-right.a) + max(0,dot(o.Normal, RIGHT))*(right*(right.a));

			o.Albedo = finalColor;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"

}