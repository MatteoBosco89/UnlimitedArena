using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;
using UnityEngine.Networking;

namespace Character
{
    public class ShootingScript : NetworkBehaviour
    {
        
        [SerializeField] protected float range = 100.0f;
        [SerializeField] protected Camera _camera;
        protected PowerUpManager powerUp;
        protected CharacterStatus status;
        protected WeaponManager weaponManager;
        private bool canShoot = true;
        protected DmgDoneCalc ddc;
        [SyncVar] protected bool pvp = true;


        public bool Pvp
        {
            get { return pvp; }
            set { pvp = value; }
        }

        private void Awake()
        {
            status = GetComponent<CharacterStatus>();
            weaponManager = GetComponent<WeaponManager>();
            ddc = GetComponent<DmgDoneCalc>();
            powerUp = GetComponent<PowerUpManager>();
        }


        void FixedUpdate()
        {
            if (!isLocalPlayer) return;

            if (powerUp.GetTimeRemaining("QuadDamage") > 0) ddc.AddBuff("QuadDamage", powerUp.GetAura("QuadDamage"));
            else ddc.RemoveBuff("QuadDamage");

            if (status.IsFiring && canShoot) DoShoot();
        }

        private void DoShoot()
        {

            canShoot = false;
            weaponManager.TriggerShoot();
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hitInfo, range))
            {
                GameObject enemy = hitInfo.transform.gameObject;
                int finalDmg = ddc.CalcDmg(weaponManager.GetActiveWeaponDamage());
                if (enemy.CompareTag("Player") && pvp)
                {
                    CmdShootEnemyPlayer(enemy, finalDmg);
                }
                //if (enemy.CompareTag("Enemy"))
                //{
                //enemy take damage
                //}
            }
            StartCoroutine(ShootCooldown());
        }

        [Command]
        private void CmdShootEnemyPlayer(GameObject enemy, int dmg)
        {
            enemy.GetComponent<PlayerLifeManager>().TakeDamage(dmg);
        }

        private IEnumerator ShootCooldown()
        {
            yield return new WaitForSeconds(weaponManager.GetActiveWeaponShootDelay());
            canShoot = true;
        }

    }
}

