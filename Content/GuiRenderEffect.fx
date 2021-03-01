#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
float2 SpritePosition;
float2 TargetRectPosition;
float2 TargetRectSize;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	if ((SpritePosition.x >= TargetRectPosition.x && SpritePosition.x <= TargetRectPosition.x + TargetRectSize.x) && (SpritePosition.y >= TargetRectPosition.y && SpritePosition.y <= TargetRectPosition.y + TargetRectSize.y))
	return tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
	else return float4(0,0,0,0);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};