using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runner.Controllable;

namespace Runner.InputController
{


    public class CharacterInputController : MonoBehaviour
    {
        private IControllable _controllable;
        private GameInput _gameInput;

        private void Awake()
        {
            _gameInput = new GameInput();
            _gameInput.Enable();

            _controllable = GetComponent<IControllable>();
        }

        private void ReadMovement()
        {
            Vector2 InputDirection = _gameInput.RunnerGameInput.Input.ReadValue<Vector2>();
            Vector3 Direction = new Vector3(InputDirection.x, InputDirection.y, 0f);

            _controllable.Move(Direction);
        }

        private void Update()
        {
            ReadMovement();
        }

    }
}