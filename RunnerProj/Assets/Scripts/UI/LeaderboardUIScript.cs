using Firebase.Database;
using Runner.BasecUI;
using Runner.Firebase;
using Runner.Score;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Runner.LeaderboardUI
{

    public class LeaderboardUIScript : BasicUIScript
    {

        public GameObject scoreElement;
        public Transform scoreboardContent;

        [SerializeField] private FirebaseManager _firebaseManager;

        public override void HideCanvas()
        {
            base.HideCanvas();
        }

        public override void ShowCanvas()
        {
            base.ShowCanvas();

            Leaderboard();
        }

        public void Leaderboard()
        {
            _firebaseManager.ScoreboardButton((snapshot) =>
            {
                //Destroy any existing scoreboard elements
                foreach (Transform child in scoreboardContent.transform)
                {
                    Destroy(child.gameObject);
                }
                Instantiate(scoreElement, scoreboardContent);
                //Loop through every users UID
                foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
                {
                    string username = childSnapshot.Child("userName").Value.ToString();
                    int score = int.Parse(childSnapshot.Child("highScore").Value.ToString());

                    //Instantiate new scoreboard elements
                    GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                    scoreboardElement.GetComponentInChildren<ScoreElement>().NewScoreElement(username, score);
                }

            });


        }
    }

}