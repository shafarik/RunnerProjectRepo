using Runner.BasecUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Runner.MainMenuUI
{

    public class MainMenuUIScript : BasicUIScript
    {
        public override void HideCanvas()
        {
            base.HideCanvas();
        }

        public override void ShowCanvas()
        {
            base.ShowCanvas();
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