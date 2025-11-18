using System.Collections;
using UnityEngine;

public class SkyboxTransition : MonoBehaviour
{
    [Header("Skybox Materials")]
    public Material blendSkyboxMaterial; // Assign the shader material
    public Cubemap sunsetSky;
    public Cubemap nightSky;

    [Header("Optional Lighting")]
    public Light sun;
    public Color sunsetAmbient = Color.white;
    public Color nightAmbient = Color.black;
    public float sunsetIntensity = 1f;
    public float nightIntensity = 0.2f;

    [Header("Transition Settings")]
    public float duration = 5f;

    private void Start()
    {
        // Assign the cubemaps to the shader
        blendSkyboxMaterial.SetTexture("_Sky1", sunsetSky);
        blendSkyboxMaterial.SetTexture("_Sky2", nightSky);
        blendSkyboxMaterial.SetFloat("_Blend", 0f);

        RenderSettings.skybox = blendSkyboxMaterial;
        DynamicGI.UpdateEnvironment();
        StartTransition();
    }

    public void StartTransition()
    {
        StartCoroutine(FadeSkybox());
    }

    IEnumerator FadeSkybox()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float blend = Mathf.Clamp01(t / duration);
            blendSkyboxMaterial.SetFloat("_Blend", blend);

            DynamicGI.UpdateEnvironment();
            yield return null;
        }

        blendSkyboxMaterial.SetFloat("_Blend", 1f);
        RenderSettings.ambientLight = nightAmbient;

        DynamicGI.UpdateEnvironment();
    }
}
