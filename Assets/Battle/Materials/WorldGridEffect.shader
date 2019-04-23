﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/WorldGridEffect"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque"
				"Overlay" = "Grid" }
		LOD 100

		Pass
		{
		ZWrite On
		Cull Back
		ZTest LEqual
		Blend OneMinusDstColor One
		//Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD1;
			};


			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = fixed4(0.5,0.5,0.5,1) * pow((abs(length((i.worldPos.xz) % 1 - 0.5))), 2) ;
				return col;
			}
			ENDCG
		}
	}
}