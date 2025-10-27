using UnityEngine;

//[ExecuteInEditMode]
public class LightEmission : MonoBehaviour
{
    public Light lightSource;
    //public Material material;
    MeshRenderer mr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lightSource == null)
        {
            mr.material.DisableKeyword("_EMISSION");
            return;
        }

        if (lightSource.intensity <= 0.1)
        {
            // Disable the emission keyword
            //material.DisableKeyword("_EMISSION");

            mr.material.DisableKeyword("_EMISSION");
            lightSource.enabled = false;
            //// Set the emission color to black
            //material.SetColor("_EmissionColor", Color.black);
            //// Optionally, set global illumination flags
            //material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
        } else
        {
            //material.EnableKeyword("_EMISSION");
            mr.material.EnableKeyword("_EMISSION");
            lightSource.enabled = true;

        }
    }


}