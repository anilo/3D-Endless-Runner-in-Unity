Shader "Unlit/SimpleUnlitColor"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Light("Light direction", Vector) = (1,1,1,1)
        _LightAmbient("Ambient light", Float) = 0.1
        _Ramp("Ramp", Vector) = (0.1, 0.5, 0.7, 0.5)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal :TEXCOORD0;
            };

            fixed4 _Color;
            fixed4 _Light;
            fixed _LightAmbient;
            fixed4 _Ramp;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(v.normal);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed d = dot(normalize(_Light.xyz), i.normal);
                //fixed min = _Ramp.x;
                fixed4 light = _Ramp.x + step(_Ramp.y, d) * (1 - _Ramp.x - _Ramp.w) + step(_Ramp.z, d) * _Ramp.w;
                fixed4 col = _LightAmbient + _Color * saturate(light);
                return col;
            }
            ENDCG
        }
    }
}
