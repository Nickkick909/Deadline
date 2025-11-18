#pragma multi_compile _ _LIGHT_LAYERS

// This shader function calculates total light contribution for reveal
// Always ensures the "FaceHighlight" layer light contributes

void RevealUsingLayer_float(float3 WorldPosition, float UltravioletLayerIndex, out float Out)
{
    half4 Shadowmask = half4(1, 1, 1, 1);
    float totalAtten = 0;

#ifndef SHADERGRAPH_PREVIEW

    uint pixelLightCount = GetAdditionalLightsCount();
    uint ultravioletLayerMask = 1 << int(UltravioletLayerIndex);

    InputData inputData = (InputData) 0;
    float4 screenPos = ComputeScreenPos(TransformWorldToHClip(WorldPosition));
    inputData.normalizedScreenSpaceUV = screenPos.xy / screenPos.w;
    inputData.positionWS = WorldPosition;

    // --- Loop over normal lights ---
    LIGHT_LOOP_BEGIN(pixelLightCount)

    Light light = GetAdditionalLight(lightIndex, WorldPosition, Shadowmask);

        #ifdef _LIGHT_LAYERS
        if (IsMatchingLightLayer(light.layerMask, ultravioletLayerMask))
        #endif
        {
        float intensity = length(light.color.rgb);
        float atten = intensity * light.distanceAttenuation * light.shadowAttenuation;
        totalAtten += atten;
    }

    LIGHT_LOOP_END

    // --- Loop over guaranteed face highlight light ---
    // Assign this light to its own layer, e.g., layer 8 (FaceHighlight)
    const uint faceHighlightLayerMask = 1 << 8;

    LIGHT_LOOP_BEGIN(pixelLightCount)

    Light light = GetAdditionalLight(lightIndex, WorldPosition, Shadowmask);

        #ifdef _LIGHT_LAYERS
        if (IsMatchingLightLayer(light.layerMask, faceHighlightLayerMask))
        #endif
        {
        float intensity = length(light.color.rgb);
        float atten = intensity * light.distanceAttenuation * light.shadowAttenuation;
        totalAtten += atten * 5.0;
    }

    LIGHT_LOOP_END

#endif

    Out = totalAtten;
}
