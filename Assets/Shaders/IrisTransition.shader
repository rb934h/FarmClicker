Shader "Custom/IrisTransition"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        _Radius ("Radius", Range(0,1)) = 0
        _ScreenAspect ("Screen Aspect", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Pass
        {
            ZTest Always
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _Radius;
            float4 _Color;
            float _ScreenAspect;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2 - 1;
                o.uv.x *= _ScreenAspect;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = length(i.uv);
                float alpha = smoothstep(_Radius, _Radius + 0.001, dist);
                return float4(_Color.rgb, alpha * _Color.a);
            }
            ENDHLSL
        }
    }
}
