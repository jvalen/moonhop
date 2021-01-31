using System.Collections;
using System.Collections.Generic;
using MoonHop.Attributes;
using MoonHop.Core;
using MoonHop.FloatingObjects;
using UnityEngine;

namespace MoonHop.Enemies
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] List<FloatingObjectDefinition> objectDefinitionList;
        [SerializeField] AudioSource throwSound = null;

        [SerializeField] float maxX = 3.5f;
        [SerializeField] float minX = -3.5f;
        [SerializeField] float maxY = 2.5f;
        [SerializeField] float minY = -2.5f;

        [SerializeField] float bossSpeed = 4f;
        [SerializeField] float bossMovementRadio = 7f;

        protected Health target = null;

        JourneyPhysics journey;
        Coroutine coroutine = null;
        Vector2 newBossPosition = new Vector3();
        int objectToThrowIndex = 0;
        float currentShootFrequency = 0.3f;
        bool keepThrowing = true;
        bool isDead = false;
        float elapsedTime = 0;
        int stateChangeCounter = 1;
        float totalMoveDuration = 1f;

        private void Awake()
        {
            target = GameObject.FindWithTag("Player").GetComponent<Health>();
            journey = GameObject.FindWithTag("GameController").GetComponent<JourneyPhysics>();
            journey.onEndJourney += BossDefeated;
        }

        private void Start()
        {
            coroutine = StartCoroutine(ThrowObject());
        }

        private void Update()
        {
            if (!isDead)
            {

                UpdateClock();
                if (stateChangeCounter % 5 == 0)
                {
                    SwitchBossState();
                }
                else
                {
                    ChangeBossDestination();
                    UpdateBossPosition();

                }
            }
            else
            {
                UpdatePositionLeaveLevel();
            }
        }

        private void UpdateClock()
        {
            elapsedTime += Time.deltaTime;
        }

        private void ChangeBossDestination()
        {
            if (elapsedTime >= totalMoveDuration)
            {
                stateChangeCounter++;
                newBossPosition =
                    (new Vector2(transform.position.x, transform.position.y)) +
                    (UnityEngine.Random.insideUnitCircle * bossMovementRadio);
                elapsedTime = elapsedTime % totalMoveDuration;
            }
        }

        private void UpdateBossPosition()
        {
            transform.LookAt(GetAimLocation());
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(
                    Mathf.Clamp(newBossPosition.x, minX, maxX),
                    Mathf.Clamp(newBossPosition.y, minY, maxY),
                    transform.position.z),
                bossSpeed * Time.deltaTime
            );
        }

        private void UpdatePositionLeaveLevel()
        {
            transform.LookAt(GetAimLocation());
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(0, 0,
                    -10),
                bossSpeed * Time.deltaTime
            );
        }

        private IEnumerator ThrowObject()
        {
            FloatingObjectDefinition currentObject = objectDefinitionList[objectToThrowIndex];

            yield return new WaitForSeconds(2f);
            stateChangeCounter = 1;
            objectToThrowIndex = objectToThrowIndex % (objectDefinitionList.Count - 1);
            objectToThrowIndex++;

            if (objectToThrowIndex == 0)
            {
                currentShootFrequency -= 0.02f;
                bossSpeed += 0.2f;
            }

            while (keepThrowing)
            {
                Vector3 newObjectPosition = new Vector3(throwSound.transform.position.x, throwSound.transform.position.y, throwSound.transform.position.z);
                currentObject.SpawnFloatingObject(newObjectPosition);
                throwSound.Play();

                yield return new WaitForSeconds(currentShootFrequency);
            }
        }

        private void SwitchBossState()
        {
            stateChangeCounter = 1;
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(ThrowObject());
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

        private void BossDefeated()
        {
            isDead = true;
            keepThrowing = false;
            StopCoroutine(coroutine);
        }
    }
}
