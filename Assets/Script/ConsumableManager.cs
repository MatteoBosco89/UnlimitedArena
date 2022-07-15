using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;
using AudioManagerPkg;

namespace Character
{
    public class ConsumableManager : MonoBehaviour
    {
        [SerializeField] protected PowerUpManager powerupManager;
        [SerializeField] protected WeaponManager weaponManager;
        [SerializeField] protected float consumableCooldown = 30.0f; 
        [SerializeField] protected AudioManager audioManager;
        [SerializeField] protected PlayerLifeManager playerLife;

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
            // send to server
            audioManager.PlaySound(o.GetComponent<AudioScript>().Clip);
            o.SetActive(false);
            StartCoroutine(SetCooldown(o));
        }

        IEnumerator SetCooldown(GameObject o)
        {
            yield return new WaitForSeconds(consumableCooldown);
            o.SetActive(true);
        }

    }
}

