using UnityEngine;

namespace MoonHop.Saving
{
    public class ElapsedTime : MonoBehaviour
    {
        float elapsedTime = 0;

        public void SetElapsedTime(float time)
        {
            elapsedTime = time;
        }

        public float GetElapsedTime()
        {
            return elapsedTime;
        }
    }
}
