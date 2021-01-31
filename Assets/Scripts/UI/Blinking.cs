using System.Collections;
using MoonHop.Core;
using TMPro;
using UnityEngine;

namespace MoonHop.UI
{
    public class Blinking : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI blinkingText = null;

        JourneyPhysics journeyPhysics = null;
        int blinkingTime = 4;

        private void Awake()
        {
            journeyPhysics = GameObject.FindWithTag("GameController").GetComponent<JourneyPhysics>();
            journeyPhysics.onAtmosphereLayerChange += StartBlinking;
        }

        private void StartBlinking(int layerId)
        {
            if (layerId == 3 || layerId == 4)
            {
                blinkingText.gameObject.SetActive(true);
                StartCoroutine(StopBlinking());
            }
        }

        private IEnumerator StopBlinking()
        {
            yield return new WaitForSeconds(blinkingTime);
            blinkingText.gameObject.SetActive(false);
        }

    }
}
