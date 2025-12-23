Shader "Custom/URP_Glitch_AlphaClip"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _GlitchIntensity ("Glitch Intensity", Range(0, 1)) = 0.1
        _ColorSplitIntensity ("Color Split Intensity", Range(0, 0.1)) = 0.02
        _Speed ("Speed", Range(0, 50)) = 10.0
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5 // 알파 절단 기준치 추가
    }

    SubShader
    {
        // 1. 투명 처리를 위해 Queue와 RenderType 수정
        Tags { 
            "RenderType"="TransparentCutout" 
            "IgnoreProjector"="True"
            "RenderPipeline"="UniversalPipeline" 
            "Queue"="AlphaTest"
        }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _GlitchIntensity;
            float _ColorSplitIntensity;
            float _Speed;
            float _Cutoff;

            float random (float2 st) {
                return frac(sin(dot(st.xy, float2(12.9898,78.233))) * 43758.5453123);
            }

            Varyings vert (Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                float2 uv = input.uv;
                float time = _Time.y * _Speed;

                // 1. 가로 블록 노이즈
                float blockNoise = step(1.0 - _GlitchIntensity, random(floor(uv.y * 10.0) + time));
                float lineNoise = step(1.0 - _GlitchIntensity, random(floor(uv.y * 40.0) - time));
                float shift = (blockNoise * 0.05 + lineNoise * 0.02) * random(time);
                
                // 2. RGB Split 및 원본 알파값 샘플링
                // 주의: 글리치가 적용된 위치의 알파값을 가져와야 합니다.
                float4 colR = tex2D(_MainTex, uv + float2(shift + _ColorSplitIntensity, 0));
                float4 colG = tex2D(_MainTex, uv + float2(shift, 0));
                float4 colB = tex2D(_MainTex, uv + float2(shift - _ColorSplitIntensity, 0));

                half4 col;
                col.r = colR.r;
                col.g = colG.g;
                col.b = colB.b;
                
                // 알파값은 기본(Green 채널 기준 위치) 알파를 사용하거나 
                // 세 채널의 평균/최대값을 사용할 수 있습니다. 여기서는 기본 위치를 사용합니다.
                col.a = colG.a; 

                // 3. 알파 테스팅 (Alpha Clipping)
                // col.a가 _Cutoff보다 작으면 이 픽셀을 버립니다.
                clip(col.a - _Cutoff);

                return col;
            }
            ENDHLSL
        }
    }
}