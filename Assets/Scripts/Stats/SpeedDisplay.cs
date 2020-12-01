using System;
using MoonHop.Core;
using TMPro;
using UnityEngine;

namespace MoonHop.Stats
{
    public class SpeedDisplay : MonoBehaviour
    {
        JourneyPhysics journey = null;

        private void Awake()
        {
            journey = GameObject.FindWithTag("GameController").GetComponent<JourneyPhysics>();
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = journey.GetSpeedFormatted().ToString("F0");
        }
    }
}

