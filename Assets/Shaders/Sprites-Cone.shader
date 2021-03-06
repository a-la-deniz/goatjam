// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Sprites/Cone"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

		_WaveStart("WaveStart", Range(0,1)) = 0
		_WaveEnd("WaveEnd", Range(0,1)) = 1
		_WaveBlur("WaveBlur", Range(0.001,1)) = 0.001
		_WaveBoost("WaveBoost", Range(1,10)) = 1
		_WaveRepeat("WaveRepeat", float) = 0
		_WavePhase("WavePhase", float) = 0

		_TotalStart("TotalStart", Range(0,1)) = 0
		_TotalEnd("TotalEnd", Range(0,1)) = 1
		_TotalBlur("TotalBlur", Range(0.001,1)) = 0.001
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SpriteFrag2
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

			fixed _WaveStart;
			fixed _WaveEnd;
			fixed _WaveBlur;
			fixed _WaveBoost;
			fixed _WaveRepeat;
			fixed _WavePhase;

			fixed _TotalStart;
			fixed _TotalEnd;
			fixed _TotalBlur;

			float Remap(float value, float from1, float to1, float from2, float to2)
			{
				return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
			}

			fixed4 SpriteFrag2(v2f IN) : SV_Target
			{
				fixed4 c = IN.color;
				fixed4 mask = SampleSpriteTexture(IN.texcoord);
				fixed radius = length(-IN.texcoord.xy + fixed2(0.5, 1)) * rsqrt(2);
				fixed repeatRad = frac(radius * _WaveRepeat + _WavePhase);
				fixed prop = smoothstep(_WaveStart - _WaveBlur, _WaveStart + _WaveBlur, repeatRad);
				prop *= smoothstep(_WaveEnd + _WaveBlur, _WaveEnd - _WaveBlur, repeatRad);

				prop *= smoothstep(_TotalStart - _TotalBlur, _TotalStart + _TotalBlur, radius);
				prop *= smoothstep(_TotalEnd + _TotalBlur, _TotalEnd - _TotalBlur, radius);


				c.a *= prop * _WaveBoost;
				c.a *= mask.g;
				c.rgb *= c.a;
				return c;
			}

        ENDCG
        }
    }
}
