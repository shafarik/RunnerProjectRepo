using Runner.BasecUI;
using Runner.PlayerCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Runner.MainMenuUI
{

    public class MainMenuUIScript : BasicUIScript
    {

        public static bool _needPlayerToRun = false;

        private void Start()
        {
            Debug.Log("Start");
            if (_needPlayerToRun)
            {
                Debug.Log("Player to run true");
                _needPlayerToRun = false;

                HideCanvas();
            }
        }

        public override void HideCanvas()
        {
            base.HideCanvas();
        }

        public override void ShowCanvas()
        {
            base.ShowCanvas();
        }

        public void SetPlayerToRun()
        {
            _needPlayerToRun = true;
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
}