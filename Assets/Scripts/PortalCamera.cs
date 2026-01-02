using UnityEngine;

public class PortalCamera : MonoBehaviour
{

    public Transform playerCam;
    public Transform portalDestination;
    public Transform portalOrigin;

    public Transform playerYawSource;
    public Transform playerPitchSource;

    public bool matchRotation = true;

    public float paralaxFactor = 0.5f;

    public float yOffset = 1f;


    public Material portalMat;
    public Camera portalCam;

    void LateUpdate()
    {

        // 1. Calculate player's position relative to the portal origin
        Vector3 relativePos = portalOrigin.InverseTransformPoint(playerCam.position);

        // Apply parallax scaling
        float parallaxFactor = paralaxFactor; // <1 = background moves slower
        relativePos.z *= parallaxFactor;

        transform.position = portalDestination.TransformPoint(relativePos) + new Vector3(0, yOffset, 0);


        // 3. Match rotation (simple & stable version)
        if (matchRotation)
        {
            // Yaw only (from PLAYER ROOT direction)
            float yaw = playerYawSource.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);

            float pitch = playerPitchSource.localEulerAngles.x;
            transform.localRotation = Quaternion.Euler(-pitch, 0f, 0f);
        }
    }

    private void Start()
    {
        if (portalCam.targetTexture != null)
        {
            portalCam.targetTexture.Release();
        }

        portalCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

        portalMat.mainTexture = portalCam.targetTexture;
    }

}
