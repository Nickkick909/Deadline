Shader "Custom/BlendTwoSkyboxes"
{
    Properties
    {
        _Sky1 ("Skybox 1", Cube) = "" {}
        _Sky2 ("Skybox 2", Cube) = "" {}
        _Blend ("Blend", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" }
        Cull Off ZWrite Off
        Fog { Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 dir : TEXCOORD0;
            };

            samplerCUBE _Sky1;
            samplerCUBE _Sky2;
            float _Blend;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.dir = v.vertex.xyz;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float3 dir = normalize(i.dir);
                half4 col1 = texCUBE(_Sky1, dir);
                half4 col2 = texCUBE(_Sky2, dir);
                return lerp(col1, col2, _Blend);
            }
            ENDCG
        }
    }
    FallBack "RenderFX/Skybox"
}
