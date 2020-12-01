using MoonHop.Core;
using MoonHop.Saving;
using UnityEngine;

namespace MoonHop.FloatingObjects.Items
{
    public class FloatingItem : FloatingObject
    {
        [SerializeField] public ItemTypes type;
        float scoreDefaultValue = 20;

        protected override void UpdatePosition()
        {
            if (transform.position.z < features.zBoundaryPosition)
            {
                DestroyFloatingObject();
            }
            float speed = speedFactor * journeyPhysics.GetSpeedFactor() * Time.deltaTime;
            float zPosition = transform.position.z + speed;
            transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
        }

        protected override void ShowHitEffect()
        {

        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            DestroyFloatingObject();
            scorePersistentObject.SetScore(scorePersistentObject.GetScore() + scoreDefaultValue);
            other.GetComponent<PickupItem>().PickupAction(type, features.effectValue);
        }
    }
}
