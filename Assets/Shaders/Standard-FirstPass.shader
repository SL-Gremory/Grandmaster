// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Nature/Terrain/StandardPixelart" {
	Properties{
		// used in fallback on old cards & base map
		[HideInInspector] _MainTex("BaseMap (RGB)", 2D) = "white" {}
		[HideInInspector] _Color("Main Color", Color) = (1,1,1,1)
	}

		SubShader{


			Pass{
				Tags {
				"Queue" = "Geometry-100"
				"RenderType" = "Opaque"
			}
				
				Cull Front

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"

				struct v2f {
					float4 pos          : POSITION;
				};

				v2f vert(appdata_full v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					return o;
				}

				sampler2D _CameraDepthTexture;
				float4 _DepthColor;
				float _WaterDepth;

				half4 frag(v2f i) : COLOR
				{
					return fixed4(0,0,0,1);
				}

				ENDCG
			}

			Tags {
				"Queue" = "Geometry-100"
				"RenderType" = "Opaque"
				"Overlay" = "Grid"
			}
			CGPROGRAM
			#pragma surface surf Standard vertex:SplatmapVert finalcolor:SplatmapFinalColor finalgbuffer:SplatmapFinalGBuffer addshadow fullforwardshadows
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
			#pragma multi_compile_fog // needed because finalcolor oppresses fog code generation.
			#pragma target 3.0
					// needs more than 8 texcoords
					#pragma exclude_renderers gles
					#include "UnityPBSLighting.cginc"

					#pragma multi_compile __ _NORMALMAP

					#define TERRAIN_STANDARD_SHADER
					#define TERRAIN_INSTANCED_PERPIXEL_NORMAL
					#define TERRAIN_SURFACE_OUTPUT SurfaceOutputStandard
					#include "TerrainSplatmapPixelart.cginc"

					half _Metallic0;
					half _Metallic1;
					half _Metallic2;
					half _Metallic3;

					half _Smoothness0;
					half _Smoothness1;
					half _Smoothness2;
					half _Smoothness3;

					void surf(Input IN, inout SurfaceOutputStandard o) {
						half4 splat_control;
						half weight;
						fixed4 mixedDiffuse;
						half4 defaultSmoothness = half4(_Smoothness0, _Smoothness1, _Smoothness2, _Smoothness3);

						// want some lowpoly power?
						fixed3 x = ddx(IN.worldPos);
						fixed3 y = -ddy(IN.worldPos);
						fixed3 c = cross(x, y);
						//if (length(c) > 0)
						//o.Normal = lerp(o.Normal, normalize(c).xzy, 1);
						o.Normal = normalize(c).xzy;

						SplatmapMix(IN, defaultSmoothness, splat_control, weight, mixedDiffuse, o.Normal);

						o.Normal = normalize(o.Normal);
						o.Albedo = mixedDiffuse.rgb;
						o.Alpha = 1;
						o.Smoothness = mixedDiffuse.a;
						o.Metallic = dot(splat_control, half4(_Metallic0, _Metallic1, _Metallic2, _Metallic3));
					}
					ENDCG

					UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
					UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"
		}

			Dependency "AddPassShader" = "Hidden/TerrainEngine/Splatmap/Standard-AddPassPixelart"
						Dependency "BaseMapShader" = "Hidden/TerrainEngine/Splatmap/Standard-Base"
						Dependency "BaseMapGenShader" = "Hidden/TerrainEngine/Splatmap/Standard-BaseGen"

						Fallback "Nature/Terrain/Diffuse"
}
