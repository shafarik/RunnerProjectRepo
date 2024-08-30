using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.PlayerCharacter;
using Runner.StateMachine;

namespace Runner.CharacterState
{
    public class PlayerState
    {
        protected PlayerStateMachine _stateMachine;
        protected Player _player;

        protected string _animBoolName;

        public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
        {
            _stateMachine = stateMachine;
            _player = player;
            _animBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            _player.Animator.SetBool(_animBoolName, true);
        }

        public virtual void Update()
        {

        }

        public virtual void Exit()
        {
            _player.Animator.SetBool(_animBoolName, false);

        }
    }
}
