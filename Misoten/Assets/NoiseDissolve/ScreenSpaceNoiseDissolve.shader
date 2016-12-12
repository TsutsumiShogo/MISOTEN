Shader "Custom/ScreenSpaceNoiseDissolve" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

		[MaterialToggle]
		_UseMetallicTex ("UseMetallicTex", Float) = 0
		_MetallicTex ("Metallic", 2D) = "white" {}
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Glossiness ("Smoothness", Range(0,1)) = 0.5

		_NoiseSize ("NoiseSize", Float) = 1
		_DissolveDepth ("DissolveDepth", Range(0,1)) = 0.12
		_DissolveSize ("DissolveSize", Range(0,1)) = 1.0
		_RimSize ("RimSize", Range(0,1)) = 1.0
		_RimColor ("RimColor", Color) = (0,0,0,1)
		_RimIntensity ("RimIntensity", Float) = 3.0
		[MaterialToggle]
		_EnhancedRimColor ("EnhancedRimColor", Float) = 1
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
		float _UseMetallicTex;
		sampler2D _MetallicTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		float _NoiseSize;
		float _DissolveDepth;
		float _DissolveSize;
		float _RimSize;
		float4 _RimColor;
		float _RimIntensity;
		float _EnhancedRimColor;

		float rand(float3 pos)
		{
			return frac(sin(dot(pos.xyz, float3(12.9898, 78.233, 56.787))) * 23758.5453);
		}

		float volumeNoise(float3 pos)
		{
			pos *= _NoiseSize;
			float3 ip = floor(pos);
			float3 fp = smoothstep(0, 1, frac(pos));
			float4 a = float4(
				rand(ip + float3(0, 0, 0)),
				rand(ip + float3(1, 0, 0)),
				rand(ip + float3(0, 1, 0)),
				rand(ip + float3(1, 1, 0)));
			float4 b = float4(
				rand(ip + float3(0, 0, 1)),
				rand(ip + float3(1, 0, 1)),
				rand(ip + float3(0, 1, 1)),
				rand(ip + float3(1, 1, 1)));
			a  = lerp(a, b, fp.z);
			a.xy = lerp(a.xy, a.zw, fp.y);
			return lerp(a.x, a.y, fp.x);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;

			// Metallic and smoothness come from slider variables
			if(_UseMetallicTex == true){
				float4 metal = tex2D (_MetallicTex, IN.uv_MainTex);
				o.Metallic = metal.r;
				o.Smoothness = metal.a * _Glossiness;
			} else {
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
			}
			o.Alpha = c.a;

			// Depth
			float depth = (IN.screenPos.w - _ProjectionParams.y) / (_ProjectionParams.z - _ProjectionParams.y);
			if(depth < _DissolveDepth){
				float2 uv = IN.screenPos.xy / IN.screenPos.w;
				float depthMag = smoothstep(_DissolveDepth, 0, depth);

				// Vignette
				float vignetteInv = clamp(_DissolveSize * length(uv - 0.5f), 0, 1);
				vignetteInv = smoothstep(vignetteInv, 0, depthMag);
				float vignette = lerp(1, 0, vignetteInv);

				// Noise
				float noise = volumeNoise(IN.worldPos.xyz);
				noise = lerp(noise, 0, depthMag);
				noise = noise - vignette;

				// RimLight
				if(noise < _RimSize){
					if(_EnhancedRimColor == true){
						float3 rimColorInv = float3(1, 1, 1) - _RimColor.rgb;
						o.Albedo -= rimColorInv* _RimColor.a;
					} else {
						o.Albedo += _RimColor.rgb * _RimColor.a;

					}
					o.Emission += _RimColor.rgb * _RimColor.a * _RimIntensity;
				}

				// Clip
				clip(noise);
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
	CustomEditor "ScreenSpaceNoiseDissolve"
}
