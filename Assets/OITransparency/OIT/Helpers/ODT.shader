﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ODT" {
	Properties {
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_Depth_Weight("Depth weight", Range(0,1)) = 0.0
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Alpha("Alpha Offset", Range(0,1)) = 0.0
		_AlphaFalloff("Alpha Fallof", Range(0,10)) = 0.0
		_MainTex ("Main Tex", 2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		
		Pass {
			Tags { "LightMode"="ForwardBase" }

			//Cull Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "Lighting.cginc"
			#define M_PI 3.1415926535897932384626433832795
			fixed4 _Color;
			float _Depth_Weight;
			float _Glossiness;
			float _Alpha;
			float _AlphaFalloff;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			
			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				float3 lightDir: TEXCOORD0;
				float3 viewDir : TEXCOORD1;
				float2 uv : TEXCOORD2;
				float depth : TEXCOORD3;
			};
			
			v2f vert(a2v v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				TANGENT_SPACE_ROTATION;
				// Transform the light direction from object space to tangent space
				o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex)).xyz;
				// Transform the view direction from object space to tangent space
				o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex)).xyz;
				o.depth = COMPUTE_DEPTH_01;
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				fixed3 tangentLightDir = normalize(i.lightDir);
				fixed3 tangentViewDir = normalize(i.viewDir);
				fixed3 tangentNormal = UnpackNormal(tex2D(_BumpMap, i.uv));

				fixed3 albedo = tex2D(_MainTex, i.uv).rgb * lerp(_Color.rgb, fixed3(0.0, 0.0, 0.0), saturate((i.depth - 0.1) * _Depth_Weight * 5));

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

				fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(tangentNormal, tangentLightDir));

				fixed3 specularReflection;
				if (dot(tangentNormal, tangentLightDir) < 0.0) //Light on the wrong side - no specular
				{
					specularReflection = fixed3(0.0, 0.0, 0.0);
				}
				else
				{
					//Blinn Phong specular lighting
					fixed3 H = normalize(tangentLightDir + tangentViewDir);
					float exp = max(_Glossiness * 32, 1);
					float conservationFactor = (exp + 8.0f) / (8.0f * M_PI);
					specularReflection = conservationFactor * _LightColor0.rgb * pow(max(0.0, dot(H, tangentNormal)), exp) * 0.3f;
				}
				
				return fixed4(specularReflection + ambient + diffuse, clamp(_Alpha - abs(dot(tangentViewDir, tangentNormal)) * _AlphaFalloff, 0.0, 1.0));
			}
			
			ENDCG
		}
	}
	FallBack "Transparent/VertexLit"
}
