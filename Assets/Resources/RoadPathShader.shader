Shader "Custom/RoadPathShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        _Radius("Radius", Float) = 0
        _Center("Center", Vector) = (0, 0, 0, 0)
        _Size("Size", Vector) = (0, 0, 0, 0)
        _Scale("Scale", Float) = 1
        
        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15
    }
         
        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }
            ColorMask[_ColorMask]

            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert     
                #pragma fragment frag    
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord  : TEXCOORD0;
                    float2 worldPosition : TEXCOORD1;
                };

                sampler2D _MainTex;
                fixed4 _Color;
                half _Radius;
                float2 _Center;
                half2 _Size;
                half _Scale;
                
                v2f vert(appdata_t IN)
                {
                    v2f OUT;
                    OUT.worldPosition = IN.vertex;
                    OUT.vertex = UnityObjectToClipPos(IN.vertex);
                    OUT.texcoord = IN.texcoord;
    #ifdef UNITY_HALF_TEXEL_OFFSET     
                    OUT.vertex.xy -= (_ScreenParams.zw - 1.0);
    #endif     
                    OUT.color = IN.color * _Color;
                    return OUT;
                }

                float sdBox(float2 p, float2 b, float r)
                {
                    float2 d = abs(p) - b + float2(r, r);
                    return min(max(d.x, d.y), 0.0) + length(max(d, 0.0)) - r;
                }

                fixed4 frag(v2f IN) : SV_Target
                {
                    half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;

                    float d = sdBox(IN.worldPosition - _Center, _Size * _Scale, _Radius * _Scale);
                    d = saturate(step(0, d) * d / ( 1 * _Scale));
                    d = saturate(1 - d);
                    color.a = color.a * d;
    
                    return color;
                }
                ENDCG
          }
        }
}


