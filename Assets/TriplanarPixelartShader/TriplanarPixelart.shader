
Shader "Standard Triplanar Pixelart"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_TopTex("Top texture", 2D) = "white" {}
		_SideTex("Side texture", 2D) = "white" {}

		_Glossiness("Glossiness", Range(0, 1)) = 0.5
		[Gamma] _Metallic("Metallic", Range(0, 1)) = 0

		_MapScale("Scale", Float) = 1
		_PPU("Pixels Per Unit", Float) = 32
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque"
					"Overlay" = "Grid"
		}

			CGPROGRAM

			#pragma surface surf Standard vertex:vert fullforwardshadows addshadow


			#pragma target 3.0

			half4 _Color;
		sampler2D _TopTex;
		sampler2D _SideTex;

			half _Glossiness;
			half _Metallic;


			half _MapScale;
			float _PPU;

			struct Input
			{
				float3 pos;
				float3 normal;
			};

			void vert(inout appdata_full v, out Input data)
			{
				//UNITY_INITIALIZE_OUTPUT(Input, data);
				data.pos = mul(unity_ObjectToWorld, v.vertex).xyz;
				
				data.pos -= data.pos % 1.0 / _PPU;
				data.normal = UnityObjectToWorldNormal(v.normal);
			}

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Blending factor of triplanar mapping
				float3 bf = normalize(abs(IN.normal));
				bf /= dot(bf, (float3)1);

				
				// Triplanar mappin
				float2 tx = (IN.pos.zy) * _MapScale;
				float2 ty = IN.pos.xz * _MapScale;
				float2 tz = IN.pos.xy * _MapScale;
				if (IN.normal.x < 0)
					tx.x *= -1;
				if (IN.normal.y < 0)
					ty.x *= -1;
				if (IN.normal.z > 0)
					tz.x *= -1;
				/*
				tx -= tx % (1.0 / _PPU) + (0.5 / _PPU);
				ty -= ty % (1.0 / _PPU) + (0.5 / _PPU);
				tz -= tz % (1.0 / _PPU) + (0.5 / _PPU);*/
				
				bf.x = step(bf.y + bf.z, bf.x);
				bf.y = step(bf.x + bf.z, bf.y);
				bf.z = step(bf.y + bf.x, bf.z);

				// Base color
				half4 cx = tex2D(_SideTex, tx) * bf.x;
				half4 cy = tex2D(_TopTex, ty) * bf.y;
				half4 cz = tex2D(_SideTex, tz) * bf.z;
				half4 color = (cx + cy + cz) * _Color;
				o.Albedo = color.rgb;
				o.Alpha = color.a;

				// Misc parameters
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
			}
			ENDCG
		}
			FallBack "Diffuse"
}