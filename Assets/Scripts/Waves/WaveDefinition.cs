using System.Collections.Generic;
using MoonHop.FloatingObjects;
using MoonHop.Enemies;
using UnityEngine;

namespace MoonHop.Waves
{
    [CreateAssetMenu(fileName = "WaveDefinition", menuName = "MoonHop/WaveDefinition", order = 0)]
    public class WaveDefinition : ScriptableObject
    {
        [System.Serializable]
        public class ObjectPosition
        {
            [Range(-4f, 4f)]
            [SerializeField] public float x = 0;
            [Range(-3f, 3f)]
            [SerializeField] public float y = 0;
            [SerializeField] public float randomOffsetFactor = 0.03f;
        }

        [System.Serializable]
        public class WaveAction
        {
            public string name;
            public float actionDelay;
            public List<FloatingObjectDefinition> objectDefinitionList;
            public int spawnCount = 1;
            public float spawnDelay = 1;
            public string message;
            public bool enabled = true;
            public float burst = 0f;

            public List<ObjectPosition> objectPositionList;
        }

        [System.Serializable]
        public class Wave
        {
            public float startWaveDelay = 0;
            public float shareActionDelay = 0;
            public string name;
            public List<WaveAction> actions;
            public EnemyAI finalBoss = null;
        }

        [SerializeField] Wave wave;

        public Wave GetWave()
        {
            return wave;
        }
    }
}