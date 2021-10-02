Shader "Unlit/UnlitShadowReceiver" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" }

    Pass {
        Name "FORWARD"
        Tags { "LightMode" = "ForwardBase" }

    CGPROGRAM

        #pragma vertex vert_surf
        #pragma fragment frag_surf
        #pragma target 2.0
        #pragma multi_compile_fwdbase
        #include "HLSLSupport.cginc"
        #include "UnityCG.cginc"
        #include "Lighting.cginc"
        #include "AutoLight.cginc"

        sampler2D _MainTex;
        float4 _MainTex_ST;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout fixed4 o) {
            o = tex2D (_MainTex, IN.uv_MainTex);
        }

        struct v2f_surf {
            float4 pos : SV_POSITION;
            float2 pack0 : TEXCOORD0;
            LIGHTING_COORDS(3,4)
        };

        v2f_surf vert_surf (appdata_full v)
        {
            v2f_surf o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
            TRANSFER_VERTEX_TO_FRAGMENT(o);
            return o;
        }

        fixed4 frag_surf (v2f_surf IN) : SV_Target
        {
            Input surfIN;
            surfIN.uv_MainTex = IN.pack0.xy;
            fixed atten = LIGHT_ATTENUATION(IN);
            fixed4 c;
            surf (surfIN, c);
            c.rgb *= atten;
            UNITY_OPAQUE_ALPHA(c.a);
            return c;
        }

    ENDCG
    }
}

FallBack "Mobile/VertexLit"
}
