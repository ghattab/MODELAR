// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/OIT/Depth Peeling/Depth Peeling" {
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
		Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Transparent"}
		
		Pass {
			Tags { "LightMode"="ForwardBase" }

			Cull Off
			ZWrite On
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "Lighting.cginc"
			
			fixed4 _Color;
			float _Depth_Weight;
			float _Glossiness;
			float _Alpha;
			float _AlphaFalloff;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			sampler2D _PrevDepthTex;
			
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
				float4 screenPos: TEXCOORD3;
				float depth : TEXCOORD4;
			};

			struct PixelOutput {
				fixed4 col : COLOR0;
				fixed4 depth : COLOR1;
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

				o.screenPos = ComputeScreenPos(o.pos);
				o.depth = COMPUTE_DEPTH_01;
				
				return o;
			}
			
			PixelOutput frag(v2f i) : SV_Target {
				float depth = i.depth;
				float prevDepth = DecodeFloatRGBA(tex2Dproj(_PrevDepthTex, UNITY_PROJ_COORD(i.screenPos)));

				clip(depth - (prevDepth + 0.001 * depth));

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
					specularReflection = _Glossiness * _LightColor0.rgb * pow(max(0.0, dot(H, tangentNormal)), max(_Glossiness * 32, 1));
					
				}

				PixelOutput o;
				o.col = fixed4(specularReflection + ambient + diffuse, clamp(_Alpha - abs(dot(tangentViewDir, tangentNormal)) * _AlphaFalloff, 0.0, 1.0));
				o.depth = EncodeFloatRGBA(i.depth);
				return o;
			}
			
			ENDCG
		}
	}
	FallBack "Diffuse/VertexLit"
}
