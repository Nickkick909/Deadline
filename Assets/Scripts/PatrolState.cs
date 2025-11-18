using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{

    public float range = 10f;

    protected override void OnEnter()
    {
        sc.footsteps.UnPause();
        // "What was that!?"

        Debug.Log("Patrol State");

        //MoveToRandomLocation();
        MoveToNextPoint();

    }

    protected override void OnUpdate()
    {
        // Search for player
        if (sc.DetectPlayer(sc.detectionRadius))
        {
            sc.ChangeState(sc.chaseState);
            return;
        }

        if (!sc.agent.pathPending)
        {
            if (sc.agent.remainingDistance <= sc.agent.stoppingDistance)
            {
                sc.patrolPointIndex += 1;

                if (sc.patrolPointIndex > sc.monsterPatrolPoints.Length - 1)
                {
                    sc.patrolPointIndex = 0;
                }

                if (!sc.agent.hasPath || sc.agent.velocity.sqrMagnitude == 0f)
                {
                    sc.ChangeState(sc.idleState);
                }
            }
        }

    }

    public void MoveToRandomLocation()
    {
        sc.RandomWalkAnimation();
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

    public void MoveToNextPoint()
    {
        sc.RandomWalkAnimation();
        Vector3 nextPoint = sc.monsterPatrolPoints[sc.patrolPointIndex].position;

        sc.agent.SetDestination(nextPoint);


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

    
}