using UnityEngine;
using UnityEngine.AI;

public class ChaseState: State
{
    float timeBeforeSleep;

    protected override void OnEnter()
    {
        sc.footsteps.UnPause();

        // "What was that!?"
        timeBeforeSleep = 5;

        Debug.Log("Chase State");

        //RandomIdleAnimation();

        sc.agent.SetDestination(sc.player.transform.position);
        sc.RandomWalkAnimation();
    }

    protected override void OnUpdate()
    {
        if (sc.DetectPlayer(1f))
        {
            sc.ChangeState(sc.attackState);
            return;
        }

        if (!sc.agent.pathPending)
        {
            if (sc.agent.remainingDistance <= sc.agent.stoppingDistance)
            {
                if (!sc.agent.hasPath || sc.agent.velocity.sqrMagnitude == 0f)
                {
                    if (sc.DetectPlayer(sc.detectionRadius))
                    {
                        sc.ChangeState(sc.chaseState);
                        return;
                    }

                    sc.ChangeState(sc.idleState);
                }
            }
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
}