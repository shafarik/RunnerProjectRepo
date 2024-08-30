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
            // ���� ��� ����������� � ���������, ���������� ���������������
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }
#else
        // ���� ��� ����������� � ������, �������� ����������
        Application.Quit();
#endif
        }
    }
}