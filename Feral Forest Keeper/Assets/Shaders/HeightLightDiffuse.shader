Shader "Custom/HeightLightDiffuse"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_HeightMin("Height Min", float) = 0.0
		_HeightMax("Height Max", float) = 0.0
		_ColorMin("Tint Color At Min", Color) = (0,0,0,1)
		_ColorMax("Tint Color At Max", Color) = (1,1,1,1)
		_Speed("Speed", float) = 0.0
		_Amplitude("Movement Amplitude", float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 worldPos;
			float2 texcoordScrollInput;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		fixed4 _ColorMin;
		fixed4 _ColorMax;
		float _HeightMin;
		float _HeightMax;
		float _Speed;
		float _Amplitude;
		float _MovementRange;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float l_rangeMovement = _Amplitude * sin(_Time.y * _Speed);
			v.vertex.x += l_rangeMovement * (v.vertex.y / 1);
			//o.texcoordScrollInput.x = v.texcoord.x + l_rangeMovement * ((0.5 - v.vertex.y) / 1);
			//v.texcoord.x += o.texcoordScrollInput;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float h = (_HeightMax - IN.worldPos.y) / (_HeightMax - _HeightMin);
			fixed4 tintColor = lerp(_ColorMax.rgba, _ColorMin.rgba, h);
			//o.worldPos.z += 0;
			fixed4 col = tintColor;
            o.Albedo = c.rgb * col.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a * col.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
