Shader "Mobile/Displacement"
{
    Properties
    {
        [NoScaleOffset]        _Albedo("Albedo (RGB), Alpha (A)", 2D) = "white" {}
        [NoScaleOffset]        _Metallic("Metallic (R), Occlusion (G), Emission (B), Smoothness (A)", 2D) = "black" {}
        [NoScaleOffset]        _Normal("Normal (RGB)", 2D) = "bump" {}
        [NoScaleOffset]        _DispTex("Displacement Texture", 2D) = "white" {}
        _Amount("Displacement Amount", Range(0,3)) = 0.5
    }
 
    SubShader
    {
        Tags
        {
            "Queue" = "Geometry"
            "RenderType" = "Opaque"
        }
 
        CGINCLUDE
        #define _GLOSSYENV 1
        ENDCG
 
        CGPROGRAM
        #pragma target 3.0
        #include "UnityPBSLighting.cginc"
        #pragma surface surf Standard vertex:vert
        #pragma exclude_renderers gles
 
        struct Input
        {
            float2 uv_Albedo;
        };
 
        sampler2D _Albedo;
        sampler2D _Normal;
        sampler2D _Metallic;
 
        float _Amount;
        sampler2D _DispTex;
 
        void vert(inout appdata_full v)
        {
            v.vertex.xyz += v.normal * tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, 0)).r * _Amount;
        }
 
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 albedo = tex2D(_Albedo, IN.uv_Albedo);
            fixed4 metallic = tex2D(_Metallic, IN.uv_Albedo);
            fixed3 normal = UnpackScaleNormal(tex2D(_Normal, IN.uv_Albedo), 1);
 
            o.Albedo = albedo.rgb;
            o.Alpha = albedo.a;
            o.Normal = normal;
            o.Smoothness = metallic.a;
            o.Occlusion = metallic.g;
            o.Emission = metallic.b;
            o.Metallic = metallic.r;
        }
        ENDCG
    }
 
        FallBack "Diffuse"
}