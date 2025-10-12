using UnityEngine;
using UnityEngine.AI;

public class IdleState: State
{
    float timeBeforeSleep;

    protected override void OnEnter()
    {
        // "What was that!?"
        timeBeforeSleep = 5;

        Debug.Log("Idle State");

        RandomIdleAnimation();
    }

    protected override void OnUpdate()
    {
        // Search for player
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
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

    private void RandomIdleAnimation()
    {
        int randomIndex = Random.Range(1,5);
        animator.SetInteger("IdleIndex", randomIndex);
        animator.SetInteger("WalkIndex", 0);
    }
}