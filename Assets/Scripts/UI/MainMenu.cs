using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MoonHop.Saving;

namespace MoonHop.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] AudioSource introMusic;
        [SerializeField] AudioSource jumpSound;
        [SerializeField] Button plaButton;

        float startDelay = 1.5f;

        public void StartGame()
        {
            plaButton.interactable = false;
            StartCoroutine(LaunchInitialLevel());
        }

        public void GotoMainMenu()
        {
            LoadScene(1);
        }

        private IEnumerator LaunchInitialLevel()
        {
            PlayStartAnimation();
            yield return FadeOutMusic();
            yield return new WaitForSeconds(startDelay);
            LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private IEnumerator FadeOutMusic()
        {
            while (introMusic.volume > 0)
            {
                introMusic.volume -= 1 * Time.deltaTime / startDelay;
                yield return null;
            }
        }

        private void PlayStartAnimation()
        {
            jumpSound.Play();
            animator.SetBool("start", true);
        }

        private void LoadScene(int sceneId)
        {
            SceneManager.LoadScene(sceneId);
        }

        public void GotoCredits()
        {
            Application.OpenURL("https://github.com/jvalen");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}