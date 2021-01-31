using System.Collections;
using UnityEngine;

namespace MoonHop.Core
{
    public class EnvironmentEffects : MonoBehaviour
    {
        [SerializeField] ParticleSystem warpParticles = null;
        [SerializeField] ParticleSystem moveToNextLayerEffect = null;
        [SerializeField] AudioSource moveToNextLayerSound = null;
        [SerializeField] float changeLayerEffectTime = 5f;

        JourneyPhysics journeyPhysics = null;
        private ParticleSystemRenderer psr;

        private void Awake()
        {
            journeyPhysics = GetComponent<JourneyPhysics>();
            journeyPhysics.onAtmosphereLayerChange += ShowNextLayerEffect;
            psr = warpParticles.GetComponent<ParticleSystemRenderer>();
        }

        private void Update()
        {
            ProcessWarpEffect();
        }

        private void ShowNextLayerEffect(int waveId)
        {
            if (waveId > 0)
            {
                StartCoroutine(EnableDisableChangeLayerEffect());
            }
        }

        private IEnumerator EnableDisableChangeLayerEffect()
        {
            moveToNextLayerEffect.Play();
            moveToNextLayerSound.Play();
            yield return new WaitForSeconds(changeLayerEffectTime);
            moveToNextLayerEffect.Stop();
        }

        private void ProcessWarpEffect()
        {
            var particleSystemProperties = warpParticles.main;
            particleSystemProperties.simulationSpeed = journeyPhysics.GetSpeedFactor();
            psr.velocityScale = journeyPhysics.GetSpeedFactor() / 20;
        }
    }
}
