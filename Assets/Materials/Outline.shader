Shader "Custom/Outline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0.0, 0.1)) = 0.02
        _TintColor ("Tint Color", Color) = (1,1,1,1)
        _TintAlpha ("Tint Alpha", Range(0.0, 1.0)) = 1.0
    }
    SubShader
    {
        Tags {"Queue"="Overlay"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting On
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : NORMAL;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineThickness;
            float4 _TintColor;
            float _TintAlpha;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.normal = v.normal;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 normal = normalize(i.normal);
                float diffuse = max(0, dot(normal, lightDir));

                float2 offset[8] = {
                    float2(-1, -1), float2(-1, 0), float2(-1, 1),
                    float2(0, -1), float2(0, 1),
                    float2(1, -1), float2(1, 0), float2(1, 1)
                };

                float4 original = tex2D(_MainTex, i.texcoord) * i.color;
                float4 outlineColor = _OutlineColor;
                float thickness = _OutlineThickness;

                float alpha = original.a;

                for (int j = 0; j < 8; j++)
                {
                    float2 samplePos = i.texcoord + offset[j] * thickness;
                    alpha = max(alpha, tex2D(_MainTex, samplePos).a);
                }

                if (original.a == 0 && alpha > 0)
                {
                    return outlineColor;
                }

                float4 tintedColor = _TintColor;
                tintedColor.a = _TintAlpha * original.a;

                float4 finalColor = lerp(original, tintedColor, _TintAlpha);
                finalColor.rgb *= diffuse;

                // Если альфа спрайта равна 0, шейдер возвращает прозрачность
                if (original.a == 0)
                {
                    return float4(0, 0, 0, 0);
                }

                return finalColor;
            }
            ENDCG
        }
    }
}