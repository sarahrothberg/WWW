Shader "Custom/Rothberg_GrainyShader"
{
	Properties
	{
		_ColorBase("Base Color", Color) = (1,1,1,1)
		_ColorLit("Lit Color", Color) = (1,1,1,1)
		_Frequency("Frequency", Range(1000, 50000)) = 30000
		_Ambience("Ambient Intensity", Range(0,2)) = 0.5
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc" // for _LightColor0

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 worldpos : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD2;
				float4 vertex : SV_POSITION;
				float3 worldpos : TEXCOORD1;
				fixed4 diff : COLOR0; // diffuse lighting color
			};

			fixed4 _ColorBase;
			fixed4 _ColorLit;
			float _Frequency;
			fixed _Ambience;

			float3 hash(float3 p)
			{
				p = float3(dot(p,float3(127.1,311.7, 74.7)),
								dot(p,float3(269.5,183.3,246.1)),
								dot(p,float3(113.5,271.9,124.6)));

				return -1.0 + 2.0*frac(sin(p)*43758.5453123);
			}

			// return value noise (in x) and its derivatives (in yzw)
			float4 noise3D(in float3 x)
			{
				// grid
				float3 p = floor(x);
				float3 w = frac(x);

				#if 1
				// quintic interpolant
				float3 u = w * w*w*(w*(w*6.0 - 15.0) + 10.0);
				float3 du = 30.0*w*w*(w*(w - 2.0) + 1.0);
				#else
				// cubic interpolant
				float3 u = w * w*(3.0 - 2.0*w);
				float3 du = 6.0*w*(1.0 - w);
				#endif    

				// gradients
				float3 ga = hash(p + float3(0.0,0.0,0.0));
				float3 gb = hash(p + float3(1.0,0.0,0.0));
				float3 gc = hash(p + float3(0.0,1.0,0.0));
				float3 gd = hash(p + float3(1.0,1.0,0.0));
				float3 ge = hash(p + float3(0.0,0.0,1.0));
				float3 gf = hash(p + float3(1.0,0.0,1.0));
				float3 gg = hash(p + float3(0.0,1.0,1.0));
				float3 gh = hash(p + float3(1.0,1.0,1.0));

				// projections
				float va = dot(ga, w - float3(0.0,0.0,0.0));
				float vb = dot(gb, w - float3(1.0,0.0,0.0));
				float vc = dot(gc, w - float3(0.0,1.0,0.0));
				float vd = dot(gd, w - float3(1.0,1.0,0.0));
				float ve = dot(ge, w - float3(0.0,0.0,1.0));
				float vf = dot(gf, w - float3(1.0,0.0,1.0));
				float vg = dot(gg, w - float3(0.0,1.0,1.0));
				float vh = dot(gh, w - float3(1.0,1.0,1.0));

				// interpolations
				return float4(va + u.x*(vb - va) + u.y*(vc - va) + u.z*(ve - va) + u.x*u.y*(va - vb - vc + vd) + u.y*u.z*(va - vc - ve + vg) + u.z*u.x*(va - vb - ve + vf) + (-va + vb + vc - vd + ve - vf - vg + vh)*u.x*u.y*u.z,    // value
							ga + u.x*(gb - ga) + u.y*(gc - ga) + u.z*(ge - ga) + u.x*u.y*(ga - gb - gc + gd) + u.y*u.z*(ga - gc - ge + gg) + u.z*u.x*(ga - gb - ge + gf) + (-ga + gb + gc - gd + ge - gf - gg + gh)*u.x*u.y*u.z +   // derivatives
							du * (float3(vb,vc,ve) - va + u.yzx*float3(va - vb - vc + vd,va - vc - ve + vg,va - vb - ve + vf) + u.zxy*float3(va - vb - ve + vf,va - vb - vc + vd,va - vc - ve + vg) + u.yzx*u.zxy*(-va + vb + vc - vd + ve - vf - vg + vh)));
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldpos = v.vertex;

				// lighting -- only uses a single Directional light
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				o.diff = nl * _LightColor0 + _Ambience;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col;
				float colN = noise3D(i.worldpos * _Frequency);
				col.rgb = lerp(_ColorBase, _ColorLit, saturate(i.diff.r + colN));

				return col;
			}
			ENDCG
		}
	}
}
