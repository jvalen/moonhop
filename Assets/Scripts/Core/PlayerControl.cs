using System.Collections;
using MoonHop.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonHop.Core
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] GameObject character = null;

        [Tooltip("In kms^-1")] [SerializeField] float controlSpeed = 10f;
        [Tooltip("In m")] [SerializeField] float xRange = 5f;
        [Tooltip("In m")] [SerializeField] float yRange = 4f;

        [Header("Screen-position Based")]
        [SerializeField] float positionPitchFactor = -2f;
        [SerializeField] float positionYawFactor = 2f;

        [Header("Control-throw Based")]
        [SerializeField] float controlPitchFactor = -5f;
        [SerializeField] float controlRollFactor = -5f;

        [Header("Character shift roll")]
        [SerializeField] float characterRollFactor = 2f;

        [SerializeField] Animator playerAnimator;
        [SerializeField] Animator coreAnimator;

        Health health = null;
        float xThrow, yThrow;
        float deadAnimationTime = 6f;
        int mainMenuSceneId = 1;

        public void Falling()
        {
            StartCoroutine(LostControl());
        }

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            health.onDead += Falling;
        }

        private void Update()
        {
            ProcessTranslation();
            ProcessRotation();
        }

        private void ProcessRotation()
        {
            float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
            float pitchDueToControlThrow = yThrow * controlPitchFactor;
            float pitch = pitchDueToPosition + pitchDueToControlThrow;
            float yaw = transform.localPosition.x * positionYawFactor;
            float roll = xThrow * controlRollFactor;

            transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
            character.transform.localRotation = Quaternion.Euler(pitch, yaw + (roll * characterRollFactor), roll);
        }

        private void ProcessTranslation()
        {
            xThrow = Input.GetAxis("Horizontal");
            float xOffset = xThrow * controlSpeed * Time.deltaTime;
            float rawXPos = transform.localPosition.x + xOffset;
            float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

            yThrow = Input.GetAxis("Vertical");
            float yOffset = yThrow * controlSpeed * Time.deltaTime;
            float rawYPos = transform.localPosition.y + yOffset;
            float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

            transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
        }

        private IEnumerator LostControl()
        {
            playerAnimator.SetBool("isDead", true);
            coreAnimator.SetBool("isDead", true);
            yield return new WaitForSeconds(deadAnimationTime);
            SceneManager.LoadScene(mainMenuSceneId);
        }
    }
}
