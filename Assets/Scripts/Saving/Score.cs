using System.Collections;
using System.Collections.Generic;
using MoonHop.Core;
using UnityEngine;

namespace MoonHop.Saving
{
    public class Score : MonoBehaviour
    {
        float currentScore = 0;

        public void SetScore(float score)
        {
            currentScore = score;
        }

        public float GetScore()
        {
            return currentScore;
        }
    }
}