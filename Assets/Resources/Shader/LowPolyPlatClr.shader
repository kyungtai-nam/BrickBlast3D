Shader "RetroCell/LowPolyPlatClr" {
	// Lowpoly + Color + Light
	Properties {
		_TopColor("Top Color", Color) = (0.431, 0.976, 0, 1)
		_LeftColor("Left Color", Color) = (0, 0.647, 0.643, 1)
		_RightColor("Right Color", Color) = (0.329, 0.584, 1, 1)        
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 2.0

		#define TOP float3(0,1,0)
      	#define RIGHT float3(0,0,1)
      	#define LEFT float3(1,0,0)

		fixed4 _TopColor;
		fixed4 _LeftColor;
		fixed4 _RightColor;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {

			half3 clr;

			if (o.Normal.x == 1)
			{
				clr = _LeftColor.rgb * _LeftColor.a;
			}
			else if (o.Normal.y == 1)
			{
				clr = _TopColor.rgb * _TopColor.a;
			}
			else
			{
				clr = _RightColor.rgb * _RightColor.a;
			}

			o.Albedo = clr;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
