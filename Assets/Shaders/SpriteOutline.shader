Shader "Custom/SpriteWithOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Sprite Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0.0, 0.1)) = 0.02
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;
            float4 _OutlineColor;
            float _OutlineWidth;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float4 texColor = tex2D(_MainTex, i.uv);
                float alpha = texColor.a;
                
                float2 offsets[8] = {
                    float2(-_OutlineWidth, 0),
                    float2(_OutlineWidth, 0),
                    float2(0, -_OutlineWidth),
                    float2(0, _OutlineWidth),
                    float2(-_OutlineWidth, -_OutlineWidth),
                    float2(-_OutlineWidth, _OutlineWidth),
                    float2(_OutlineWidth, -_OutlineWidth),
                    float2(_OutlineWidth, _OutlineWidth)
                };

                float outlineAlpha = 0.0;
                for (int j = 0; j < 8; j++) {
                    float4 sampleColor = tex2D(_MainTex, i.uv + offsets[j]);
                    outlineAlpha = max(outlineAlpha, sampleColor.a);
                }

                if (outlineAlpha > 0.0 && alpha == 0.0) {
                    return _OutlineColor;
                }

                return texColor * _Color;
            }
            ENDCG
        }
    }
}
