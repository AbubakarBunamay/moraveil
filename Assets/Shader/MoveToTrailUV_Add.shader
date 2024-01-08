/*
Trail ������Ʈ�� Tiling ���� �����Ǿ����. (�׷��� ��ũ���� ���� UV�� Trail ���׸�Ʈ ũ��� ��� ���� �����ϰ� ����)
*/

Shader "MoveToTrailUV/MoveToTrailUV_Add"
{
	Properties
	{
		_MainTex("Main Texture (RGB)", 2D) = "white" {}
		_MainTexVFade("MainTex V Fade", Range(0, 1)) = 0
		_MainTexVFadePow("MainTex V Fade Pow", Float) = 1
		_MainTexPow("Main Texture Gamma", Float) = 1
		_MainTexMultiplier("Main Texture Multiplier", Float) = 1
		_TintTex("Tint Texture (RGB)", 2D) = "white" {}
		_Multiplier("Multiplier", Float) = 1
		_MainScrollSpeedU("Main Scroll U Speed", Float) = 10
		_MainScrollSpeedV("Main Scroll V Speed", Float) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent"}
			Blend One One // Additive
			ZWrite Off

			Pass
			{
				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

				struct Attributes
				{
					float4 positionOS : POSITION;
					float2 uv : TEXCOORD0;
					half4 color : COLOR;
				};

				struct Varyings
				{
					float2 uv : TEXCOORD0;
					float2 uvOrigin : TEXCOORD1; // ���� UV
					float4 positionHCS : SV_POSITION;
					half4 color : COLOR;
				};

				sampler2D _MainTex;
				sampler2D _TintTex;

				CBUFFER_START(UnityPerMaterial)
					half4 _MainTex_ST;
					half _MainTexVFade;
					half _MainTexVFadePow;
					half _MainTexPow;
					half _MainTexMultiplier;
					half _Multiplier;
					half _MainScrollSpeedU;
					half _MainScrollSpeedV;
					
					// MoveToMaterialUV ��ũ��Ʈ���� ���޹޴� UV ��ũ�� ��.
					// ������Ƽ���� �Ϻη� ���� ����. ������Ƽ�� ���� ��� �����Ϳ��� �̸������ ���޵Ǵ� ������ ��� ���� ���� �������� �νĵǾ ������Ƽ ���� �۵��ϴ� ������� ����
					half _MoveToMaterialUV;
				CBUFFER_END

				Varyings vert(Attributes IN)
				{
					Varyings o;
					o.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
					o.uv = TRANSFORM_TEX(IN.uv, _MainTex);
					o.uv.x -= frac(_Time.x * _MainScrollSpeedU) + _MoveToMaterialUV;
					o.uv.y -= frac(_Time.x * _MainScrollSpeedV);
					o.uvOrigin = IN.uv;
					o.color = IN.color;
					return o;
				}

				half4 frag(Varyings IN) : SV_Target
				{
					half4 mainTex = tex2D(_MainTex, IN.uv);

					// ���� �ؽ��� ����
					half vFade = 1 - abs(IN.uvOrigin.y - 0.5) * 2; // ���� uv �������� A �׷��� ����
					vFade = pow(abs(vFade), _MainTexVFadePow); // A ����� ���� �����ϰ� Ȥ�� �ձ۰�
					vFade = lerp(1, vFade, _MainTexVFade);
					mainTex.rgb *= vFade; // �ϴ� �ؽ��Ŀ� ���� ���̵�ƿ����� ����
					mainTex.rgb = pow(abs(mainTex.rgb), _MainTexPow) * _MainTexMultiplier; // ���� �ؽ��� 1�� ����
					
					// ���ý� ���Ŀ� _Multiplier�� �̿�ȭ�� ���� �ϳ��� ����
					half intensity = _Multiplier * IN.color.a;

					// Tint
					half avr = mainTex.r * 0.3333 + mainTex.g * 0.3334 + mainTex.b * 0.3333;
					avr = saturate(avr * intensity); // intensity 1�� �Ѵ� ������ �ϴ� 1�� ���ø�
					half4 col = tex2D(_TintTex, half2(avr, 0.5));

					half intensityHigh = max(1, intensity); // 1���� ������ 1�� �ǰ� 1���� ũ�� intensity ���� ��� (1���� ū ���� HDR�� ��Ƣ��)
					col.rgb *= intensityHigh * IN.color.rgb;
					return col;
				}
				ENDHLSL
			}
		}
}
