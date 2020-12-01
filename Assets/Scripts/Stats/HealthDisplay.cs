using System;
using MoonHop.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonHop.Stats
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] Image healthImage = null;
        Health health;
        int maxHealth = 0;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            maxHealth = health.GetMaxHealthPoints();
        }

        private void Update()
        {
            healthImage.fillAmount = (float)health.GetHealthPoints() / maxHealth;
        }
    }

}
