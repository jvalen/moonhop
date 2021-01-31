using MoonHop.Core;
using MoonHop.Saving;
using TMPro;
using UnityEngine;

namespace MoonHop.Stats
{
    public class ScoreDisplay : MonoBehaviour
    {
        JourneyPhysics journeyPhysics = null;
        Score scorePersistentObject;
        float score = 0f;

        private void Awake()
        {
            journeyPhysics = GameObject.FindWithTag("GameController").GetComponent<JourneyPhysics>();
            journeyPhysics.onTimeCounterChange += UpdateScore;
        }

        private void Start()
        {
            scorePersistentObject = GameObject.FindObjectOfType<Score>();
        }

        private void UpdateScore(int value)
        {
            score = value;
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = scorePersistentObject.GetScore().ToString("F0");
        }
    }

}

