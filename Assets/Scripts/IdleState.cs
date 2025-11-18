using UnityEngine;
using UnityEngine.AI;

public class IdleState: State
{
    float timeBeforeSleep;

    protected override void OnEnter()
    {
        sc.footsteps.Pause();
        // "What was that!?"
        timeBeforeSleep = UnityEngine.Random.Range(3f, 10f);

        Debug.Log("Idle State");

        sc.RandomIdleAnimation();
    }

    protected override void OnUpdate()
    {
        // Search for player
        if (sc.DetectPlayer(sc.detectionRadius))
        {
            sc.ChangeState(sc.chaseState);
            return;
        }

        if (timeBeforeSleep <= 0)
        //if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        { 
            sc.ChangeState(sc.patrolState);
        }

        timeBeforeSleep -= Time.deltaTime;
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