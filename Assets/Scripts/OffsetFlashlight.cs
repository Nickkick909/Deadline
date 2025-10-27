using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OffsetFlashlight : MonoBehaviour
{

    public GameObject followCam;
    [SerializeField] private float moveSpeed = 10f;

    public Light flashlight;
    private bool isOn = false;

    // Audio
    public AudioSource audioSource;
    public AudioClip onSound;
    public AudioClip offSound;

    private InputAction flashlightButton;

    public float lightRange = 25f;

    public float hit1Dist = 0;
    public float hit2Dist = 0;
    public float hit3Dist = 0;
    public float hit4Dist = 0;

    public float smoothTime = 0.5f; // Adjust for smoothness
    private float velocityI = 0f;
    private float velocityR = 0f;

    public float flashlightBattery = 100f;
    public float batteryDrainSpeed = 1f;
    [SerializeField] Image flashlightUIBar;

    public LayerMask ignoreLayer;

    public static OffsetFlashlight playerFlashlight;

    public bool dontFollowCam = false;
    private void Awake()
    {
        playerFlashlight = this;
        flashlightButton = InputSystem.actions.FindAction("Flashlight");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flashlight.range = lightRange;
        isOn = flashlight.enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (dontFollowCam)
        {
            return;
        }

        transform.position = followCam.transform.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, followCam.transform.rotation, moveSpeed * Time.deltaTime);

        if (flashlightButton.WasPressedThisFrame())
        {
            if (isOn)
            {
                flashlight.enabled = false;
                isOn = false;

                audioSource.PlayOneShot(offSound);
            }
            else
            {
                flashlight.enabled = true;
                isOn = true;

                audioSource.PlayOneShot(onSound);
            }
        }


        if (isOn)
        {
            // Drain battery if flashlight is on
            flashlightBattery -= batteryDrainSpeed * Time.deltaTime;

            if (flashlightBattery < 0)
            {
                flashlightBattery = 0;
            } else if (flashlightBattery > 100)
            {
                flashlightBattery = 100;
            }

            if (flashlightBattery <= 25)
            {
                flashlightUIBar.color = Color.red;
            }
            else
            {
                 flashlightUIBar.color = Color.green;
            }

            flashlightUIBar.fillAmount = flashlightBattery / 100;

            // Cast a ray to the center
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, lightRange, ~ignoreLayer);
            hit1Dist = hit.distance;

            float maxHitDist = 0;
            if (hit.transform != null && hit.distance < lightRange)
            {
                // If hit something AND hit is less than our max light range
               maxHitDist = hit.distance;

                // Cast ray to the right (at roughly same angle as flashlight cone
                RaycastHit hit2;
                Physics.Raycast(transform.position , transform.forward + (0.45f * transform.right), out hit2, lightRange, ~ignoreLayer);

                hit2Dist = hit2.distance;

                if (hit2.transform != null && hit2.distance < lightRange)
                {
                    // If hit something AND hit is less than our max light range
                    if (hit2.distance > maxHitDist)
                    {
                        maxHitDist = hit2.distance;
                    }

                    // Cast ray to the left (at roughly same angle as flashlight cone
                    RaycastHit hit3;
                    Physics.Raycast(transform.position, transform.forward - (0.45f * transform.right), out hit3, lightRange, ~ignoreLayer);

                    hit3Dist = hit3.distance;

                    if (hit3.transform != null && hit3.distance < lightRange)
                    {
                        // If hit something AND hit is less than our max light range
                        if (hit3.distance > maxHitDist)
                        {
                            maxHitDist = hit3.distance;
                        }

                        // Cast ray upward (just in case we are looking at a short object like a kitchen counter)
                        RaycastHit hit4;
                        Physics.Raycast(transform.position, transform.forward + (0.45f * transform.up), out hit4, lightRange, ~ignoreLayer);

                        hit4Dist = hit4.distance;

                        if (hit4.transform != null && hit4.distance < lightRange)
                        {
                            // If hit something AND hit is less than our max light range

                            if (hit4.distance > maxHitDist)
                            {
                                maxHitDist = hit4.distance;
                            }

                            // Calculate the furthest point a ray hit, add a bit to it, and restrict it to min and max
                            float desiredFlashRange = Mathf.Clamp(maxHitDist + (maxHitDist / 2), 5f, 35); 
                            float desiredFlashIntensity = Mathf.Clamp(maxHitDist + (maxHitDist / 2), 5f, 15);

                            // Set flashlight Intesity (brightness) and Range
                            flashlight.intensity = Mathf.SmoothDamp(flashlight.intensity, desiredFlashIntensity, ref velocityI, smoothTime);
                            flashlight.range = Mathf.SmoothDamp(flashlight.range, desiredFlashRange, ref velocityR, smoothTime);

                        }
                        else
                        {
                            // If missed then set to max range.... There are more of these below...
                            flashlight.range = lightRange;
                        }
                    } else
                    {
                        flashlight.range = lightRange;
                    }
                } else
                {
                    flashlight.range = lightRange;
                }

            } else
            {
                flashlight.range = lightRange;
            }


            Debug.DrawRay(transform.position, lightRange * (transform.forward), Color.red);
            Debug.DrawRay(transform.position, lightRange * (transform.forward - (0.45f * transform.right)), Color.red);
            Debug.DrawRay(transform.position, lightRange * (transform.forward + (0.45f * transform.right)), Color.red);

            //Debug.Log("Hit: " + hit.transform.gameObject.name);

            //if (hit.transform != null)
            //{
            //    Debug.Log(hit.distance);

            //    flashlight.range = Mathf.Clamp(hit.distance + (hit.distance / 2), 5, 25);

            //}
            //else
            //{
            //    flashlight.range = lightRange;
            //}
        }
    }

    public void TurnOffFlashlight()
    {
        isOn = false;
        flashlight.enabled = false;
    }

    public void TurnOnFlashlight()
    {
        isOn = true;
        flashlight.enabled = true;
    }

    public bool IsOn { get { return isOn; } }
}
