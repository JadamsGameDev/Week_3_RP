Shader "Custom/Acid"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_Emissive("Emissive", 2D) = "white" {}
		[HDR]_EmissiveColor("EmissiveColor", Color) = (1,1,1,1)
		_RippleSpeed("Ripple Speed", Range(0,1000)) = 1.0
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
		
		float rand2dTo1d(float2 value, float2 dotDir = float2(12.9898, 78.233)) {
	float2 smallValue = sin(value);
	float random = dot(smallValue, dotDir);
	random = frac(sin(random) * 143758.5453);
	return random;
}

		float2 rand2dTo2d(float2 value) {
	return float2(
		rand2dTo1d(value, float2(12.989, 78.233)),
		rand2dTo1d(value, float2(39.346, 11.135))
	);
}

		

		float voronoiNoise(float2 value) {
			float2 baseCell = floor(value);

			float minDistToCell = 10;
			[unroll]
			for (int x = -1; x <= 1; x++) {
				[unroll]
				for (int y = -1; y <= 1; y++) {
					float2 cell = baseCell + float2(x, y);
					float2 cellPosition = cell + rand2dTo2d(cell);
					float2 toCell = cellPosition - value;
					float distToCell = length(toCell);
					if (distToCell < minDistToCell) {
							minDistToCell = distToCell;
					}
				}
			}
			return minDistToCell;
		}

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
		fixed4 _EmissiveColor;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		sampler2D _Emissive;

		float _RippleSpeed;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			////////////////////////// uvs
			float rippleSpeed = _RippleSpeed * Time.x;

			float2 varoInput;

			varoInput.x = rippleSpeed;
			varoInput.y = 1;

			float varonoi = voronoiNoise(varoInput);





			//////////////////////////////




            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 e = tex2D (_Emissive, IN.uv_MainTex) * _EmissiveColor;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = _EmissiveColor.a;
			e.r = varonoi;
			e.g = varonoi;
			e.b = varonoi;
			o.Emission = e.rgb;
			
        }
        ENDCG
    }
    FallBack "Diffuse"
}
