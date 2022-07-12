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
        [SerializeField] protected GameObject wpManager;
        protected CharacterStatus status;
        protected WeaponManager weaponManager;
        private float curr_dmg;
        private float curr_range;
        private bool canShoot = true;

        private void Awake()
        {
            status = GetComponent<CharacterStatus>();
            weaponManager = wpManager.GetComponent<WeaponManager>();
        }


        void FixedUpdate()
        {
            if (status.IsFiring && canShoot)
            {
                Shoot();
            }
        }

        void Shoot()
        {
            RaycastHit hitInfo;
            canShoot = false;
            weaponManager.ShotWeapon();
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo, range))
            {
                Debug.Log(hitInfo.transform.name);
                //give damage to object
            }
            StartCoroutine(ShotCooldown());
        }
        private IEnumerator ShotCooldown()
        {

            yield return new WaitForSeconds(weaponManager.GetActiveWeaponShotDelay());
            canShoot = true;
        }

    }
}

