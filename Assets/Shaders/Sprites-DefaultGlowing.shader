// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Sprites/Default Glowing"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_Glow("Glow", Color) = (0,0,0,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
		[HideInInspector]_Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.5

	}

		SubShader
		{
			Tags
			{
				"LightMode" = "ForwardBase"
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex SpriteVert
				#pragma fragment SpriteFrag

				#pragma target 2.0
				#pragma multi_compile_fwdbase
				#pragma multi_compile_instancing
				#pragma multi_compile _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

				#include "UnitySpritesPagiGlow.cginc"


			ENDCG
			}

		}
			Fallback "Transparent/Cutout/Diffuse"
}
