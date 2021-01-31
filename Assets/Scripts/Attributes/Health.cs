using System;
using MoonHop.Core;
using MoonHop.Saving;
using UnityEngine;

namespace MoonHop.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealthPoints = 100;
        [SerializeField] AudioSource hitSound = null;
        [SerializeField] AudioSource healthPowerupSound = null;
        [SerializeField] AudioSource gameoverSound = null;
        [SerializeField] AudioSource music = null;
        [SerializeField] ParticleSystem rocketLeftParticle = null;
        [SerializeField] ParticleSystem rocketRightParticle = null;

        public event Action onTakenDamage;
        public event Action onDead;

        int healthPoints = 100;
        int hits = 0;
        bool isDead = false;
        PickupItem pickupItem = null;
        Hits hitsPersistentObject = null;

        public void TakeDamage(int damage)
        {
            if (isDead) return;

            healthPoints = Math.Max(healthPoints - damage, 0);

            if (healthPoints <= 0)
            {
                DeadSequence();
            }
            else
            {
                hitSound.Play();
                hits++;
                UpdateHitsTaken(hits);
                onTakenDamage();
            }
        }

        public int GetHealthPoints()
        {
            return healthPoints;
        }

        public int GetMaxHealthPoints()
        {
            return maxHealthPoints;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void HealthPowerUp(int healthIncrease)
        {
            healthPowerupSound.Play();
            healthPoints = Math.Min(healthPoints + healthIncrease, GetMaxHealthPoints());
        }

        private void Awake()
        {
            pickupItem = GetComponent<PickupItem>();
            pickupItem.onPickupItemHealth += HealthPowerUp;
            hitsPersistentObject = GameObject.FindObjectOfType<Hits>();
            UpdateHitsTaken(hits);
        }

        private void UpdateHitsTaken(int hitsTaken)
        {
            hitsPersistentObject.SetHits(hitsTaken);
        }

        private void DeadSequence()
        {
            isDead = true;
            music.Stop();
            rocketLeftParticle.Stop();
            rocketRightParticle.Stop();
            gameoverSound.Play();
            onDead();
        }
    }
}

