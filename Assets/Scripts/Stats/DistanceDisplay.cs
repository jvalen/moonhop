using MoonHop.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonHop.Stats
{
    public class DistanceDisplay : MonoBehaviour
    {
        [SerializeField] Image journeyImage = null;
        [SerializeField] TextMeshProUGUI distanceToMoonText = null;

        JourneyPhysics journeyPhysics = null;
        float totalLayers = 0f;

        private void Awake()
        {
            journeyPhysics = GameObject.FindWithTag("GameController").GetComponent<JourneyPhysics>();
            journeyPhysics.onAtmosphereLayerChange += AdvanceToNextLayer;
            totalLayers = journeyPhysics.GetAtmosphereLayerCount();
        }

        private void Update()
        {
            distanceToMoonText.text = journeyPhysics.GetMoonDistance().ToString("F0");
        }

        private void AdvanceToNextLayer(int layerId)
        {
            journeyImage.fillAmount = (float)layerId / totalLayers;
        }
    }
}