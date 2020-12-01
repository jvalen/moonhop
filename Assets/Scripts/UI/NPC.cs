using System;
using System.Collections;
using System.Collections.Generic;
using MoonHop.Attributes;
using MoonHop.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MoonHop.UI
{
    public class NPC : MonoBehaviour
    {
        [SerializeField] List<AudioSource> voiceClipList = null;
        [SerializeField] AudioSource beepSound = null;
        [SerializeField] AudioSource interferenceSound = null;
        [SerializeField] AudioSource gameoverVoice = null;
        [SerializeField] Animator animator;

        JourneyPhysics journeyPhysics = null;
        Health health = null;
        Image face = null;

        Coroutine talkCoroutine = null;

        private void Awake()
        {
            journeyPhysics = GameObject.FindWithTag("GameController").GetComponent<JourneyPhysics>();
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            journeyPhysics.onAtmosphereLayerChange += ShowNPC;
            health.onDead += Falling;
            face = GetComponent<Image>();
            DisableNPC();
        }

        private void Falling()
        {
            StopCoroutine(talkCoroutine);
            StartCoroutine(NPCGameOver());
        }

        private IEnumerator NPCGameOver()
        {
            beepSound.Play();
            yield return new WaitForSeconds(1f);
            animator.SetBool("leave", false);
            animator.SetBool("isDead", true);
            interferenceSound.Play();
            gameoverVoice.Play();
            EnableNPC();
        }

        public void ShowNPC(int layerId)
        {
            talkCoroutine = StartCoroutine(NPCTalks(layerId));
        }

        private IEnumerator NPCTalks(int layerId)
        {
            yield return new WaitForSeconds(1f);
            beepSound.Play();
            animator.SetBool("leave", false);
            EnableNPC();
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("connected", true);
            yield return new WaitForSeconds(1f);
            PlayVoice(layerId);
            yield return new WaitForSeconds(2f);
            DisableNPC();
            animator.SetBool("connected", false);
            animator.SetBool("leave", true);
        }

        private void PlayVoice(int voiceId)
        {
            voiceClipList[voiceId].Play();
        }

        public void EnableNPC()
        {
            face.enabled = true;
        }

        public void DisableNPC()
        {
            face.enabled = false;
        }
    }
}
