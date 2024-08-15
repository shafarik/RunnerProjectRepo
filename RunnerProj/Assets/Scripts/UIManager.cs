using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] Player _player;
    //Screen object variables
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject userDataUI;
    public GameObject scoreboardUI;
    public GameObject deathUI;
    public GameObject adLoadUI;

    [Space]
    public GameObject deathCanvas;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    //Functions to change the login screen UI

    public void ClearScreen() //Turn off all screens
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        userDataUI.SetActive(false);
        scoreboardUI.SetActive(false);
        deathUI.SetActive(false);
        adLoadUI.SetActive(false);
    }

    public void ShowAdScreen()
    {
        deathCanvas.SetActive(false);
    }

    public void CloseAdScreen()
    {
        ClearScreen();
        _player.StateMachine.ChangeState(_player.RunningState);
    }


    public void LoginScreen() //Back button
    {
        ClearScreen();
        loginUI.SetActive(true);
    }
    public void RegisterScreen() // Regester button
    {
        ClearScreen();
        registerUI.SetActive(true);
    }

    public void UserDataScreen() //Logged in
    {
        ClearScreen();
        userDataUI.SetActive(true);
    }

    public void ScoreboardScreen() //Scoreboard button
    {
        ClearScreen();
        scoreboardUI.SetActive(true);
    }

    public void DeathScreen() //Death screen
    {
        ClearScreen();
        deathUI.SetActive(true);
    }
    public void AdLoadScreen() //adLoad screen
    {
        ClearScreen();
        adLoadUI.SetActive(true);
    }


    public void QuitApplication()
    {
#if UNITY_EDITOR
        // ≈сли код выполн€етс€ в редакторе, остановите воспроизведение
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
#else
        // ≈сли код выполн€етс€ в сборке, закройте приложение
        Application.Quit();
#endif
    }
}