using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.CharacterState;


namespace Runner.StateMachine
{
    public class PlayerStateMachine
    {

        public PlayerState CurrentState { get; private set; }

        public void Initiaize(PlayerState _newState)
        {
            CurrentState = _newState;
            CurrentState.Enter();
        }

        public void ChangeState(PlayerState _newState)
        {
            CurrentState.Exit();
            CurrentState = _newState;
            CurrentState.Enter();
        }
    }
}