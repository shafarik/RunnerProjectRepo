using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.PlayerCharacter;
using Runner.StateMachine;
using Runner.CharacterState;
using Runner.CharacterMovingState;

namespace Runner.JumpState
{

    public class PlayerJumpState : MovingState
    {

        public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
        {

        }

        public override void Enter()
        {
            base.Enter();

            _player.ChangeCollisionToJump();
        }

        public override void Exit()
        {
            base.Exit();

            _player.ResetCollision();
        }

        public override void Update()
        {
            base.Update();
        }

    }

}