Shader "Unlit/ShadowCaster" {
Properties {
}
SubShader {
    Tags { "RenderType"="Opaque" }

CGPROGRAM
#pragma surface surf Lambert noforwardadd


struct Input {
    half a;
};

void surf (Input IN, inout SurfaceOutput o) {
}
ENDCG
}

Fallback "Mobile/VertexLit"
}
