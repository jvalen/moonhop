using UnityEngine;
using TMPro;
using MoonHop.Saving;

namespace MoonHop.Stats
{
    public class FinalScore : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI scoreText = null;
        [SerializeField] TextMeshProUGUI elapsedText = null;
        [SerializeField] TextMeshProUGUI hitsText = null;

        private void Start()
        {
            ElapsedTime elapsedTimePersistentObject = GameObject.FindObjectOfType<ElapsedTime>();
            Score scorePersistentObject = GameObject.FindObjectOfType<Score>();
            Hits hitsPersistentObject = GameObject.FindObjectOfType<Hits>();

            elapsedText.text = elapsedTimePersistentObject.GetElapsedTime().ToString("F0") + " s";
            scoreText.text = scorePersistentObject.GetScore().ToString("F0") + " points";
            hitsText.text = hitsPersistentObject.GetHits().ToString("F0");
        }
    }
}