using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

namespace Character
{
    public class ShootingScript : MonoBehaviour
    {
        [SerializeField] protected float dmg = 1.0f;
        [SerializeField] protected float range = 100.0f;
        [SerializeField] protected Camera _camera;
        [SerializeField] protected GameObject _powerupManager;
        protected PowerUpManager powerUp;
        protected CharacterStatus status;
        protected WeaponManager weaponManager;
        private float curr_dmg;
        private float curr_range;
        private bool canShoot = true;
        protected DmgDoneCalc ddc;

        private void Awake()
        {
            status = GetComponent<CharacterStatus>();
            weaponManager = GetComponent<WeaponManager>();
            ddc = GetComponent<DmgDoneCalc>();
            powerUp = _powerupManager.GetComponent<PowerUpManager>();
        }


        void FixedUpdate()
        {
            if (status.IsFiring && canShoot)
            {
                DoShoot();
            }

            if (powerUp.GetTimeRemaining("QuadDamage") > 0) ddc.AddBuff("QuadDamage", powerUp.GetAura("QuadDamage"));
            else ddc.RemoveBuff("QuadDamage");

        }

        private void DoShoot()
        {
            canShoot = false;
            weaponManager.TriggerShoot();
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hitInfo, range))
            {
                if (hitInfo.transform.gameObject.CompareTag("Player"))
                {
                    float finalDmg = ddc.CalcDmg(weaponManager.GetActiveWeaponDamage());
                    Debug.Log(finalDmg);
                    // SendToServer();
                }
            }
            StartCoroutine(ShootCooldown());
        }

        private IEnumerator ShootCooldown()
        {

            yield return new WaitForSeconds(weaponManager.GetActiveWeaponShootDelay());
            canShoot = true;
        }

    }
}

