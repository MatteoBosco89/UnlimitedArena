using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

namespace Character
{
    public class UIPauseManager : NetworkBehaviour
    {
        [SerializeField] protected GameObject pauseMenu;
        [SerializeField] protected GameObject soundMenu;
        protected CharacterStatus status;
        protected bool inPauseMenu = false;
        private void Awake()
        {
            status = GetComponent<CharacterStatus>();
           

        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer) return;
            if (!inPauseMenu && status.IsPaused)
            {
                IsPause();
            }
            if (inPauseMenu && !status.IsPaused)
            {
                IsGame();
            }
        }

        public void IsPause()
        {
            pauseMenu.SetActive(true);
            soundMenu.SetActive(false);
            inPauseMenu = true;
        }

        public void IsGame()
        {
            pauseMenu.SetActive(false);
            soundMenu.SetActive(false);
            inPauseMenu = false;
            if (status.IsPaused) status.IsPaused = false;
        }

        public void IsSoundOption()
        {
            pauseMenu.SetActive(false);
            soundMenu.SetActive(true);
            inPauseMenu = true;
        }
    }
}

