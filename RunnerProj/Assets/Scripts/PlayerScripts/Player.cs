using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _basicMovementSpeed = 2;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float _movespeedMultiplier = 0.1f;

    [Space]
    [Header("Components")]
    [SerializeField] private FirebaseManager _firebaseManager;
    public TMP_InputField ScoreField;

    [Header("Collisions")]
    [SerializeField] private CapsuleCollider _runningCollision;
    [SerializeField] private CapsuleCollider _slideCollision;
    [SerializeField] private CapsuleCollider _jumpCollision;
    [SerializeField] private GameObject[] _lineNumArray = new GameObject[3];


    public Animator _animator { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }



    private Vector2 touchStartPosition;
    private float swipeThreshold = 50.0f;
    private bool isTouching = false;


    #region States

    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunningState RunningState { get; private set; }
    public PlayerRollingState RollingState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDeathState DeathState { get; private set; }

    #endregion

    private int _currentLineNum = 1;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        _animator = this.GetComponentInChildren<Animator>();

        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        RunningState = new PlayerRunningState(this, StateMachine, "Run");
        RollingState = new PlayerRollingState(this, StateMachine, "Roll");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        DeathState = new PlayerDeathState(this, StateMachine, "Death");
    }

    // Start is called before the first frame update
    void Start()
    {
        StateMachine.Initiaize(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentState.Update();


#if UNITY_STANDALONE || UNITY_EDITOR
        // Обработка ввода с клавиатуры для ПК
        HandleKeyboardInput();
        HandleMouseInput();
#endif

#if UNITY_IOS || UNITY_ANDROID
        // Обработка тач ввода для мобильных устройств
        HandleTouchInput();
#endif
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.D) && StateMachine.CurrentState != IdleState && StateMachine.CurrentState != DeathState)
        {
            if (_currentLineNum >= _lineNumArray.Length - 1) return;
            _currentLineNum++;
            this.transform.DOMoveZ(_lineNumArray[_currentLineNum].transform.position.z, 1.0f);
        }

        if (Input.GetKeyDown(KeyCode.A) && StateMachine.CurrentState != IdleState && StateMachine.CurrentState != DeathState)
        {
            if (_currentLineNum <= 0) return;
            _currentLineNum--;
            this.transform.DOMoveZ(_lineNumArray[_currentLineNum].transform.position.z, 1.0f);
        }
        if (Input.GetKeyDown(KeyCode.S) && StateMachine.CurrentState != IdleState && StateMachine.CurrentState != DeathState)
        {
            ChangeCollision(_slideCollision);
            StateMachine.ChangeState(RollingState);
        }
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    UIManager.instance.ClearScreen();
        //    StateMachine.ChangeState(RunningState);

        //}
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.W) && StateMachine.CurrentState != IdleState && StateMachine.CurrentState != DeathState)
        {
            ChangeCollision(_jumpCollision);
            StateMachine.ChangeState(JumpState);
        }
    }

    private void HandleTouchInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPosition = Input.mousePosition;
            isTouching = true;
        }

        if (Input.GetMouseButtonUp(0) && isTouching && StateMachine.CurrentState == RunningState)
        {
            isTouching = false;
            Vector2 touchEndPosition = Input.mousePosition;
            Vector2 swipeDelta = touchEndPosition - touchStartPosition;

            if (swipeDelta.magnitude > swipeThreshold )
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    // Горизонтальный свайп
                    if (swipeDelta.x > 0)
                    {
                        // Свайп вправо
                        if (_currentLineNum >= _lineNumArray.Length - 1) return;
                        _currentLineNum++;
                        this.transform.DOMoveZ(_lineNumArray[_currentLineNum].transform.position.z, 1.0f);
                    }
                    else
                    {
                        // Свайп влево
                        if (_currentLineNum <= 0) return;
                        _currentLineNum--;
                        this.transform.DOMoveZ(_lineNumArray[_currentLineNum].transform.position.z, 1.0f);
                    }
                }
                else
                {
                    // Вертикальный свайп
                    if (swipeDelta.y > 0)
                    {
                        // Свайп вверх
                        if (StateMachine.CurrentState != IdleState)
                        {
                            ChangeCollision(_jumpCollision);
                            StateMachine.ChangeState(JumpState);
                        }
                    }
                    else
                    {
                        // Свайп вниз
                        if (StateMachine.CurrentState != IdleState)
                        {
                            ChangeCollision(_slideCollision);
                            StateMachine.ChangeState(RollingState);
                        }
                    }
                }
            }
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPosition = Input.mousePosition;
            isTouching = true;
        }

        if (Input.GetMouseButtonUp(0) && isTouching && StateMachine.CurrentState == RunningState)
        {
            isTouching = false;
            Vector2 touchEndPosition = Input.mousePosition;
            Vector2 swipeDelta = touchEndPosition - touchStartPosition;

            if (swipeDelta.magnitude > swipeThreshold)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    // Горизонтальный свайп
                    if (swipeDelta.x > 0)
                    {
                        // Свайп вправо
                        if (_currentLineNum >= _lineNumArray.Length - 1) return;
                        _currentLineNum++;
                        this.transform.DOMoveZ(_lineNumArray[_currentLineNum].transform.position.z, 1.0f);
                    }
                    else
                    {
                        // Свайп влево
                        if (_currentLineNum <= 0) return;
                        _currentLineNum--;
                        this.transform.DOMoveZ(_lineNumArray[_currentLineNum].transform.position.z, 1.0f);
                    }
                }
                else
                {
                    // Вертикальный свайп
                    if (swipeDelta.y > 0)
                    {
                        // Свайп вверх
                        if (StateMachine.CurrentState != IdleState)
                        {
                            ChangeCollision(_jumpCollision);
                            StateMachine.ChangeState(JumpState);
                        }
                    }
                    else
                    {
                        // Свайп вниз
                        if (StateMachine.CurrentState != IdleState)
                        {
                            ChangeCollision(_slideCollision);
                            StateMachine.ChangeState(RollingState);
                        }
                    }
                }
            }
            else
            {
                // Обработка тапа
                if (StateMachine.CurrentState != IdleState)
                {
                    UIManager.instance.ClearScreen();
                    StateMachine.ChangeState(RunningState);
                }
            }
        }
    }

    public void StartRunning()
    {
        UIManager.instance.ClearScreen();
        StateMachine.ChangeState(RunningState);
    }
    private void FixedUpdate()
    {
        if (StateMachine.CurrentState != IdleState && StateMachine.CurrentState != DeathState)
        {
            MoveSpeed = _basicMovementSpeed + this.transform.position.x * _movespeedMultiplier;
            this.transform.position += new Vector3(1, 0, 0) * _basicMovementSpeed * Time.deltaTime;
        }
        if (ScoreField.text != "")
        {
            if (int.Parse(ScoreField.text) < this.transform.position.x)
            {
                ScoreField.text = (Math.Round(this.transform.position.x, 0)).ToString();
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Barrier"))
        {
            _firebaseManager.SaveDataButton();
            UIManager.instance.DeathScreen();
            StateMachine.ChangeState(DeathState);
        }

    }
    private void OnTriggerEnter(Collider other)
    {

    }

    public void RestartToMainMenu()
    {
        StateMachine.ChangeState(IdleState);
    }
    public void RestartToRunn()
    {
        StateMachine.ChangeState(IdleState);
        StateMachine.ChangeState(RunningState);
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

    public GameObject[] GetLinesArray()
    {
        return _lineNumArray;
    }

}
