using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonHop.Saving
{
    public class Hits : MonoBehaviour
    {
        float currentHits = 0;

        public void SetHits(float hits)
        {
            currentHits = hits;
        }

        public float GetHits()
        {
            return currentHits;
        }
    }
}
