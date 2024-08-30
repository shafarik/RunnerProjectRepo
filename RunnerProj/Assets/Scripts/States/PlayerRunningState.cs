using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.PlayerCharacter;
using Runner.CharacterState;
using Runner.StateMachine;
public class PlayerRunningState : PlayerState
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
