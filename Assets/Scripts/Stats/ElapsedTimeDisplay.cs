using MoonHop.Core;
using TMPro;
using UnityEngine;

namespace MoonHop.Stats
{
    public class ElapsedTimeDisplay : MonoBehaviour
    {
        JourneyPhysics journeyPhysics = null;
        float elapsedTime = 0f;

        private void Awake()
        {
            journeyPhysics = GameObject.FindWithTag("GameController").GetComponent<JourneyPhysics>();
            journeyPhysics.onTimeCounterChange += UpdateElapsedTime;
        }

        private void UpdateElapsedTime(int value)
        {
            elapsedTime = value;
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = elapsedTime.ToString("F0");
        }
    }
}

