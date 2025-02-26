Shader "Hidden/ZbufferViewer" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            
            float AdjustedDepth(float rawDepth)
            {
                float persp = LinearEyeDepth(rawDepth);
                float ortho = (_ProjectionParams.z - _ProjectionParams.y) * (1 - rawDepth) + _ProjectionParams.y;
                return lerp(persp,ortho,unity_OrthoParams.w) / _ProjectionParams.z;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                float depth = tex2D(_CameraDepthTexture, i.uv).r;
                depth = Linear01Depth(depth);
                return AdjustedDepth(depth);
            }
            ENDCG
        }
    }
}
