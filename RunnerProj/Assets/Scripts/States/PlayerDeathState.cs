using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.PlayerCharacter;
using Runner.CharacterState;
using Runner.StateMachine;
public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        _player.SetIsDead(true);
    }

    public override void Exit()
    {
        base.Exit();

        _player.SetIsDead(false);
    }

    public override void Update()
    {
        base.Update();
    }
}
