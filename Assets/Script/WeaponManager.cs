using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

namespace Weapon
{
    public class WeaponManager : MonoBehaviour
    {

        [SerializeField] protected List<GameObject> weaponsList;
        [SerializeField] protected GameObject playerManager;
        protected PlayerManagerScript pms;
        private SortedDictionary<int, bool> enabledWeapons;
        private SortedDictionary<int, GameObject> playerWeapons;

        private AudioSource audioS;
        private GameObject activeWeapon = null;
        private int activeWeaponId;
        private WeaponStatus activeWeaponStatus;

        private bool changeButtonsPressed = false;
        protected CharacterStatus status;

        protected GameObject weaponContainer;

        public GameObject ActiveWeapon
        {
            get { return activeWeapon; }
        }

        public GameObject WeaponContainer
        {
            get { return weaponContainer; }
            set { weaponContainer = value; }
        }

        private void Awake()
        {
            pms = playerManager.GetComponent<PlayerManagerScript>();
        }

        public void Spawn()
        {
            if (weaponsList.Count > 0)
            {
                enabledWeapons = new SortedDictionary<int, bool>();
                playerWeapons = new SortedDictionary<int, GameObject>();
                status = playerManager.GetComponent<CharacterStatus>();
                audioS = playerManager.GetComponent<AudioSource>();
                for (int i = 0; i < weaponsList.Count; i++)
                {
                    GameObject weapon = Instantiate(weaponsList[i], weaponContainer.transform.position, weaponContainer.transform.rotation, weaponContainer.transform);
                    weapon.SetActive(false);
                    enabledWeapons.Add(weapon.GetComponent<WeaponStatus>().Id, false);
                    playerWeapons.Add(weapon.GetComponent<WeaponStatus>().Id, weapon);
                }
                ChangeWeapon(0);
            }

        }

        void Update()
        {
            if (weaponsList.Count > 0)
            {
                SelectWeapon();
                CheckButtonPressed();
            }
        }

        private void SelectWeapon()
        {
            if (status.IsChangingWeaponsNext && !changeButtonsPressed) ChangeWeapon(NextWeapon());
            if (status.IsChangingWeaponsPre && !changeButtonsPressed) ChangeWeapon(PreWeapon());
        }

        public void PickUpWeapon(GameObject pickWeap)
        {
            WeaponConsumable pick_consumable = pickWeap.GetComponent<WeaponConsumable>();
            if (enabledWeapons.ContainsKey(pick_consumable.Id))
            {
                if (enabledWeapons[pick_consumable.Id])
                {
                    WeaponStatus w_stat = playerWeapons[pick_consumable.Id].GetComponent<WeaponStatus>();
                    w_stat.AddAmmo(pick_consumable.Ammo);
                }
                else
                {
                    enabledWeapons[pick_consumable.Id] = true;
                    ChangeWeapon(pick_consumable.Id);
                    activeWeaponStatus.AddAmmo(pick_consumable.Ammo);
                }
            }
        }

        public void AddAmmoToWeapon(GameObject ammoBox)
        {
            AmmoBox ab = ammoBox.GetComponent<AmmoBox>();
            int id = ab.Id;
            int ammo = ab.Ammo;
            if (playerWeapons.ContainsKey(id))
            {
                WeaponStatus weps = playerWeapons[id].GetComponent<WeaponStatus>();
                weps.AddAmmo(ammo);
            }
        }

        public void TriggerShoot()
        {
            if (activeWeaponStatus.Ammo > 0 || activeWeaponStatus.Has_infinite_ammo) WeaponShoot();
            else if (activeWeaponStatus.Ammo <= 0 && !activeWeaponStatus.Has_infinite_ammo) ChangeWeapon(NextWeapon());
        }

        protected void WeaponShoot()
        {
            audioS.clip = activeWeaponStatus.Fire_sound;
            audioS.Play();
            activeWeaponStatus.Shoot();
        }

        private int NextWeapon()
        {
            int i;
            bool found = false;
            for (i = activeWeaponId + 1; i < playerWeapons.Count && !found; i++)
            {
                if (enabledWeapons[i])
                {
                    WeaponStatus wep_stat = playerWeapons[i].GetComponent<WeaponStatus>();
                    if (wep_stat.Ammo > 0 || wep_stat.Has_infinite_ammo)
                        found = true;
                }
            }
            if (found) return i - 1;
            return 0;
        }

        private int PreWeapon()
        {
            int i;
            bool found = false;
            for (i = activeWeaponId - 1; i >= 0 && !found; i--)
            {
                if (enabledWeapons[i])
                {
                    WeaponStatus wep_stat = playerWeapons[i].GetComponent<WeaponStatus>();
                    if (wep_stat.Ammo > 0 || wep_stat.Has_infinite_ammo)
                        found = true;
                }
            }
            if (found) return i + 1;
            return 0;
        }

        protected void ChangeWeapon(int weaponId)
        {
            if(activeWeapon != null) activeWeapon.SetActive(false);
            activeWeaponId = weaponId;
            activeWeapon = playerWeapons[activeWeaponId];
            activeWeapon.SetActive(true);
            activeWeaponStatus = activeWeapon.GetComponent<WeaponStatus>();
            WeaponInPosition();
            AnimateByWeapon();
        }

        protected void WeaponInPosition()
        {
            weaponContainer.transform.localPosition = activeWeaponStatus.Position;
            weaponContainer.transform.localRotation = Quaternion.Euler(activeWeaponStatus.Rotation);
        }

        public WeaponStatus ActiveWeaponStatus
        {
            get { return activeWeaponStatus; }
        }

        protected void AnimateByWeapon()
        {
            pms.SetAnimator(activeWeaponStatus.WeaponType);
        }

        private void CheckButtonPressed()
        {
            if (!status.IsChangingWeaponsNext && !status.IsChangingWeaponsPre) changeButtonsPressed = false;
            else changeButtonsPressed = true;
        }

        public float GetActiveWeaponShootDelay()
        {
            return activeWeaponStatus.Time_between_shot;
        }

        public string GetActiveWeaponName()
        {
            return activeWeaponStatus.WeaponName;
        }

        public int GetActiveWeaponAmmo()
        {
            return activeWeaponStatus.Ammo;
        }

        public float GetActiveWeaponDamage()
        {
            return activeWeaponStatus.Damage;
        }

        public float GetActiveWeaponRange()
        {
            return activeWeaponStatus.Range;
        }

        public int GetActiveWeaponMaxAmmo()
        {
            return activeWeaponStatus.Max_ammo;
        }

    }
}
