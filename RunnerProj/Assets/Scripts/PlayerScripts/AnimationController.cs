using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnAnimationEnd()
    {
        _player.ResetCollision();
        _player.StateMachine.ChangeState(_player.RunningState);
    }
}
