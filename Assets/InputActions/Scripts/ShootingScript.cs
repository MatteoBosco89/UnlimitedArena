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
        [SerializeField] protected GameObject WeaponManagerObj;
        protected CharacterStatus status;
        protected WeaponManager weaponManager;
        private float curr_dmg;
        private float curr_range;
        private bool canShoot = true;

        private void Awake()
        {
            status = GetComponent<CharacterStatus>();
            weaponManager = WeaponManagerObj.GetComponent<WeaponManager>();
        }


        void FixedUpdate()
        {
            if (status.IsFiring && canShoot)
            {
                DoShoot();
            }
        }

        private void DoShoot()
        {
            canShoot = false;
            weaponManager.TriggerShoot();
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hitInfo, range))
            {
                if (hitInfo.transform.gameObject.CompareTag("Player")) Debug.Log("Player Hit");
                // SendToServer(weaponManager.GetActiveWeaponDamage());
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

