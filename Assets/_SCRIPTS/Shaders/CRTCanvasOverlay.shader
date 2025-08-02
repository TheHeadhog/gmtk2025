Shader "UI/CRTCanvasOverlay"
{
    Properties
    {
        _MainTex        ("Overlay Mask (optional)", 2D)              = "white" {}
        _Opacity        ("Master Opacity", Range(0,1))               = 0.6
        _Scanlines      ("Scanline Strength", Range(0,1))            = 1
        _Curvature      ("Screen Curvature", Range(0,1))             = 0.2
        _Vignette       ("Vignette Strength", Range(0,1))            = 0.4
        _AberrationPx   ("Chromatic Aberration (px)", Range(0,4))    = 1.5
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        Lighting Off

        Pass
        {
            HLSLPROGRAM
            #include <UnityShaderUtilities.cginc>

            #include "UnityUI.cginc"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

            TEXTURE2D(_MainTex);     SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

            float  _Opacity;
            half   _Scanlines;
            half   _Curvature;
            half   _Vignette;
            float  _AberrationPx;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color  : COLOR;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                float2 uv    : TEXCOORD0;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos   = UnityObjectToClipPos(v.vertex);
                o.uv    = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            float2 Barrel(float2 uv, half k)
            {
                float2 p = uv * 2.0 - 1.0;
                float  r = dot(p, p);
                p *= 1.0 + k * r;
                return (p + 1.0) * 0.5;
            }

            half4 frag(v2f i) : SV_Target
            {
                float2 uv = Barrel(i.uv, _Curvature);

                float2 texel = _AberrationPx * _ScreenParams.zw;   // zw = 1/width , 1/height

                float3 rgb;
                rgb.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + texel).r;
                rgb.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv        ).g;
                rgb.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv - texel).b;

                float scan = sin(uv.y * _ScreenParams.y * 0.5) * 0.5 + 0.5;
                rgb *= lerp(1.0, scan, _Scanlines);

                float2 d = uv - 0.5;
                rgb *= saturate(1.0 - _Vignette * dot(d, d) * 4.0);

                half alpha = _Opacity * i.color.a;
                return half4(rgb, alpha);
            }
            ENDHLSL
        }
    }

    FallBack "UI/Default"
}
