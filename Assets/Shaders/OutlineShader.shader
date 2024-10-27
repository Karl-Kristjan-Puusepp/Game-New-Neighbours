Shader "UI/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off

        Pass
        {
            // Main texture
            Name "Main"
            SetTexture [_MainTex] { combine texture * primary }
        }
        Pass
        {
            // Outline pass
            Name "Outline"
            SetTexture [_MainTex]
            {
                constantColor [_OutlineColor]
                combine constant * primary
            }
        }
    }
}
