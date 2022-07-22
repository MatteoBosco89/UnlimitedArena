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
        }

        public void ApplyAura(GameObject o)
        {
            CheckManager(o);
            BeginCooldown(o);
        }

        public void CheckManager(GameObject o)
        {
            if (o.CompareTag("PowerUp")) powerupManager.PowerUpPickup(o);
            if (o.CompareTag("Weapon")) weaponManager.PickUpWeapon(o);
            if (o.CompareTag("Ammo")) weaponManager.AddAmmoToWeapon(o);
            if (o.CompareTag("Consumable")) playerLife.PickConsumable(o);
        }

        protected void BeginCooldown(GameObject o)
        {
            audioManager.PlaySound(o.GetComponent<AudioScript>().Clip);
            netManager.ConsumablePickedUp(o);
        }
    }
}

