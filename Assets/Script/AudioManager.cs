using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AudioManagerPkg
{
    public class AudioManager : MonoBehaviour
    {
        protected AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }


        public void PlaySound(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

    }
}

