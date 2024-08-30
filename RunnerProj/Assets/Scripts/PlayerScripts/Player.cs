using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Runner.CharacterState;
using Runner.StateMachine;
using Runner.Controllable;
using Runner.Firebase;
using Runner.UImanager;
using Runner.DeathBoard;
using UnityEngine.SceneManagement;
using Runner.DeathState;
using Runner.JumpState;
using Runner.RunningState;
using Runner.RollingState;


namespace Runner.PlayerCharacter
{
    public class Player : MonoBehaviour, IControllable
    {
        [Header("Movement")]
        public float _basicMovementSpeed = 2;
        public float _moveSpeed;
        public float _movespeedMultiplier = 0.1f;
        [SerializeField] private float _lineRotationTime = 0.5f;

        [Space]
        [Header("Components")]
        [SerializeField] private FirebaseManager _firebaseManager;
        [SerializeField] private DeathBoardUIScript _deathBoardUI;
        public TMP_InputField ScoreField;

        [Header("Collisions")]
        [SerializeField] private CapsuleCollider _runningCollision;
        [SerializeField] private CapsuleCollider _slideCollision;
        [SerializeField] private CapsuleCollider _jumpCollision;
        [SerializeField] private GameObject[] _lineNumArray = new GameObject[3];


        [SerializeField] public Animator Animator { get; private set; }
        public PlayerStateMachine StateMachine { get; private set; }



        private Vector2 _touchStartPosition;
        private float _swipeThreshold = 50.0f;
        private bool _isTouching = false;

        public static bool _needPlayerToRun = false;


        #region States

        public PlayerIdleState IdleState { get; private set; }
        public PlayerRunningState RunningState { get; private set; }
        public PlayerRollingState RollingState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerDeathState DeathState { get; private set; }

        #endregion

        private int _currentLineNum = 1;
        private bool _isdead = false;

        private void Awake()
        {
            StateMachine = new PlayerStateMachine();
            Animator = this.GetComponentInChildren<Animator>();

            IdleState = new PlayerIdleState(this, StateMachine, "Idle");
            RunningState = new PlayerRunningState(this, StateMachine, "Run");
            RollingState = new PlayerRollingState(this, StateMachine, "Roll");
            JumpState = new PlayerJumpState(this, StateMachine, "Jump");
            DeathState = new PlayerDeathState(this, StateMachine, "Death");

        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Bool is " + _needPlayerToRun);

            if (_needPlayerToRun)
            {
                StateMachine.Initiaize(RunningState);
            }
            else
            {
                StateMachine.Initiaize(IdleState);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }


        public void StartRunning()
        {
            StateMachine.ChangeState(RunningState);
        }
        private void FixedUpdate()
        {
            MoveForward();
            StateMachine.CurrentState.Update();

            if (ScoreField.text != "")
            {
                if (int.Parse(ScoreField.text) < this.transform.position.x)
                {
                    ScoreField.text = (Math.Round(this.transform.position.x, 0)).ToString();
                }

            }
        }

        private void MoveForward()
        {
            if (StateMachine.CurrentState != IdleState && !_isdead)
            {
                _moveSpeed = _basicMovementSpeed + this.transform.position.x * _movespeedMultiplier;
                this.transform.position += new Vector3(1, 0, 0) * _basicMovementSpeed * Time.deltaTime;
            }
        }


        public void Move(Vector3 direction)
        {
            if (StateMachine.CurrentState == IdleState || StateMachine.CurrentState == DeathState) return;

            //  Debug.Log(" direction " + direction);

            if (Math.Round(this.transform.position.z, 1) == Math.Round(_lineNumArray[_currentLineNum].transform.position.z, 1))
            {
                //Debug.Log("Inputing");
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    // Горизонтальный свайп
                    if (direction.x > 0)
                    {
                        // Свайп вправо
                        if (_currentLineNum >= _lineNumArray.Length - 1) return;
                        _currentLineNum++;
                        this.transform.DOMoveZ(_lineNumArray[_currentLineNum].transform.position.z, _lineRotationTime);
                    }
                    else if (direction.x < 0)
                    {
                        // Свайп влево
                        if (_currentLineNum <= 0) return;
                        _currentLineNum--;
                        this.transform.DOMoveZ(_lineNumArray[_currentLineNum].transform.position.z, _lineRotationTime);
                    }
                }
                else
                {
                    // Вертикальный свайп
                    if (direction.y > 0)
                    {
                        // Свайп вверх
                        if (StateMachine.CurrentState != IdleState)
                        {
                            //ChangeCollision(_jumpCollision);
                            StateMachine.ChangeState(JumpState);
                        }
                    }
                    else if (direction.y < 0)
                    {
                        // Свайп вниз
                        if (StateMachine.CurrentState != IdleState)
                        {
                            //ChangeCollision(_slideCollision);
                            StateMachine.ChangeState(RollingState);
                        }
                    }
                }
            }
            // else
            //  Debug.Log("NotEqual " + Math.Round(this.transform.position.z, 1) + " != " + Math.Round(_lineNumArray[_currentLineNum].transform.position.z, 1));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Barrier"))
            {
                _deathBoardUI.ShowCanvas();
                StateMachine.ChangeState(DeathState);
            }

        }
        private void OnTriggerEnter(Collider other)
        {

        }

        public void RestartToMainMenu()
        {
            Scene currentScene = SceneManager.GetActiveScene();

            UnSetPlayerToRun();

            SceneManager.LoadScene(currentScene.name);
        }
        public void RestartToRunn()
        {
            Scene currentScene = SceneManager.GetActiveScene();

            SetPlayerToRun();

            SceneManager.LoadScene(currentScene.name);
        }

        public void ChangeCollision(CapsuleCollider _newCollision)
        {
            _runningCollision.enabled = false;
            _newCollision.enabled = true;
        }

        public void ResetCollision()
        {
            _jumpCollision.enabled = false;
            _slideCollision.enabled = false;
            _runningCollision.enabled = true;
        }

        public void ChangeCollisionToSlide()
        {
            _runningCollision.enabled = false;
            _slideCollision.enabled = true;
        }

        public void ChangeCollisionToJump()
        {
            _runningCollision.enabled = false;
            _jumpCollision.enabled = true;
        }
        public GameObject[] GetLinesArray()
        {
            return _lineNumArray;
        }

        public void SetIsDead(bool _dead)
        {
            _isdead = _dead;
        }

        public void SetPlayerToRun()
        {
            _needPlayerToRun = true;
        }

        public void UnSetPlayerToRun()
        {
            _needPlayerToRun = false;
        }


    }
}
