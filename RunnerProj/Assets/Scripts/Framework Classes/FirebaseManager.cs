using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Runner.UImanager;
using Runner.Score;
using System;

namespace Runner.Firebase
{


    public class FirebaseManager : MonoBehaviour
    {
        //Firebase variables
        [Header("Firebase")]
        public DependencyStatus dependencyStatus;
        public FirebaseAuth auth;
        public FirebaseUser User;
        public DatabaseReference DBreference;

        //Login variables
        //[Header("Login")]
        //public TMP_InputField emailLoginField;
        //public TMP_InputField passwordLoginField;
        //public TMP_Text warningLoginText;
        //public TMP_Text confirmLoginText;

        //Register variables
        //[Header("Register")]
        //public TMP_InputField usernameRegisterField;
        //public TMP_InputField emailRegisterField;
        //public TMP_InputField passwordRegisterField;
        //public TMP_InputField passwordRegisterVerifyField;
        //public TMP_Text warningRegisterText;

        //User Data variables
        //[Header("UserData")]
        //public TMP_InputField usernameField;
        //public TMP_InputField ScoreField;
        //public GameObject scoreElement;
        //public Transform scoreboardContent;

        public bool IsLogined = false;

        void Awake()
        {
            //Check that all of the necessary dependencies for Firebase are present on the system
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    //If they are avalible Initialize Firebase
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }


        public void SilentLoginCheck()
        {
            if (auth != null)
            {
                Debug.Log("auth not null");
                if (auth.CurrentUser != null)
                {
                    Debug.Log("CurrentUser not null");

                    if (auth.CurrentUser != User)
                    {
                        Debug.Log("auth.CurrentUser != User");

                        bool signedIn = User != auth.CurrentUser && auth.CurrentUser != null;
                        if (signedIn)
                        {
                            IsLogined = true;

                            Debug.Log("IsLogined = "+ IsLogined);
                            User = auth.CurrentUser;

                            //Debug.Log("signedIn = true");

                            //Debug.Log("Signed in " + User.UserId);


                        }
                    }
                    else
                    {
                        Debug.Log("Not Signed In ");
                    }
                }
            }
        }
        private void InitializeFirebase()
        {
            Debug.Log("Setting up Firebase Auth");
            //Set the authentication instance object
            auth = FirebaseAuth.DefaultInstance;
            DBreference = FirebaseDatabase.DefaultInstance.RootReference;

             SilentLoginCheck();
        }

        public void SilentLogin(Action<bool, string, string, string> onComplete)
        {
            LoadUserDataFunction((success, BestScore) =>
            {
                onComplete?.Invoke(success, "Logged In", User.DisplayName, BestScore);

            });
        }

        // Track state changes of the auth object.
        void AuthStateChanged(object sender, System.EventArgs eventArgs)
        {
            if (auth != null)
                Debug.Log("auth is Null");
            else
                if (auth.CurrentUser != null)
                Debug.Log("auth.CurrentUser is Null");
            else
                if (auth.CurrentUser.DisplayName != null)
                Debug.Log("CurrentUser.DisplayName is Null");

            if (User == null || User.DisplayName == null)
                Debug.Log("Name is Null");

        }

        //Function for the login button
        public void LoginButton(string emailLoginText, string passwordLoginText, Action<bool, string, string, string> OnComplete = null)
        {
            //Call the login coroutine passing the email and password
            StartCoroutine(Login(emailLoginText, passwordLoginText, OnComplete));
        }
        public void RegisterButton()
        {
            //Call the register coroutine passing the email, password, and username
            //   StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
        }

        public void RegisterUser(
            string emailRegisterText,
            string passwordRegisterText,
            string passwordVerificationRegisterText,
            string usernameRegisterText,
            Action<bool, string, string, string> onComplete = null)
        {
            StartCoroutine(Register(emailRegisterText, passwordRegisterText, passwordVerificationRegisterText, usernameRegisterText, onComplete));
        }
        //Function for the sign out button
        public void LogOutButton()
        {
            auth.SignOut();
        }
        //Function for the save button
        public void SaveDataButton(string usernameText, string ScoreText)
        {
            StartCoroutine(UpdateUsernameAuth(usernameText));
            StartCoroutine(UpdateUsernameDatabase(usernameText));

            StartCoroutine(UpdateScore(int.Parse(ScoreText)));
        }
        //Function for the scoreboard button
        public void ScoreboardButton(Action<DataSnapshot> OnCompete = null)
        {
            StartCoroutine(LoadScoreboardData(OnCompete));
        }
        public void LoadUserDataFunction(Action<bool, string> onFinish = null)
        {
            StartCoroutine(LoadUserData(onFinish));
        }
        public void CreateUsernameDatabaseFunction(string username, Action<bool, string> onFinish = null)
        {
            StartCoroutine(CreateUsernameDatabase(User.DisplayName, onFinish));
        }
        private IEnumerator Login(string _email, string _password, Action<bool, string, string, string> onComplete)
        {
            //Call the Firebase auth signin function passing the email and password
            Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

            if (LoginTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
                FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Login Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WrongPassword:
                        message = "Wrong Password";
                        break;
                    case AuthError.InvalidEmail:
                        message = "Invalid Email";
                        break;
                    case AuthError.UserNotFound:
                        message = "Account does not exist";
                        break;
                }
                onComplete?.Invoke(false, message, null, null);
                // warningLoginText.text = message;
            }
            else
            {
                //User is now logged in
                //Now get the result
                User = LoginTask.Result.User;
                Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);


