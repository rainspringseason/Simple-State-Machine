using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T> : IState<T> where T : MonoBehaviour
{
    protected StateMachine<T> stateMachine;
    protected T owner;

    public void Initialize(StateMachine<T> stateMachine, T owner)
    {
        this.stateMachine = stateMachine;
        this.owner = owner;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }

    public abstract void CheckTransitions();
}
