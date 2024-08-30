using Runner.BasecUI;
using Runner.Firebase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LoginUI
{

    public class LoginUIScript : BasicUIScript
    {
        [SerializeField] private TMP_InputField emailLoginField;
        [SerializeField] private TMP_InputField passwordLoginField;
        [SerializeField] private TMP_Text warningLoginText;
        [SerializeField] private TMP_Text confirmLoginText;

        [SerializeField] private TMP_InputField usernameField;
        [SerializeField] private TMP_InputField scoreField;

        [SerializeField] private FirebaseManager _firebaseManager;


        public override void HideCanvas()
        {
            ClearAllData();

            base.HideCanvas();
        }

        private void ClearAllData()
        {
            emailLoginField.text = string.Empty;
            passwordLoginField.text = string.Empty;
            warningLoginText.text = string.Empty;
            confirmLoginText.text = string.Empty;
        }

        public override void ShowCanvas()
        {
            ClearAllData();

            base.ShowCanvas();
        }

        public void Login()
        {
            warningLoginText.text = string.Empty;
            confirmLoginText.text = string.Empty;

            _firebaseManager.LoginButton(emailLoginField.text, passwordLoginField.text, (success, message, Username, BestScore) =>
            {
                if (!success)
                {
                    warningLoginText.text = message;
                }
                else
                {
                    confirmLoginText.text = message;
                    usernameField.text = Username;
                    scoreField.text = BestScore;

                    HideCanvas();
                }
            });
        }

        private void Update()
        {
            if (_firebaseManager.IsLogined)
            {
                _firebaseManager.IsLogined = false;

                Debug.Log("Try to silent login");


                _firebaseManager.SilentLogin((success, message, Username, BestScore) =>
                {
                    if (success)
                    {
                        confirmLoginText.text = message;
                        usernameField.text = Username;
                        scoreField.text = BestScore;

                        HideCanvas();
                    }
                });
            }
        }

    }

}