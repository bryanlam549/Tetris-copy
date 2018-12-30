using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public void updateScoreNormal()
    {
        scoreText.text = (int.Parse(scoreText.text) + 10).ToString();
    }

    public void updateScore(int multiplier)
    {
        if(multiplier == 0)
        {
            scoreText.text = (int.Parse(scoreText.text) + 10).ToString();
        }
        else if(multiplier == 1)
        {
            scoreText.text = (int.Parse(scoreText.text) + 100).ToString();
        }
        else if (multiplier == 2)
        {
            scoreText.text = (int.Parse(scoreText.text) + 200).ToString();
        }
        else if (multiplier == 3)
        {
            scoreText.text = (int.Parse(scoreText.text) + 400).ToString();
        }
        else if (multiplier == 4)
        {
            scoreText.text = (int.Parse(scoreText.text) + 800).ToString();
        }


    }
}   
