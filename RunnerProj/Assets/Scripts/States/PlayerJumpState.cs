using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.PlayerCharacter;
using Runner.StateMachine;
using Runner.CharacterState;
public class PlayerJumpState : PlayerState
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
