using Runner.BasecUI;
using Runner.Firebase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Runner.RegisterUI
{

    public class RegisterUIScript : BasicUIScript
    {
        public TMP_InputField usernameRegisterField;
        public TMP_InputField emailRegisterField;
        public TMP_InputField passwordRegisterField;
        public TMP_InputField passwordRegisterVerifyField;
        public TMP_Text warningRegisterText;

        [SerializeField] private TMP_InputField usernameField;
        [SerializeField] private TMP_InputField scoreField;

        public Canvas RegistrationSuccessCanvas;


        [SerializeField] private FirebaseManager _firebaseManager;
        public override void HideCanvas()
        {
            ClearAllData();
            RegistrationSuccessCanvas.enabled = false;
            base.HideCanvas();
        }

        private void ClearAllData()
        {
            usernameRegisterField.text = string.Empty;
            emailRegisterField.text = string.Empty;
            passwordRegisterField.text = string.Empty;
            passwordRegisterVerifyField.text = string.Empty;
            warningRegisterText.text = string.Empty;
        }

        public void RegisterButton()
        {
            _firebaseManager.RegisterUser(
                emailRegisterField.text,
                passwordRegisterField.text,
                passwordRegisterVerifyField.text,
                usernameRegisterField.text,
                (success, message, Username, BestScore) =>
            {
                if (success)
                {
                    Debug.Log("Registration successful!");

                    usernameField.text = Username;
                    scoreField.text = BestScore;

                    HideCanvas();
                }
                else
                {
                    Debug.Log("Registration failed!");
                }
            });
        }

        public void ShowRegistrationSuccessCanvas()
        {
            RegistrationSuccessCanvas.enabled = true;
        }

        public override void ShowCanvas()
        {
            base.ShowCanvas();
        }
    }

}