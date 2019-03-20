// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//world position transformations from https://github.com/chriscummings100/worldspaceposteffect this is magic

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
			#include "Noise/HLSL/SimplexNoise3D.hlsl"

			// Provided by our script
			uniform float4x4 _FrustumCornersES;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;
			uniform float4x4 _CameraInvViewMatrix;
			uniform float3 _CameraWS;
			uniform float3 _CameraDir;
			uniform sampler2D _CameraDepthTexture;
			uniform sampler2D _VisHeightMap;
			uniform float4 _LevelSize;

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
				float3 ray : TEXCOORD1;
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

				// Get the eyespace view ray (normalized)
				o.ray = _FrustumCornersES[(int)index].xyz;
				// Dividing by z "normalizes" it in the z axis
	// Therefore multiplying the ray by some number i gives the viewspace position
	// of the point on the ray with [viewspace z]=i
				//o.ray /= abs(o.ray.z);

				// Transform the ray from eyespace to worldspace
				//o.ray = mul(unity_ObjectToWorld, UnityObjectToClipPos(o.ray));
				return o;
			}
			/*
			float terrainDist(float3 pos) {
				return pos.y - tex2D(_VisHeightMap, pos.xz / _LevelSize.xz).g*_LevelSize.y;
			}*/

			// Raymarch along given ray
			// ro: ray origin
			// rd: ray direction
			fixed4 raymarch(float3 ro, float3 rd, float maxDist) {
				float4 ret = fixed4(0, 0, 0, 0);

				const int maxstep = 16;
				float t = 0; // current distance traveled along ray
				for (int i = 0; i < maxstep; ++i) {/*
					if (t >= maxDist) {
						break;
					}*/
					float3 p = ro + rd * t; // World space position of sample
					float3 visHeight = tex2D(_VisHeightMap, p.xz / _LevelSize.xz);
					//float d = p.y - visHeight.g*_LevelSize.y;
					float d = p.y - visHeight.g*_LevelSize.y - 1.5;

					// If the sample <= 0, we have hit something (see map()).
					if (d < 0.001) {
						// Simply return a gray color if we have hit an object
						// We will deal with lighting later.
						p.x += _SinTime.x; // sin time/8
						p.y += _SinTime.y; // sin time/4
						//p = p - p % 1;
						float lum = snoise(0.03*p)*0.8 + snoise(0.3*p)*0.2;
						ret += float4(lum, lum, lum, 0) * visHeight.r * -d;//fixed4(t/(float)(maxDist), 0.5, 0.5, 1);
						ret.a += visHeight.r * -d;
						//break;
					}

					// If the sample > 0, we haven't hit anything yet so we should march forward
					// We step forward by distance d, because d is the minimum distance possible to intersect
					// an object (see map()).
					//t += d;
					t += maxDist / (maxstep);
				}

				return clamp(fixed4((ret.rgb / max(1,ret.a))*0.25 + 0.65, ret.a*0.35), 0, 1);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// ray direction
				float3 rd = normalize(_CameraDir);//normalize(i.ray.xyz);
				// ray origin (camera position)
				float3 ro = i.ray;

				float2 duv = i.uv;
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					duv.y = 1 - duv.y;
				#endif

				// Convert from depth buffer (eye space) to true distance from camera
				// This is done by multiplying the eyespace depth by the length of the "z-normalized"
				// ray (see vert()).  Think of similar triangles: the view-space z-distance between a point
				// and the camera is proportional to the absolute distance.
				float depth = 1 - (tex2D(_CameraDepthTexture, duv).r);
				depth = depth * 60 + 0.3; // near and far clip planes
				//depth *= length(i.ray.xyz);

				fixed3 col = tex2D(_MainTex, i.uv); // Color of the scene before this shader was run
				fixed4 add = raymarch(ro + rd * (depth - 10), rd, 10);

				// Returns final color using alpha blending
				return fixed4(col*(1.0 - add.w) + add.xyz * add.w, 1.0);
			}



			ENDCG
		}
	}
}
