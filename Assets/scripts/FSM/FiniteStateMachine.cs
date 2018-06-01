using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiniteStateMachines
{

    public class FiniteStateMachine<AgentType>
    {
        public AgentType Agent;

        private State<AgentType> _CurrentState;
        List<int> triggeredEvents = new List<int>();


        public FiniteStateMachine(AgentType agent)
        {
            Agent = agent;
        }

        public void SetState(State<AgentType> state)
        {
            if (_CurrentState != null)
            {
                _CurrentState.Exit();
            }
            _CurrentState = state;
            if (_CurrentState != null)
            {
                _CurrentState.Agent = Agent;
                _CurrentState.StateMachine = this;
                _CurrentState.Enter();
            }
        }

        public void Update()
        {
            if (_CurrentState != null)
            {
                _CurrentState.TriggeredEvents = triggeredEvents;
                _CurrentState.CheckTransitions();
                triggeredEvents.Clear();
                if (_CurrentState != null)
                {
                    _CurrentState.Update();
                }
            }
            else
            {
                triggeredEvents.Clear();
            }

            //CheckTransitions may change state
            
        }

        public void TriggerEvent(int triggerEvent)
        {

            if (_CurrentState != null)
            {
               triggeredEvents.Add(triggerEvent);
               //_CurrentState.TriggerEvent(triggerEvent);
            }
        }
    }


}