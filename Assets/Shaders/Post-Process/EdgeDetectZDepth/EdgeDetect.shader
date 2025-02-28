Shader "Hidden/EdgeDetect" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader {

        Pass {
            CGPROGRAM
            #pragma vertex vp
            #pragma fragment fp

            #include "UnityCG.cginc"

            struct VertexData {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            Interpolators vp(VertexData v) {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex, _CameraDepthTexture;
            float4 _CameraDepthTexture_TexelSize;
            float4 _BorderColor;

            float AdjustedDepth(float rawDepth) {
                float persp = LinearEyeDepth(rawDepth);
                float ortho = (_ProjectionParams.z - _ProjectionParams.y) * (1 - rawDepth) + _ProjectionParams.y;
                return lerp(persp,ortho,unity_OrthoParams.w) / _ProjectionParams.z;
            }

            fixed4 fp(Interpolators i) : SV_Target {
                int x, y;
                fixed4 col = tex2D(_MainTex, i.uv);
                float depth = tex2D(_CameraDepthTexture, i.uv).r;
                depth = Linear01Depth(AdjustedDepth(depth));
                depth = (depth*(depth+5));
                
                float n = Linear01Depth(AdjustedDepth(tex2D(_CameraDepthTexture, i.uv + _CameraDepthTexture_TexelSize * float2(0, 1)).r));
                float e = Linear01Depth(AdjustedDepth(tex2D(_CameraDepthTexture, i.uv + _CameraDepthTexture_TexelSize * float2(1, 0)).r));
                float s = Linear01Depth(AdjustedDepth(tex2D(_CameraDepthTexture, i.uv + _CameraDepthTexture_TexelSize * float2(0, -1)).r));
                float w = Linear01Depth(AdjustedDepth(tex2D(_CameraDepthTexture, i.uv + _CameraDepthTexture_TexelSize * float2(-1, 0)).r));
                
                if (n - s > 0.001 || w - e > 0.0001 || e - w > 0.0001 || s - n > 0.0001)
                    col = _BorderColor;
                
                return col;
            }
            ENDCG
        }
    }
}
