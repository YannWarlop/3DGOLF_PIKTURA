Shader "Hidden/PostProcssOutline"
{
    Properties{
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        Tags { 
			"RenderType" = "Opaque" 
		}

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float _Scale

            struct MeshData {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
				float3 viewNormal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.viewNormal = COMPUTE_VIEW_NORMAL;
				//o.viewNormal = mul((float3x3)UNITY_MATRIX_M, v.normal);
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                //On sample les pixels en X pour detection d'edges
                float halfScaleFloor = floor(_Scale * 0.5); //-Y a partir du pixel qu'on sample
                float halfScaleCeil = ceil(_Scale * 0.5); //+Y a partir du pixel qu'on sample

                float2 bottomLeftUV = i.texcoord - float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleFloor;
                float2 topRightUV = i.texcoord + float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleCeil;  
                float2 bottomRightUV = i.texcoord + float2(_MainTex_TexelSize.x * halfScaleCeil, -_MainTex_TexelSize.y * halfScaleFloor);
                float2 topLeftUV = i.texcoord + float2(-_MainTex_TexelSize.x * halfScaleFloor, _MainTex_TexelSize.y * halfScaleCeil);
                
            }
            ENDCG
        }
    }
}
