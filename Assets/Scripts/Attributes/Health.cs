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
        int healthPoints = 100;
        int hits = 0;
        PickupItem pickupItem = null;
        Hits hitsPersistentObject = null;
        bool isDead = false;

        public event Action onTakenDamage;
        public event Action onDead;

        private void Awake()
        {
            pickupItem = GetComponent<PickupItem>();
            pickupItem.onPickupItemHealth += HealthPowerUp;
            hitsPersistentObject = GameObject.FindObjectOfType<Hits>();
            UpdateHitsTaken(hits);
        }

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

        private void UpdateHitsTaken(int hitsTaken)
        {
            hitsPersistentObject.SetHits(hitsTaken);
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

        private void DeadSequence()
        {
            isDead = true;
            music.Stop();
            rocketLeftParticle.Stop();
            rocketRightParticle.Stop();
            gameoverSound.Play();
            onDead();
        }

        public void HealthPowerUp(int healthIncrease)
        {
            healthPowerupSound.Play();
            healthPoints = Math.Min(healthPoints + healthIncrease, GetMaxHealthPoints());
        }
    }
}

