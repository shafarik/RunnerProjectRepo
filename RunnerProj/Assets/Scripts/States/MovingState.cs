using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.CharacterState;
using Runner.PlayerCharacter;
using Runner.StateMachine;

namespace Runner.CharacterMovingState
{

    public class MovingState : PlayerState
    {
        public MovingState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            _player._moveSpeed = _player._basicMovementSpeed + _player.transform.position.x * _player._movespeedMultiplier;
            _player.transform.position += new Vector3(1, 0, 0) * _player._basicMovementSpeed * Time.deltaTime;
        }
    }

}