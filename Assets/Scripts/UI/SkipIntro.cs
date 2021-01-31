using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonHop.UI
{
    public class SkipIntro : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI blinkingText = null;

        float showTextDelay = 3f;
        bool isShown = false;

        private void Start()
        {
            StartCoroutine(ShowText());
        }

        private void Update()
        {
            if (isShown && Input.anyKeyDown)
            {
                int buildIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(buildIndex + 1);
            }
        }

        private IEnumerator ShowText()
        {
            yield return new WaitForSeconds(showTextDelay);
            isShown = true;
            blinkingText.gameObject.SetActive(true);
        }
    }
}