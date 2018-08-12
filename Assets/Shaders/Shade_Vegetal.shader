Shader "Custom/TestShader 1"
{
	Properties
	{
        _MainTex ("Base (RGB)", 2D) = "white" { }
		_ScrollSpeeds ("Scroll Speeds", vector) = (-5, -20, 0, 0)
		_Brightness("Offset",range(0.1,5))=3
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			//declare new variable
			float4 _ScrollSpeeds;
			float4 _Offset;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//set the UV coords such that the vegetal is centered perfectly
				o.uv+=_Offset;

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			// Convert our texture coordinates to polar form:
			float2 polar = float2(
				   atan2(i.uv.y, i.uv.x)/(2.0f * 3.141592653589f), // angle
				   length(i.uv)                                    // radius
				);

			// Apply texture scale
			polar *= _MainTex_ST.xy;

			// Scroll the texture over time.
			polar += _ScrollSpeeds.xy * _Time.x;

			// Sample using the polar coordinates, instead of the original uvs.
			// Here I multiply by MainTex
			fixed4 col = tex2D(_MainTex, polar);
			    
	
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
	        ENDCG
		}
	}
}
