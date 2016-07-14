Shader "Test/NewImageEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}



	SubShader
	{
		Tags
		{
			"Queue" = "Geometry"
			"RenderType" = "Opaque"
		}

		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass{
			CGPROGRAM

			#pragma vertex vert             
			#pragma fragment frag

			struct vertInput {
				float4 pos : POSITION;
			};

			struct vertOutput {
				float4 pos : SV_POSITION;
			};

			vertOutput vert(vertInput input) {
				vertOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, input.pos);
				return o;
			}

			half4 frag(vertOutput output) : COLOR{
				return half4(1.0, 0.0, 0.0, 1.0);
			}
			ENDCG
		}
	}
}
