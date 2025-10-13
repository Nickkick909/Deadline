using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateController : MonoBehaviour
{
    [SerializeField] State currentState;

    public PatrolState patrolState = new PatrolState();
    public IdleState idleState = new IdleState();
    public ChaseState chaseState = new ChaseState();
    public AttackState attackState = new AttackState();

    public NavMeshAgent agent;

    public Animator animator;

    public GameObject player;
    public float fieldOfViewAngle = 120f; // Field of view in degrees
    public const float detectionRadius = 10f; // Radius of detection

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

    public bool DetectPlayer(float detectionRadius = detectionRadius)
    {
        RaycastHit hit; // Stores information about the hit
        Vector3 direction = transform.TransformDirection(Vector3.forward); // Ray direction
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        // Calculate direction to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if the player is within the detection radius
        if (distanceToPlayer <= detectionRadius)
        {
            // Check if the player is within the field of view
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer <= fieldOfViewAngle / 2)
            {
                Debug.Log("Detected Player!!");
                return true;
            }

                return false;
        }
        return false;

        //// Cast a ray forward from the object's position
        //if (Physics.Raycast(transform.position, directionToPlayer, out hit, 10f))
        //{
        //    Debug.Log($"Hit {hit.collider.gameObject.name} at distance {hit.distance}");
        //    return true;
        //} else
        //{
        //    return false;
        //}
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
