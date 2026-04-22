using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T> where T : MonoBehaviour
{
    void OnEnter();
    void OnExit();
    void OnUpdate();
    void OnFixedUpdate();
    void CheckTransitions();
}

public class StateMachine<T> where T : MonoBehaviour
{
    private Dictionary<Type, IState<T>> states;
    private IState<T> currentState;
    private T owner;

    public IState<T> CurrentState => currentState;

    public StateMachine(T owner)
    {
        this.owner = owner;
        states = new Dictionary<Type, IState<T>>();
    }

    public void AddState(IState<T> state)
    {
        states[state.GetType()] = state;
    }

    public void Initialize<TState>() where TState : IState<T>
    {
        ChangeState<TState>();
    }

    public void ChangeState<TState>() where TState : IState<T>
    {
        var stateType = typeof(TState);

        if (!states.ContainsKey(stateType))
            return;

        if (currentState?.GetType() == stateType)
            return;

        currentState?.OnExit();
        currentState = states[stateType];
        currentState?.OnEnter();
    }

    public void Update()
    {
        currentState?.CheckTransitions();
        currentState?.OnUpdate();
    }

    public void FixedUpdate()
    {
        currentState?.OnFixedUpdate();
    }
}
