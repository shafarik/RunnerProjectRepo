using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.PlayerCharacter;
using Runner.CharacterState;
using Runner.StateMachine;
using Runner.CharacterMovingState;

namespace Runner.RollingState
{
    public class PlayerRollingState : MovingState
    {
        public PlayerRollingState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
        {

        }

        public override void Enter()
        {
            base.Enter();

            _player.ChangeCollisionToSlide();
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