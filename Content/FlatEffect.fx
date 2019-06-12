float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

float4x4 WorldInverseTranspose;

float3 DiffuseLightDirection1 = float3(0.5, 0.5, 0.5);
float3 DiffuseLightDirection2 = float3(-0.5, -0.5, -0.5);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity1 = 1.0;
float DiffuseIntensity2 = 0.5;

texture ModelTexture;
sampler2D textureSampler = sampler_state {
	Texture = (ModelTexture);
};

struct VertexShaderInput
{
	float4 Position : SV_POSITION;
	float4 Normal : NORMAL;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR;
	float4 Normal : NORMAL;
	float2 TextureCoordinate : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float4 normal = mul(input.Normal, WorldInverseTranspose);
	float lightIntensity = dot(normal, DiffuseLightDirection1) + dot(normal, DiffuseLightDirection2);
	output.Color = saturate(DiffuseColor * DiffuseIntensity1 * DiffuseIntensity2 * lightIntensity);
	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
	textureColor.a = 1;

	return saturate(textureColor * (input.Color) + AmbientColor * AmbientIntensity);
}

technique Ambient
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}

