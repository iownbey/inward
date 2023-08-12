Shader "Hidden/Custom/Lighting2D"
{
    HLSLINCLUDE
        // StdLib.hlsl holds pre-configured vertex shaders (VertDefault), varying structs (VaryingsDefault), and most of the data you need to write common effects.
#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    sampler2D _Lightmap;
    float4 Frag(VaryingsDefault i) : SV_Target
    {
        float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
        float4 light = tex2D(_Lightmap, i.texcoord);
        //Right now, the shader assumes the r g and b chanels are identical.
        color.rgb = lerp(color.rgb * light.rgb, float3(0, 0, 0), 1-light.a);
        return color;
    }
        ENDHLSL
        SubShader
    {
        Cull Off ZWrite Off ZTest Always
            Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment Frag
            ENDHLSL
        }
    }
}

