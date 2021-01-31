using System.Collections;
using System.Collections.Generic;
using MoonHop.Core;
using MoonHop.Enemies;
using UnityEngine;
using static MoonHop.Waves.WaveDefinition;

namespace MoonHop.Waves
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] AudioSource mainMusic = null;
        [SerializeField] AudioSource bossMusic = null;
        [SerializeField] List<WaveDefinition> waves;

        Coroutine currenCoroutineWave = null;
        JourneyPhysics journey;
        EnemyAI currentBoss = null;
        float delayFactor = 1f;
        bool keepSpawning = true;
        float bossStartDelay = 4f;

        private void Awake()
        {
            journey = GameObject.FindWithTag("GameController").GetComponent<JourneyPhysics>();
            journey.onAtmosphereLayerChange += LaunchWave;
            journey.onEndJourney += BossDefeated;
        }

        private IEnumerator SpawnLoop(int waveId)
        {
            if (waveId >= waves.Count)
            {
                yield return null;
            }
            else
            {
                Wave wave = waves[waveId].GetWave();

                if (wave.finalBoss == null)
                {

                    while (keepSpawning)
                    {
                        if (wave.startWaveDelay > 0)
                        {
                            yield return new WaitForSeconds(wave.startWaveDelay * delayFactor);
                        }
                        foreach (WaveAction action in wave.actions)
                        {
                            if (action.enabled)
                            {
                                yield return new WaitForSeconds(getActionDelay(wave, action) * delayFactor);

                                if (action.objectDefinitionList.Count > 0 && action.spawnCount > 0)
                                {
                                    for (int i = 0; i < action.spawnCount; i++)
                                    {
                                        foreach (ObjectPosition objectPosition in action.objectPositionList)
                                        {
                                            Vector3 newObstaclePosition = new Vector3(objectPosition.x, objectPosition.y, transform.position.z);
                                            int randomSelectedObjet = Random.Range(0, action.objectDefinitionList.Count);

                                            if (objectPosition.randomOffsetFactor > 0)
                                            {
                                                newObstaclePosition = newObstaclePosition + (UnityEngine.Random.insideUnitSphere * objectPosition.randomOffsetFactor);
                                            }
                                            action.objectDefinitionList[randomSelectedObjet].SpawnFloatingObject(newObstaclePosition);
                                            if (action.burst > 0)
                                            {
                                                yield return new WaitForSeconds(action.burst);
                                            }
                                        }
                                        if (action.spawnCount > 1)
                                        {
                                            yield return new WaitForSeconds(action.spawnDelay);
                                        }
                                    }
                                }
                            }

                        }
                        yield return null;

                        yield return null;
                    }

                }
                else
                {
                    yield return StartBossLevel(wave);
                }
            }
        }

        private IEnumerator StartBossLevel(Wave wave)
        {
            yield return FadeOutMusic(mainMusic);
            yield return new WaitForSeconds(bossStartDelay);
            bossMusic.Play();
            currentBoss = Instantiate(wave.finalBoss);
            currentBoss.transform.position = transform.position;
        }

        private void BossDefeated()
        {
            StartCoroutine(FadeOutMusic(bossMusic));
        }

        private IEnumerator FadeOutMusic(AudioSource music)
        {
            while (music.volume > 0)
            {
                music.volume -= 1 * Time.deltaTime / bossStartDelay;
                yield return null;
            }
            music.Stop();
        }

        private float getActionDelay(Wave wave, WaveAction action)
        {
            return wave.shareActionDelay > 0 ? wave.shareActionDelay : (action.actionDelay > 0 ? action.actionDelay : 0);
        }

        private void LaunchWave(int waveId)
        {
            if (currenCoroutineWave != null)
            {
                StopWave();
            }
            currenCoroutineWave = StartCoroutine(SpawnLoop(waveId));
        }

        private void StopWave()
        {
            StopCoroutine(currenCoroutineWave);
        }
    }
}