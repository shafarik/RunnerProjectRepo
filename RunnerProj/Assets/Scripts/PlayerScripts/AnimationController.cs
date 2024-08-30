using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.PlayerCharacter;


namespace Runner.AnimationControl
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private Player _player;

        // Start is called before the first frame update
        void Start()
        {

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
}