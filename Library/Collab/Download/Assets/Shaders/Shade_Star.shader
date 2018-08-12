Shader "Custom/Star" {
	Properties {
		_MainTex ("Star Map", 2D) = "black" {}
		_Brightness("Brightness",range(0.1,50))=10
		_StarColor("Color",Color)=(0,0,0,0)
		_SunspotSpeed("Rotation Speed",range(0,10))=1
	}
	SubShader {
	    //emits light
		Pass {
			Name "EMIT"
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
				#pragma target 3.0
				#pragma shader_feature _EMISSION

				#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
				#pragma shader_feature EDITOR_VISUALIZATION

				#pragma vertex vertBase
				#pragma fragment fragBase
				#include "UnityStandardCoreForward.cginc"
			ENDCG
		}

		//rotates texture
		Pass {
			Name "ROTATE"
		
			Tags { "RenderType"="Opaque" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float _Brightness;
			float _SunspotSpeed;
			fixed4 _StarColor;

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{   
				//define the point on the UV texture from which to circle around
 				float2 center = (0.5,0.5);  

				//declare polar coordinate variable
				float2 polar;

				//declare the coordinates of the rotated texture
				float2 newTex;

				//assign the distance between the sampled coordinate and the defined center of the UV
				polar.x=distance(center,i.texcoord);
		    
				//calculate the polar angle
				polar.y=atan2((i.texcoord.y-center.y),(i.texcoord.x-center.x));
	        
				//rotate texture
				polar.y+=(_Time*_SunspotSpeed);

				//convert back to XY coordinates
				newTex.x=(polar.x)*cos(polar.y);
				newTex.y=(polar.x)*sin(polar.y);

				//undo shift
				newTex+=center;

				//declare variable
				fixed4 newPixel;
				//sample rotated texture
				fixed4 oldPixel = tex2D(_MainTex, newTex);

				//calculate the color of our new pixel
				newPixel.rgb = (oldPixel.rgb*_StarColor*_Brightness);
				//assign alpha values
				newPixel.a = oldPixel.a;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, newPixel);

				//return the modified pixel
				return newPixel;
			}
			ENDCG
		}
	}
}