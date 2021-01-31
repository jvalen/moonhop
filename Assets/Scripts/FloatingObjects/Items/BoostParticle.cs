using System.Collections;
using UnityEngine;
using MoonHop.Core;
using MoonHop.Attributes;

namespace MoonHop.FloatingObjects.Items
{
    public class BoostParticle : MonoBehaviour
    {
        [SerializeField] ParticleSystem boostParticle;
        [SerializeField] AudioSource boostSound = null;

        Coroutine currentCoroutine = null;
        PickupItem pickupItem = null;
        Health health = null;

        public void EnableBoostParticleSystem()
        {
            boostParticle.Play();
        }

        public void DisableBoostParticleSystem()
        {
            boostParticle.Stop();
        }

        public void Boost(float boostTime)
        {
            if (currentCoroutine == null)
            {
                boostSound.Play();
                currentCoroutine = StartCoroutine(EnableDisableParticleSystem(boostTime));
            }
        }

        private void Awake()
        {
            pickupItem = GameObject.FindWithTag("Player").GetComponent<PickupItem>();
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            pickupItem.onPickupItemBoost += Boost;
            health.onTakenDamage += DisableBoostParticleSystem;
            health.onDead += DisableBoostParticleSystem;
        }

        private IEnumerator EnableDisableParticleSystem(float boostTime)
        {
            EnableBoostParticleSystem();
            yield return new WaitForSeconds(boostTime);
            DisableBoostParticleSystem();
            currentCoroutine = null;
        }
    }

}
