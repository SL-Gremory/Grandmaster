// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//world position transformations from https://github.com/chriscummings100/worldspaceposteffect this is magic
// well those transformations didn't really help with ortographic camera, had to butcher it

Shader "Fog Of War"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "Noise/HLSL/SimplexNoise3D.hlsl"

			// Provided by our script
			uniform float4x4 _FrustumCornersES;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;
			uniform float3 _CameraDir;
			uniform sampler2D _CameraDepthTexture;
			uniform sampler2D _VisHeightMap;
			uniform float4 _LevelSize;
			uniform float4 _ScreenResolution;

			// Input to vertex shader
			struct appdata
			{
				// Remember, the z value here contains the index of _FrustumCornersES to use
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			// Output of vertex shader / input to fragment shader
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 origin : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;

				// Index passed via custom blit function in RaymarchGeneric.cs
				half index = v.vertex.z;
				v.vertex.z = 0.1;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
				#endif

				o.origin = _FrustumCornersES[(int)index].xyz;

				return o;
			}

			float2 pixelize(float2 uv) {
				return uv;// -uv % 4 / _ScreenResolution.xy + 2 / _ScreenResolution.xy;
			}

			float3 Hue(float H)
			{
				float R = abs(H * 6 - 3) - 1;
				float G = 2 - abs(H * 6 - 2);
				float B = 2 - abs(H * 6 - 4);
				return saturate(float3(R, G, B));
			}

			float3 HSVtoRGB(in float3 HSV)
			{
				return float3(((Hue(HSV.x) - 1) * HSV.y + 1) * HSV.z);
			}

			fixed4 raymarch(float3 ro, float3 rd, float maxDist) {
				float4 ret = fixed4(0, 0, 0, 0);

				const int maxstep = 16;
				float t = 0; // current distance traveled along origin
				for (int i = 0; i < maxstep; ++i) {/*
					if (t >= maxDist) {
						break;
					}*/
					float3 p = ro + rd * t; // World space position of sample
					float4 visHeight = tex2D(_VisHeightMap, pixelize(p.xz / _LevelSize.xz));
					float d = p.y - (visHeight.b*(_LevelSize.y + 5) - 5) - 1.5;
					//return float4(HSVtoRGB(float3(visHeight.r, visHeight.a, 1)),1);
					// If the sample <= 0, we have hit something (see map()).
					if (d < 0.001) {

						p.x += _SinTime.x; // sin time/8
						p.y += _SinTime.y; // sin time/4
						//p = p - p % 1.0 / 32.0;
						float noiseScale = (visHeight.g + 0.25)*(visHeight.g + 0.25);
						float lum = 0.75 + snoise(noiseScale*0.1*p)*0.17 + snoise(noiseScale*p)*0.09;
						ret += float4(HSVtoRGB(float3(visHeight.r, visHeight.a, lum)), 0) *-d;
						ret.rgb += _LightColor0.rgb*clamp(dot(snoise_grad(noiseScale*p).rgb, _WorldSpaceLightPos0.rgb),0,1) * -d * 0.1;
						ret.a += -d;
						//break;
					}

					t += maxDist / (maxstep);
				}
				ret.rgb = clamp(ret.rgb / max(1, ret.a),0,1);
				return clamp(fixed4(ret.rgb, ret.a*0.35),0,1);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// origin direction
				float3 rd = normalize(_CameraDir);//normalize(i.origin.xyz);
				// origin origin (camera position)
				float3 ro = i.origin;
				//ro = ro - ro % 4 / _ScreenResolution.y + 2 / _ScreenResolution.y;

				float2 duv = i.uv;
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					duv.y = 1 - duv.y;
				#endif

				float depth = 1 - (tex2D(_CameraDepthTexture, pixelize(duv)).r);
				depth = depth * 100 + 0.5; // near and far clip planes
				fixed3 col = tex2D(_MainTex, i.uv); // Color of the scene before this shader was run
				fixed4 add = raymarch(ro + rd * (depth - 10), rd, 10);
				//add.rgb = add.rgb - add.rgb % (1 / 32.0) + 1/64.0;

				// Returns final color using alpha blending
				return fixed4(col*(1.0 - add.w) + add.xyz * add.w, 1.0);
			}



			ENDCG
		}
	}
}
