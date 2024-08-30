using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.PlayerCharacter;
using Runner.CharacterState;
using Runner.StateMachine;
using Runner.CharacterMovingState;


namespace Runner.RunningState
{

    public class PlayerRunningState : MovingState
    {

        public PlayerRunningState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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
            base.Update();

        }
    }

}