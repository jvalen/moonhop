using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonHop.FloatingObjects
{
    [CreateAssetMenu(fileName = "FloatingObject", menuName = "MoonHop/FloatingObject", order = 0)]
    public class FloatingObjectDefinition : ScriptableObject
    {
        [SerializeField] string displayName;
        [SerializeField] string description;
        [SerializeField] FloatingObject floatingObjectPrefab = null;
        [SerializeField] public ParticleSystem effectParticleSystem = null;
        [SerializeField] public int effectValue = 10;

        [SerializeField] public bool isRotational = true;
        [SerializeField] public bool rotateOnlyX = false;
        [SerializeField] public bool rotateOnlyY = false;
        [SerializeField] public bool rotateOnlyZ = false;
        [SerializeField] public bool isChild = false;

        [Range(-0.01f, -5f)]
        [SerializeField] public float zBoundaryPosition = 0;

        [Range(1f, 100f)]
        [SerializeField] public float rotationSpeed = 50f;
        [SerializeField] public float rotationRandomFactor = 30f;

        [Range(0f, 10f)]
        [SerializeField] public float oscilatorFactorX = 0f;
        [Range(0f, 10f)]
        [SerializeField] public float oscilatorSpeedX = 0f;

        [Range(0f, 10f)]
        [SerializeField] public float oscilatorFactorY = 0f;
        [Range(0f, 10f)]
        [SerializeField] public float oscilatorSpeedY = 0f;

        [SerializeField] public float homingSpeed = 0f;

        public FloatingObject SpawnFloatingObject(Vector3 position)
        {
            var floatingObjectInstance = Instantiate(floatingObjectPrefab);
            floatingObjectInstance.transform.position = position;
            floatingObjectInstance.Setup(this);
            return floatingObjectInstance;
        }
    }
}
