using MoonHop.Attributes;
using MoonHop.Core;
using MoonHop.Saving;
using UnityEngine;

namespace MoonHop.FloatingObjects
{
    public class FloatingObject : MonoBehaviour
    {
        protected FloatingObjectDefinition features = null;

        protected JourneyPhysics journeyPhysics = null;
        protected Health target = null;
        protected Score scorePersistentObject;

        protected float speedFactor = -2.4f;
        protected int effectValue = 10;
        ParticleSystem effectParticleSystem = null;

        Vector3 initialRotationVector;
        Vector3 initialPosition;

        private void Awake()
        {
            target = GameObject.FindWithTag("Player").GetComponent<Health>();
            journeyPhysics = GameObject.FindWithTag("GameController").GetComponent<JourneyPhysics>();
            scorePersistentObject = GameObject.FindObjectOfType<Score>();
        }

        public void Setup(FloatingObjectDefinition definition)
        {
            features = definition;
            initialRotationVector = getRotationRandomVector();
            initialPosition = transform.position;
            effectValue = features.effectValue;
            effectParticleSystem = features.effectParticleSystem;
            if (IsHoming())
            {
                transform.LookAt(GetAimLocation());
            }
        }

        private Vector3 getRotationRandomVector()
        {
            int xBinRandomValue = Random.Range(0, 2);
            int yBinRandomValue = Random.Range(0, 2);
            int zBinRandomValue = Random.Range(0, 2);

            if (features.rotateOnlyX || features.rotateOnlyY || features.rotateOnlyZ)
            {
                return new Vector3(
                    features.rotateOnlyX ? 1 : 0,
                    features.rotateOnlyY ? 1 : 0,
                    features.rotateOnlyZ ? 1 : 0
                );
            }

            if (xBinRandomValue + yBinRandomValue + zBinRandomValue > 1)
            {
                return new Vector3(
                    xBinRandomValue,
                    yBinRandomValue,
                    zBinRandomValue
                );
            }

            return new Vector3(1, Random.Range(0, 2), 1);
        }

        private bool IsHoming()
        {
            return features.homingSpeed > 0 && !target.IsDead();
        }

        private void Update()
        {
            if (features == null) return;

            if (features.isChild) return;

            UpdatePosition();
            if (features.isRotational)
            {
                UpdateRotation();
            }
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position;
        }

        protected void DestroyFloatingObject()
        {
            Destroy(gameObject);
        }

        protected virtual void UpdatePosition()
        {
            if (transform.position.z < features.zBoundaryPosition)
            {
                DestroyFloatingObject();
            }
            float forwardSpeed = speedFactor * journeyPhysics.GetSpeedFactor() * Time.deltaTime;
            float xPos = initialPosition.x + Mathf.Sin(Time.time * features.oscilatorSpeedX) * features.oscilatorFactorX;
            float yPos = initialPosition.y + Mathf.Sin(Time.time * features.oscilatorSpeedY) * features.oscilatorFactorY;
            float zPos = transform.position.z + forwardSpeed;

            if (IsHoming())
            {
                transform.Translate(Vector3.forward * -forwardSpeed * features.homingSpeed);
            }
            else
            {
                transform.position = new Vector3(xPos, yPos, zPos);
            }
        }

        protected virtual void UpdateRotation()
        {
            float rotationFactor = features.rotationSpeed * Time.deltaTime;

            transform.RotateAround(
                transform.position,
                initialRotationVector,
                rotationFactor
            );
        }

        protected virtual void ShowHitEffect()
        {
            if (effectParticleSystem != null)
            {
                Instantiate(effectParticleSystem, transform.position, transform.rotation);
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;

            target.TakeDamage(effectValue);
            ShowHitEffect();
            DestroyFloatingObject();
        }
    }
}
