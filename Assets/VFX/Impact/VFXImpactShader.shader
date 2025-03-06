 Shader "Unlit/VFXBase"{
    Properties{
        _Color ("Color", Color) = (1,1,1,1)
        _StartTime ("StartTime", Float) = 0
    }
    SubShader{
        Tags {
            "RenderType"="TransparentCutout"
            "Queue"="AlphaTest"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite off
            Cull off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #define TAU 6.283185307179586

            struct MashData {
                float4 vertex : POSITION;
                float4 normals : NORMAL;
                float2 uv : TEXCOORD0;
            };
            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 normal :TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _StartTime;
            float InverseLerp( float a, float b, float v) {  
            return (v-a) / (b-a);
            }

            Interpolators vert (MashData v)
            {
                Interpolators o;
                float lT = ((_Time.y - _StartTime) * 2)% 1;
                float TFunctionxz = (lT*(1-lT)*(1-lT)*6);
                float TFunctiony = (lT*(1-lT*2)*(1-lT)*6);
                v.vertex.xz *= TFunctionxz;
                v.vertex.y *= TFunctiony;
                v.vertex.y +=TFunctiony;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normals;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target {
                float Vertical = (0.8- i.uv.y) * 20;
                Vertical = saturate(Vertical);
                Vertical *= abs(i.normal.y) < 0.9;
                return float4(_Color.x,_Color.y,_Color.z,Vertical);  
            }
            ENDCG
        }
    }
}
