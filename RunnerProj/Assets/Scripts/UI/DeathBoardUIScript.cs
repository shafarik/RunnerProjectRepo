using Runner.BasecUI;
using Runner.Firebase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Runner.DeathBoard
{

    public class DeathBoardUIScript : BasicUIScript
    {
        public TMP_InputField usernameField;
        public TMP_InputField ScoreField;

        [SerializeField] private FirebaseManager _firebaseManager;

        [SerializeField] private Canvas _deathCanvas;

        public override void HideCanvas()
        {
            _deathCanvas.enabled = false;

            base.HideCanvas();
        }

        public override void ShowCanvas()
        {
            base.ShowCanvas();

            _deathCanvas.enabled = true;

            _firebaseManager.SaveDataButton(usernameField.text, ScoreField.text);
        }
    }

}