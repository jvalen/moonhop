using System;
using System.Collections;
using System.Collections.Generic;
using MoonHop.FloatingObjects.Items;
using UnityEngine;
using UnityEngine.Events;

namespace MoonHop.Core
{
    public class PickupItem : MonoBehaviour
    {
        public delegate void PickupItemBoostDelegate(float boostValue);
        public event PickupItemBoostDelegate onPickupItemBoost;

        public delegate void PickupItemHealthDelegate(int boostValue);
        public event PickupItemHealthDelegate onPickupItemHealth;

        public void PickupAction(ItemTypes type, int itemEffectValue)
        {
            if (type == ItemTypes.Boost)
            {
                onPickupItemBoost(itemEffectValue);
            }
            else if (type == ItemTypes.Health)
            {
                onPickupItemHealth(itemEffectValue);
            }
        }
    }
}
