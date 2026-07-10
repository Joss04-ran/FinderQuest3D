struct VS_INPUT {
    float3 pos : POSITION;
    float2 tex : TEXCOORD;
    float3 normal : NORMAL;
};

struct PS_INPUT {
    float4 pos : SV_POSITION;
    float2 tex : TEXCOORD0;
    float3 normal : NORMAL;
    float3 worldPos : POSITION;
};

cbuffer ConstantBuffer : register(b0) {
    matrix world;
    matrix view;
    matrix projection;
    float3 lightDirection;
    float padding;
};

Texture2D shaderTexture : register(t0);
SamplerState sampleState : register(s0);

PS_INPUT VS(VS_INPUT input) {
    PS_INPUT output;
    float4 localPos = float4(input.pos, 1.0f);
    float4 worldPos = mul(localPos, world);
    output.worldPos = worldPos.xyz;
    float4 viewPos = mul(worldPos, view);
    output.pos = mul(viewPos, projection);
    output.tex = input.tex;
    output.normal = mul(input.normal, (float3x3)world);
    return output;
}

float4 PS(PS_INPUT input) : SV_Target {
    float3 normal = normalize(input.normal);
    float3 invLightDir = normalize(-lightDirection);
    float diffuse = max(0.0f, dot(normal, invLightDir));
    
    // If it is a billboard sprite (XZ plane normal), keep it constant bright
    if (abs(normal.y) < 0.1f) {
        diffuse = 0.6f;
    }

    float ambient = 0.35f;
    float lightIntensity = saturate(diffuse + ambient);
    
    float4 textureColor = shaderTexture.Sample(sampleState, input.tex);
    
    // Discard transparent pixels to prevent depth writing issues
    if (textureColor.a < 0.1f) {
        discard;
    }

    return float4(textureColor.rgb * lightIntensity, textureColor.a);
}
