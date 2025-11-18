using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateController : MonoBehaviour
{
    [SerializeField] State currentState;

    public Transform monsterEyes;

    public PatrolState patrolState = new PatrolState();
    public IdleState idleState = new IdleState();
    public ChaseState chaseState = new ChaseState();
    public AttackState attackState = new AttackState();

    public NavMeshAgent agent;

    public Animator animator;

    public GameObject player;
    public float fieldOfViewAngle = 120f; // Field of view in degrees
    public float detectionRadius = 10f; // Radius of detection

    public float lightsOnFOV = 300f;
    public float lightsOnRadius = 8f;

    public float lightsOffFOV = 45f;
    public float lightsOffRadius = 2f;

    public Transform[] monsterPatrolPoints;
    public int patrolPointIndex = 0;

    bool lightsOn = false;

    public AudioSource footsteps;

    private void Start()
    {
        ChangeState(patrolState);
    }

    void Update()
    {
        if (currentState != null)
        {
            //if (currentState != chaseState)
            //{
            //    bool detectedPlayer = DetectPlayer();
            //    if (detectedPlayer)
            //    {
            //        ChangeState(chaseState);
            //    }
            //    else
            //    {
            //        currentState.OnStateUpdate();
            //    }
            //} else
            //{
                currentState.OnStateUpdate();
            //}

        }

        UpdateMonsterVision();
    }

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }
        currentState = newState;
        currentState.OnStateEnter(this, animator);
    }

    public bool DetectPlayer(float detection)
    {
        RaycastHit hit; // Stores information about the hit
        Vector3 direction = transform.TransformDirection(Vector3.forward); // Ray direction
        Vector3 directionToPlayer = ((player.transform.position + new Vector3(0, 0.35f, 0)) - monsterEyes.position).normalized;
        // Calculate direction to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if the player is within the detection radius
        if (distanceToPlayer <= detection)
        {
            // Check if the player is within the field of view
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer <= fieldOfViewAngle / 2)
            {
                Debug.DrawRay(monsterEyes.position, directionToPlayer * detection, Color.red);


                // Cast a ray forward from the object's position
                if (Physics.Raycast(monsterEyes.position, directionToPlayer, out hit, detection))
                {

                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        Debug.Log("Detected Player");
                        return true;
                    } else
                    {
                        Debug.Log("Player hidden");
                        return false;
                    }
                    
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
        return false;

        
    }


    public void LightsOn()
    {
        lightsOn = true;
    }

    public void LightsOff()
    {
        lightsOn = false;
    }

    void UpdateMonsterVision()
    {
        if (OffsetFlashlight.playerFlashlight.IsOn || lightsOn)
        {
            fieldOfViewAngle = lightsOnFOV;
            detectionRadius = lightsOnRadius;
        } else
        {
            fieldOfViewAngle = lightsOffFOV;
            detectionRadius = lightsOffRadius;
        }
    }
    public void RandomWalkAnimation()
    {
        int randomIndex = UnityEngine.Random.Range(1, 4);
        animator.SetInteger("WalkIndex", randomIndex);
        animator.SetInteger("IdleIndex", 0);

    }

    public void RandomIdleAnimation()
    {
        int randomIndex = UnityEngine.Random.Range(1, 5);
        animator.SetInteger("IdleIndex", randomIndex);
        animator.SetInteger("WalkIndex", 0);
    }
}

[Serializable]
public abstract class State
{
    public string StateName;
    public EnemyStateController sc;
    public Animator animator;

    public void OnStateEnter(EnemyStateController stateController, Animator anim)
    {
        // Code placed here will always run
        sc = stateController;
        animator = anim;
        OnEnter();
    }

    protected virtual void OnEnter()
    {
        // Code placed here can be overridden
    }

    public void OnStateUpdate()
    {
        // Code placed here will always run
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        // Code placed here can be overridden
    }

    public void OnStateHurt()
    {
        // Code placed here will always run
        OnHurt();
    }

    protected virtual void OnHurt()
    {
        // Code placed here can be overridden
    }

    public void OnStateExit()
    {
        // Code placed here will always run
        OnExit();
    }

    protected virtual void OnExit()
    {
        // Code placed here can be overridden
    }
}