                //usernameField.text = User.DisplayName;
                //warningLoginText.text = "";
                //confirmLoginText.text = "Logged In";

                LoadUserDataFunction((success, BestScore) =>
                {
                    onComplete?.Invoke(success, "Logged In", User.DisplayName, BestScore);

                });
            }
        }

        private IEnumerator Register(string _email, string _password, string _passwodVerification, string _username, Action<bool, string, string, string> onComplete)
        {
            if (_username == "")
            {
                //If the username field is blank show a warning
                onComplete?.Invoke(false, "Missing Username", null, null);
                // warningRegisterText.text = "Missing Username";
            }
            else if (_password != _passwodVerification)
            {
                //If the password does not match show a warning
                onComplete?.Invoke(false, "Password Does Not Match!", null, null);

                // warningRegisterText.text = "Password Does Not Match!";
            }
            else
            {
                //Call the Firebase auth signin function passing the email and password
                Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
                //Wait until the task completes
                yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

                if (RegisterTask.Exception != null)
                {
                    //If there are errors handle them
                    Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                    FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                    string message = "Register Failed!";
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;
                        case AuthError.WeakPassword:
                            message = "Weak Password";
                            break;
                        case AuthError.EmailAlreadyInUse:
                            message = "Email Already In Use";
                            break;
                    }
                    //warningRegisterText.text = message;
                    onComplete?.Invoke(false, message, null, null);

                }
                else
                {
                    //User has now been created
                    //Now get the result
                    User = RegisterTask.Result.User;

                    if (User != null)
                    {
                        //Create a user profile and set the username
                        UserProfile profile = new UserProfile { DisplayName = _username };

                        //Call the Firebase auth update user profile function passing the profile with the username
                        Task ProfileTask = User.UpdateUserProfileAsync(profile);
                        //Wait until the task completes
                        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                        if (ProfileTask.Exception != null)
                        {
                            //If there are errors handle them
                            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                            // warningRegisterText.text = "Username Set Failed!";
                            onComplete?.Invoke(false, "Username Set Failed", null, null);

                        }
                        else
                        {
                            CreateUsernameDatabaseFunction(User.DisplayName, (success, UserName) =>
                            {
                                onComplete?.Invoke(success, "Registration is success", UserName, "0");
                            });
                        }
                    }
                }
            }
        }

        private IEnumerator UpdateUsernameAuth(string _username)
        {
            //Create a user profile and set the username
            UserProfile profile = new UserProfile { DisplayName = _username };

            //Call the Firebase auth update user profile function passing the profile with the username
            Task ProfileTask = User.UpdateUserProfileAsync(profile);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

            if (ProfileTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
            }
            else
            {
                //Auth username is now updated
            }
        }

        private IEnumerator UpdateUsernameDatabase(string _username)
        {
            //Set the currently logged in user username in the database
            Task DBTask = DBreference.Child("users").Child(User.UserId).Child("userName").SetValueAsync(_username);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }
        }

        private IEnumerator CreateUsernameDatabase(string _username, Action<bool, string> onComplete)
        {
            //Set the currently logged in user username in the database
            Task DBTask = DBreference.Child("users").Child(User.UserId).Child("userName").SetValueAsync(_username);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                onComplete?.Invoke(true, User.DisplayName);

                //usernameField.text = User.DisplayName;
                //ScoreField.text = "0";
                //Database username is now updated
                StartCoroutine(UpdateScore(0));
            }
        }

        private IEnumerator UpdateScore(int score)
        {
            //Set the currently logged in user kills
            Task DBTask = DBreference.Child("users").Child(User.UserId).Child("highScore").SetValueAsync(score);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Kills are now updated
            }
        }

        private IEnumerator LoadUserData(Action<bool, string> onComplete)
        {
            //Get the currently logged in user data
            Task<DataSnapshot> DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else if (DBTask.Result.Value == null)
            {
                //No data exists yet
                onComplete?.Invoke(false, "No data exists yet");

                //ScoreField.text = "0";
            }
            else
            {
                //Data has been retrieved
                DataSnapshot snapshot = DBTask.Result;

                onComplete?.Invoke(true, snapshot.Child("highScore").Value.ToString());


                //ScoreField.text = snapshot.Child("highScore").Value.ToString();
            }
        }

        private IEnumerator LoadScoreboardData(Action<DataSnapshot> onComplete)
        {
            //Get all the users data ordered by kills amount
            Task<DataSnapshot> DBTask = DBreference.Child("users").OrderByChild("highScore").GetValueAsync();

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Data has been retrieved
                DataSnapshot snapshot = DBTask.Result;

                onComplete?.Invoke(snapshot);
            }
        }
    }
}