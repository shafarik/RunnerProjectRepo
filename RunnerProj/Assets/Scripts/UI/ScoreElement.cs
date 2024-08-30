using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Runner.Score
{

    public class ScoreElement : MonoBehaviour
    {

        public TMP_Text usernameText;
        public TMP_Text HighestScore;

        public void NewScoreElement(string _username, int _score)
        {
            usernameText.text = _username;
            HighestScore.text = _score.ToString();
        }

    }
}