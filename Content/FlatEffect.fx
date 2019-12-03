float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

float4x4 WorldInverseTranspose;

float3 DiffuseLightDirection1 = float3(1, 1, 1);
float3 DiffuseLightDirection2 = float3(1, 1, 1);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0;

float4 ModelColor = float4(0.5, 0.5, 0.5, 1);
bool IsColorModel = false;

texture ModelTexture;
sampler2D textureSampler = sampler_state {
	Texture = (ModelTexture);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : SV_POSITION;
	float4 Normal : NORMAL0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float4 Normal : TEXCOORD0;
	float2 TextureCoordinate : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float4 normal = mul(input.Normal, WorldInverseTranspose);



	//float dot1 = max(0,dot(normal, DiffuseLightDirection1));
	//float dot2 = max(0,dot(normal, DiffuseLightDirection2));
	//float lightIntensity = dot1 + dot2;
	//output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);//saturate(float4(DiffuseColor.r * DiffuseIntensity * lightIntensity, DiffuseColor.g * DiffuseIntensity * lightIntensity, DiffuseColor.b * DiffuseIntensity * lightIntensity,1));
	output.TextureCoordinate = input.TextureCoordinate;
	output.Normal = normal;
	output.Color = float4(0,0,0,1);

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	//float4 textureColor = float4(1,1,1,1);
	//if (IsColorModel == true)
	//{
	//	textureColor = ModelColor;
	//}
	//else

	float f = clamp(dot(DiffuseLightDirection1, input.Normal), 0.3, 1);

	//input.Color.rgb += AmbientColor * AmbientIntensity;
	input.Color.rgb += f;
	input.Color *= tex2D(textureSampler, input.TextureCoordinate);
	input.Color.rgb = saturate(input.Color.rgb);
	input.Color.a = 1;

	//float4 ret = saturate(textureColor * (input.Color) + AmbientColor * AmbientIntensity);
	//ret.a = 1;

	//float4 ret = float4((textureColor.r * input.Color.r) + (AmbientColor.r * AmbientIntensity), (textureColor.g * input.Color.g) + (AmbientColor.g * AmbientIntensity), (textureColor.b * input.Color.b) + (AmbientColor.b * AmbientIntensity),1);
	//ret.a = 1;

	return input.Color;
}

technique Ambient
{
	pass Pass1
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}

