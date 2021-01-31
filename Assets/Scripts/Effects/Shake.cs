using System.Collections;
using MoonHop.Attributes;
using UnityEngine;

namespace MoonHop.Effects
{
    public class Shake : MonoBehaviour
    {

        [SerializeField] float shakeDuration = 0f;
        [SerializeField] float shakeAmount = 0.7f;
        [SerializeField] float decreaseFactor = 1.0f;
        [SerializeField] bool autoShake = false;

        Transform objectTransform;
        Health health = null;
        Coroutine shakeCoroutine = null;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            health.onTakenDamage += ShakeObject;

            objectTransform = GetComponent<Transform>();
        }

        private void Start()
        {
            if (autoShake)
            {
                shakeDuration = Mathf.Infinity;
                shakeCoroutine = StartCoroutine(ShakeByDamageTaken());
            }
        }

        public void ShakeObject()
        {
            if (shakeCoroutine == null)
            {
                shakeCoroutine = StartCoroutine(ShakeByDamageTaken());
            }
        }

        private IEnumerator ShakeByDamageTaken()
        {
            float elapsedTime = 0f;
            float shakeValue = shakeDuration;
            Vector3 originalPos = objectTransform.localPosition;

            while (elapsedTime < shakeValue)
            {
                objectTransform.localPosition = originalPos + UnityEngine.Random.insideUnitSphere * shakeAmount;
                shakeValue -= Time.deltaTime * decreaseFactor;

                elapsedTime += Time.deltaTime;

                yield return null;
            }
            objectTransform.localPosition = originalPos;
            shakeCoroutine = null;
        }
    }
}
