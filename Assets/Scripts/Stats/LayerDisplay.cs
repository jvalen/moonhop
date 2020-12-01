using MoonHop.Core;
using TMPro;
using UnityEngine;

namespace MoonHop.Stats
{
    public class LayerDisplay : MonoBehaviour
    {
        JourneyPhysics journeyPhysics = null;

        private void Awake()
        {
            journeyPhysics = GameObject.FindWithTag("GameController").GetComponent<JourneyPhysics>();
            journeyPhysics.onAtmosphereLayerChange += ChangeLayerName;
        }

        private void ChangeLayerName(int layerId)
        {
            GetComponent<TextMeshProUGUI>().text = journeyPhysics.GetAtmosphereLayerName();
        }
    }
}