using System;
using System.Collections;
using System.Collections.Generic;
using MoonHop.Attributes;
using MoonHop.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonHop.Core
{
    public class JourneyPhysics : MonoBehaviour
    {
        [System.Serializable]
        private class AtmosphereLayer
        {
            public string name;
            public float endDistance;
            public float moonApproachFactorLayer;
            public float moonSpeedFactor;
            public float speedFactorLayer;
        }

        [SerializeField] List<AtmosphereLayer> atmosphereLayers;
        AtmosphereLayer currentLayer = new AtmosphereLayer();

        int currentLayerIndex = 0;

        [SerializeField] GameObject moon = null;
        [SerializeField] float moonApproachFactor = 0.001f;
        [SerializeField] float distanceTravelled = 0;
        [SerializeField] float distanceToMoon = 384000f;
        float lastMoonDistance = 0f;
        float speedFormatted = 0f;
        float elapsedTimeSpeedCounter = 0f;
        float totalElapsedTime = 0f;

        [SerializeField] float speedFactor = 1.5f;
        [SerializeField] float minSpeedFactor = 1.5f;
        [SerializeField] float maxSpeedFactor = 5f;

        Vector3 closestMoonDistance = new Vector3(0, 0, 40);

        Health health = null;
        PickupItem pickup = null;
        float fallingTime = 3f;
        float fallingSpeed = 0.01f;
        bool isDead = false;
        float endJourneyDelay = 6f;
        bool isEndJourney = false;

        Coroutine currentCoroutine = null;
        ElapsedTime ElapsedTimePersistentObject = null;
        Score ScorePersistentObject = null;

        public delegate void AtmosphereLayerChangeDelegate(int layerId);
        public event AtmosphereLayerChangeDelegate onAtmosphereLayerChange;

        public delegate void TimeCounterChangeDelegate(int elapsedTime);
        public event TimeCounterChangeDelegate onTimeCounterChange;

        public event Action onEndJourney;

        private void Awake()
        {
            UpdateCurrentAtmosphereLayerValues(atmosphereLayers[currentLayerIndex]);

            ElapsedTimePersistentObject = GameObject.FindObjectOfType<ElapsedTime>();
            ScorePersistentObject = GameObject.FindObjectOfType<Score>();
            ElapsedTimePersistentObject.SetElapsedTime(0);
            ScorePersistentObject.SetScore(0);

            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            pickup = GameObject.FindWithTag("Player").GetComponent<PickupItem>();

            health.onTakenDamage += StoppedByHit;
            health.onDead += Falling;
            pickup.onPickupItemBoost += Boost;
        }

        private void Start()
        {
            onAtmosphereLayerChange(currentLayerIndex);
        }

        public void Boost(float boostTime)
        {
            if (currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(SpeedUpAndSlowDown(boostTime));
            }
        }

        private IEnumerator SpeedUpAndSlowDown(float boostTime)
        {
            speedFactor = maxSpeedFactor;
            yield return new WaitForSeconds(boostTime);
            speedFactor = GetDefaultSpeedFactorLayer();
            currentCoroutine = null;
        }

        public void StoppedByHit()
        {
            speedFactor = GetDefaultSpeedFactorLayer();
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
        }

        public void Falling()
        {
            isDead = true;
            ElapsedTimePersistentObject.SetElapsedTime(-1);
            StartCoroutine(ReverseSpeed());
        }

        private IEnumerator ReverseSpeed()
        {
            while (fallingTime > 0)
            {
                fallingTime -= 1 * Time.deltaTime / 10f;
                speedFactor -= fallingSpeed;
                yield return null;
            }
        }

        private void UpdateCurrentAtmosphereLayerValues(AtmosphereLayer layer)
        {
            moonApproachFactor = layer.moonApproachFactorLayer;
            speedFactor = layer.speedFactorLayer;
        }

        private void Update()
        {
            if (!isEndJourney)
            {
                UpdateTimeCounter();
                ProcessMoonApproach();
            }
        }

        private void UpdateTimeCounter()
        {
            totalElapsedTime += Time.deltaTime;
            onTimeCounterChange(GetElapsedTimeFormatted(totalElapsedTime));
        }

        public float GetMoonSpeed()
        {
            return GetSpeedFactor() * moonApproachFactor;
        }

        public float GetMoonDistance()
        {
            return distanceToMoon - distanceTravelled; ;
        }

        public float GetDistanceTravelled()
        {
            return distanceTravelled;
        }

        public float GetAtmosphereLayerCount()
        {
            return atmosphereLayers.Count;
        }

        private void UpdateMoonPosition()
        {
            if (currentLayerIndex + 1 == GetAtmosphereLayerCount())
            {
                moon.transform.position = closestMoonDistance;
            }
            else
            {
                moon.transform.position = new Vector3(
                    0,
                    0,
                    GetMoonDistance() * atmosphereLayers[currentLayerIndex].moonSpeedFactor
                );
            }
        }

        public float GetSpeedFactor()
        {
            return speedFactor;
        }

        public float GetDefaultSpeedFactorLayer()
        {
            return atmosphereLayers[currentLayerIndex].speedFactorLayer;
        }

        public void SetSpeedFactor(float newFactor)
        {
            speedFactor = newFactor;
        }

        public float GetMinSpeedFactor()
        {
            return minSpeedFactor;
        }

        public float GetMaxSpeedFactor()
        {
            return maxSpeedFactor;
        }

        private void ProcessMoonApproach()
        {
            distanceTravelled += GetSpeedFactor() * moonApproachFactor;
            UpdateMoonPosition();
            calculateSpeedFormatted(distanceTravelled);
            UpdateAtmosphereLayerName(distanceTravelled);
        }

        public float GetSpeedFormatted()
        {
            return speedFormatted;
        }

        public void calculateSpeedFormatted(float currentDistance)
        {
            elapsedTimeSpeedCounter += Time.deltaTime;
            if (elapsedTimeSpeedCounter >= 1f)
            {
                speedFormatted = (currentDistance - lastMoonDistance) * 3600;
                elapsedTimeSpeedCounter = elapsedTimeSpeedCounter % 1f;
                lastMoonDistance = currentDistance;
            }
        }

        private int GetElapsedTimeFormatted(float elapsedTime)
        {
            return (int)elapsedTime;
        }

        private void UpdateAtmosphereLayerName(float distance)
        {
            if (distance > atmosphereLayers[currentLayerIndex].endDistance)
            {
                if (currentLayerIndex + 1 <= atmosphereLayers.Count)
                {
                    moveToNextAtmosphereLayer();
                }
            }
        }

        private void moveToNextAtmosphereLayer()
        {
            if (isDead) return;

            if (IsLastLayer(currentLayerIndex))
            {
                StartCoroutine(LandOnTheMoon());
            }
            else
            {
                currentLayerIndex++;
                onAtmosphereLayerChange(currentLayerIndex);
                UpdateCurrentAtmosphereLayerValues(atmosphereLayers[currentLayerIndex]);
            }
        }

        private IEnumerator LandOnTheMoon()
        {
            isEndJourney = true;
            onEndJourney();
            yield return new WaitForSeconds(endJourneyDelay);
            ElapsedTimePersistentObject.SetElapsedTime(GetElapsedTimeFormatted(totalElapsedTime));
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private bool IsLastLayer(int layerId)
        {
            return atmosphereLayers.Count == layerId + 1;
        }

        public string GetAtmosphereLayerName()
        {
            return atmosphereLayers[currentLayerIndex].name;
        }
    }
}
