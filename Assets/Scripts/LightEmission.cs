using UnityEngine;

public class LightEmission : MonoBehaviour
{
    public Light lightSource;
    public Material material;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lightSource.intensity <= 0.1)
        {
            // Disable the emission keyword
            material.DisableKeyword("_EMISSION");
            //// Set the emission color to black
            //material.SetColor("_EmissionColor", Color.black);
            //// Optionally, set global illumination flags
            //material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
        } else
        {
            material.EnableKeyword("_EMISSION");
        }
    }


}