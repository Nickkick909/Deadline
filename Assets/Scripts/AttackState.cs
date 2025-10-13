using UnityEngine;
using UnityEngine.AI;

public class AttackState: State
{
    float timeBeforeSleep;

    protected override void OnEnter()
    {
        // "What was that!?"
        timeBeforeSleep = 5;

        Debug.Log("Attack State");
        sc.agent.ResetPath();
        sc.agent.isStopped = true;
        sc.RandomIdleAnimation();

        
    }

    protected override void OnUpdate()
    {
        //if (!sc.agent.pathPending)
        //{
        //    if (sc.agent.remainingDistance <= sc.agent.stoppingDistance)
        //    {
        //        if (!sc.agent.hasPath || sc.agent.velocity.sqrMagnitude == 0f)
        //        {
        //            sc.ChangeState(sc.idleState);
        //        }
        //    }
        //}
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