using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{

    public float range = 10f;

    protected override void OnEnter()
    {
        // "What was that!?"

        Debug.Log("Patrol State");

        MoveToRandomLocation();


    }

    protected override void OnUpdate()
    {
        // Search for player
        //if (Physics.Raycast(sc.transform.position, sc.transform.forward))
        //{
        //    //sc.ChangeState(sc.chaseState);
        //} else 
        if (!sc.agent.pathPending)
        {
            if (sc.agent.remainingDistance <= sc.agent.stoppingDistance)
            {
                if (!sc.agent.hasPath || sc.agent.velocity.sqrMagnitude == 0f)
                {
                    sc.ChangeState(sc.idleState);
                }
            }
        }

    }

    public void MoveToRandomLocation()
    {
        RandomWalkAnimation();
        Vector3 randomPoint = sc.transform.position + Random.insideUnitSphere * range;
        randomPoint.y = 0;
        
        NavMeshHit hit;

        // Check if the random point is on the NavMesh
        if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
        {
            sc.agent.SetDestination(hit.position);
        }
        else
        {
            Debug.Log("No valid NavMesh point found.");
        }
    }

    protected override void OnHurt()
    {
        // Transition to Hurt State
        //sc.ChangeState(sc.hurtState);

    }
    protected override void OnExit()
    {
        // "Must've been the wind"
    }

    private void RandomWalkAnimation()
    {
        int randomIndex = Random.Range(1, 5);
        animator.SetInteger("WalkIndex", randomIndex);
        animator.SetInteger("IdleIndex", 0);

    }
}