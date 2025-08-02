Shader "UI/CRTOverlay"
{
    Properties
    {
        _MainTex            ("Sprite Texture", 2D)                    = "white" {}
        _ScanlineIntensity  ("Scanline Intensity", Range(0,1))        = 0.5
        _Curvature          ("Screen Curvature",  Range(0,1))         = 0.12
        _Vignette           ("Vignette Strength", Range(0,1))         = 0.35
        _Aberration         ("Chromatic Aberration (px)", Range(0,4)) = 1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent"
               "IgnoreProjector"="True" "PreviewType"="Plane"
               "CanUseSpriteAtlas"="True" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        Lighting Off

        Pass
        {
            HLSLPROGRAM
            #include <UnityShaderUtilities.cginc>

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "UnityUI.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color  : COLOR;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                float4 color : COLOR;
                float2 uv    : TEXCOORD0;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float2 _MainTex_TexelSize;

            half  _ScanlineIntensity;
            half  _Curvature;
            half  _Vignette;
            float _Aberration;           // in texels / pixels

            v2f vert (appdata v)
            {
                v2f o;
                o.pos   = UnityObjectToClipPos(v.vertex);
                o.uv    = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            // simple barrel distortion
            float2 Barrel(float2 uv, half c)
            {
                float2 p  = uv * 2.0 - 1.0;
                float  r2 = dot(p, p);
                p *= 1.0 + c * r2;
                return (p + 1.0) * 0.5;
            }

            half4 frag (v2f i) : SV_Target
            {
                // curved CRT glass
                float2 uv = Barrel(i.uv, _Curvature);

                // chromatic aberration (RGB split)
                float2 offset = (_Aberration * _MainTex_TexelSize);
                float3 col;
                col.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + offset).r;
                col.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv          ).g;
                col.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv - offset).b;

                // horizontal scanlines
                float scan  = sin(uv.y * _ScreenParams.y * 0.5) * 0.5 + 0.5;
                col        *= lerp(1.0, scan, _ScanlineIntensity);

                // vignette (soft dark corners)
                float2 d   = uv - 0.5;
                col       *= 1.0 - _Vignette * dot(d, d) * 4.0;

                half alpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv).a;
                return half4(col, alpha) * i.color;
            }

            ENDHLSL
        }
    }
    FallBack "UI/Default"
}
