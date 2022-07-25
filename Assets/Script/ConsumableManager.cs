using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;
using AudioManagerPkg;
using UnityEngine.Networking;
using GameManager;

namespace Character
{
    public class ConsumableManager : MonoBehaviour
    {
        [SerializeField] protected AudioManager audioManager;
        protected NetManager netManager;
        protected PlayerLifeManager playerLife;
        protected WeaponManager weaponManager;
        protected PowerUpManager powerupManager;
        protected InGameUIManager inGameUI;


        public NetManager NetM
        {
            set { netManager = value; }
            get { return netManager; }
        }

        private void Awake()
        {
            playerLife = GetComponent<PlayerLifeManager>();
            weaponManager = GetComponent<WeaponManager>();
            powerupManager = GetComponent<PowerUpManager>();
            inGameUI = GetComponent<PlayerManagerScript>().InGameUI;
        }

        public void ApplyAura(GameObject o)
        {
            CheckManager(o);
            BeginCooldown(o);
        }

        public void CheckManager(GameObject o)
        {
            Consumable c = o.GetComponent<Consumable>();
            c.Pickup(gameObject);
            if(c.FeedbackColor._isFeedback) inGameUI.DoFeedback(c.FeedbackColor._feedbackColor);
            // send command to server
        }

        protected void BeginCooldown(GameObject o)
        {
            audioManager.PlaySound(o.GetComponent<AudioScript>().Clip);
            netManager.ConsumablePickedUp(o);
        }
    }
}

