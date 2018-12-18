Shader "Custom/Floor" {
	Properties {
		_Color1 ("Color 1", Color) = (1,1,1,1)
		_Color2 ("Color 2", Color) = (0,0,0,0)
		_FadePoint("Fade point", Vector) = (0.5,0.5,0.5,0)
		_MinDistance("Fade min distance", Range(0,1)) = 0.2
		_MaxDistance("Fade max distance", Range(0,1)) = 0.4

		_MainTex ("Texture", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;

		float _MinDistance;
		float _MaxDistance;
		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _FadePoint;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float dist = distance(IN.uv_MainTex, _FadePoint);
			if (dist < _MinDistance) {
				o.Albedo = _Color1;
			} else {
				o.Albedo = lerp(_Color1, _Color2, (dist - _MinDistance) / (_MaxDistance - _MinDistance));
			}

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
