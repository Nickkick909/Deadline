using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateController : MonoBehaviour
{
    [SerializeField] State currentState;

    public PatrolState patrolState = new PatrolState();
    public IdleState idleState = new IdleState();

    public NavMeshAgent agent;

    public Animator animator;

    private void Start()
    {
        ChangeState(patrolState);
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.OnStateUpdate();
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
