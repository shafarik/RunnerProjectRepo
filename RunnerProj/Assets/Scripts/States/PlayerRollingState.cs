using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollingState : PlayerState
{
    //CapsuleCollider _capsuleCollider;
    //public float RollingCollisionHeight = 1.5f;
    //public float RunningCollisionHeight = 3.75f;
    public PlayerRollingState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
       // _capsuleCollider = player.GetComponent<CapsuleCollider>();
    }

    public override void Enter()
    {
        base.Enter();

       // ChangeCollision(RollingCollisionHeight);
    }

    public override void Exit()
    {
        base.Exit();

       // ChangeCollision(RunningCollisionHeight);
    }

    public override void Update()
    {
        base.Update();
    }

    //private void ChangeCollision(float _targetHeight)
    //{
    //    float currentHeight = _capsuleCollider.height;
    //    float currentCenterY = _capsuleCollider.center.y;

    //    // Устанавливаем новую высоту
    //    _capsuleCollider.height = _targetHeight;

    //    // Корректируем центр коллайдера так, чтобы нижняя грань осталась на месте
    //    float heightDifference = (_targetHeight - currentHeight) / 2;
    //    _capsuleCollider.center = new Vector3(_capsuleCollider.center.x, currentCenterY + heightDifference, _capsuleCollider.center.z);
    //}
}
