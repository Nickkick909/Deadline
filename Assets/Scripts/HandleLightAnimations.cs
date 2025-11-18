using UnityEngine;

public class HandleLightAnimations : MonoBehaviour
{

    public EnemyStateController fourthFloorMonster;
    public AudioSource lightHum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightHum.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RedLightsOn()
    {
        fourthFloorMonster.LightsOn();
        lightHum.UnPause();
    }

    public void RedLightsOff()
    {
        fourthFloorMonster.LightsOff();
        lightHum.Pause();
    }
}
