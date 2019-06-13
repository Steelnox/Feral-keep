// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/ButterflyMovement_Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Speed("Speed", float) = 0.0
		_Amplitude("Amplitud", float) = 0.0
		_DividerX("DividerX", float) = 0.0
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 100
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _Height;
			float _Speed;
			float _Amplitude;
			float _DividerX;

            v2f vert (appdata v)
            {
                v2f o;

				o.vertex = v.vertex;
				//o.vertex = mul(unity_ObjectToWorld, o.vertex);
				_Height = _Amplitude * sin(_Time.y * _Speed);
				o.vertex.y += v.uv.x < 0.5 ? _Height * ((0.5 - v.uv.x) / 0.5) : _Height * ((v.uv.x - 0.5) / 0.5);
				o.vertex = UnityObjectToClipPos(o.vertex);
				//o.vertex = mul(UNITY_MATRIX_MVP, o.vertex);
				//o.vertex = UnityObjectToClipPos(o.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
