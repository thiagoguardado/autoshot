using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FiniteStateMachines
{

    public abstract class State<AgentType>
    {
        public AgentType Agent;
        public FiniteStateMachine<AgentType> StateMachine;
        public List<Transition<AgentType>> Transitions = new List<Transition<AgentType>>();
        public List<int> TriggeredEvents = new List<int>();

        public virtual void Enter()
        {
        }

        public virtual void Update()
        {

        }

        public virtual void Exit()
        {
        }

        public virtual void TriggerEvent(int triggerEvent)
        {
        }

        public void AddTransition(Transition<AgentType> transition)
        {
            Transitions.Add(transition);
        }

        public void AddTransition(
            int trigger,
            Transition<AgentType>.Condition condition,
            State<AgentType> nextState)
        {
            var transition = new Transition<AgentType>(trigger, condition, nextState);
            AddTransition(transition);
        }

        public void AddCondition(
            Transition<AgentType>.Condition condition,
            State<AgentType> nextState)
        {
            AddTransition(Transition<AgentType>.NoTrigger, condition, nextState);
        }

        public void AddTrigger(int trigger, State<AgentType> nextState)
        {
            AddTransition(trigger, null, nextState);
        }

        public void CheckTransitions()
        {
            foreach (var transition in Transitions)
            {
                if (transition.Check(TriggeredEvents))
                {
                    StateMachine.SetState(transition.NextState);
                    return;
                }
            }
        }
    }

}
