using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiniteStateMachines
{
    public class Transition<AgentType>
    {
        public static readonly int NoTrigger = -1;

        private int _Trigger = -1;
        public Condition _Condition = null; 
        public State<AgentType> NextState;

        public delegate bool Condition();

        public Transition(
            int trigger,
            Condition condition,
            State<AgentType> nextState)
        {
            _Trigger = trigger;
            _Condition = condition;
            NextState = nextState;
        }

        public bool Check(List<int> triggers)
        {
            if (triggers.Contains(_Trigger) || _Trigger == NoTrigger)
            {
                if (_Condition != null)
                {
                    if (_Condition())
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}