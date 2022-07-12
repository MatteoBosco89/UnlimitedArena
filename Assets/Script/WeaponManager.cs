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
        private AudioSource audioS;
        private Dictionary<int, bool> enabledWeaponList = new Dictionary<int, bool>();
        private Dictionary<int, GameObject> playerWeapons = new Dictionary<int, GameObject>();
        protected GameObject activeWeapon;
        private int actWepIndex;
        private bool changeButtonsPressed = false;
        protected CharacterStatus status;
        
        public GameObject ActiveWeapon
        {
            get { return activeWeapon; }
        }
        void Awake()
        {
            if (weaponsList.Count > 0)
            {
                status = playerManager.GetComponent<CharacterStatus>();
                audioS = playerManager.GetComponent<AudioSource>();
                Debug.Log("Weapon manager Start");
                enabledWeaponList.Add(weaponsList[0].GetComponent<WeaponStatus>().Id, true);
                //set i = 1 after testing
                for (int i = 1; i < weaponsList.Count; i++)
                {
                    Debug.Log(i);
                    enabledWeaponList.Add(weaponsList[i].GetComponent<WeaponStatus>().Id, false);
                    weaponsList[i].SetActive(false);
                }
                AddWeapon();
                activeWeapon = weaponsList[0];
                actWepIndex = 0;
                activeWeapon.SetActive(true);

            }

        }

        void Update()
        {
            if(weaponsList.Count > 0)
            {
                SelectWeapon();
                CheckButtonPressed();
            }
        }

        private void AddWeapon()
        {
            for (int i = 0; i < weaponsList.Count; i++)
            {
                WeaponStatus wps = weaponsList[i].GetComponent<WeaponStatus>();
                if (enabledWeaponList[wps.Id])
                {
                    if(!playerWeapons.ContainsKey(i))
                        playerWeapons.Add(wps.Id, weaponsList[i]);
                }
            }
        }

        private void SelectWeapon()
        {
            if (status.IsChangingWeaponsNext && !changeButtonsPressed)
            {
                Debug.Log("Weapon: " + activeWeapon.name);
                activeWeapon.SetActive(false);
                actWepIndex = NextWeapon();
                activeWeapon = playerWeapons[actWepIndex];
                activeWeapon.SetActive(true);
                Debug.Log("Weapon: " + activeWeapon.name);
            }
            if (status.IsChangingWeaponsPre && !changeButtonsPressed)
            {
                Debug.Log("Weapon: " + activeWeapon.name);
                activeWeapon.SetActive(false);
                actWepIndex = PreWeapon();
                activeWeapon = playerWeapons[actWepIndex];
                activeWeapon.SetActive(true);
                Debug.Log("Weapon: " + activeWeapon.name);
            }
        }

        public void PickUpWeapon(GameObject pickWeap)
        {
            WeaponConsumable pick_stat = pickWeap.GetComponent<WeaponConsumable>();
            if (enabledWeaponList[pick_stat.Id])
            {
                //increase ammo
                Debug.Log("Increase ammo of " + pick_stat.Id);
                WeaponStatus w_stat = playerWeapons[pick_stat.Id].GetComponent<WeaponStatus>();
                w_stat.AddAmmo(pick_stat.Ammo);
            }
            else
            {
                //enable weapon for use
                enabledWeaponList[pick_stat.Id] = true;
                AddWeapon();
                WeaponStatus w_stat = playerWeapons[pick_stat.Id].GetComponent<WeaponStatus>();
                w_stat.AddAmmo(pick_stat.Ammo);
                activeWeapon.SetActive(false);
                activeWeapon = playerWeapons[pick_stat.Id];
                activeWeapon.SetActive(true);
                actWepIndex = pick_stat.Id;
                Debug.Log("Picked " + pick_stat.Id);
            }
        }

        public void AddAmmToWeapon(int id, int ammo)
        {
            for(int i = 0; i < weaponsList.Count; i++)
            {
                WeaponStatus weps = weaponsList[i].GetComponent<WeaponStatus>();
                if(weps.Id == id)
                {
                    weps.AddAmmo(ammo);
                }
            }
        }

        public void ShotWeapon()
        {
            WeaponStatus active_stats = activeWeapon.GetComponent<WeaponStatus>();
            if (active_stats.Ammo > 0 || active_stats.Has_infinite_ammo) {
                audioS.clip = active_stats.Fire_sound;
                audioS.Play();
                active_stats.Shot();
            }
            else if(active_stats.Ammo <= 0 && !active_stats.Has_infinite_ammo)
            {
                Debug.Log("next");
                activeWeapon.SetActive(false);
                actWepIndex = NextWeapon();
                activeWeapon = playerWeapons[actWepIndex];
                activeWeapon.SetActive(true);
            }
        }

        private int NextWeapon()
        {
            int i;
            bool found = false;
            for (i = actWepIndex + 1; i < playerWeapons.Count && !found; i++)
            {
                if (playerWeapons.ContainsKey(i))
                {
                    WeaponStatus wep_stat = playerWeapons[i].GetComponent<WeaponStatus>();
                    if (wep_stat.Ammo>0 || wep_stat.Has_infinite_ammo)
                        found = true;
                }
            }
            if (found)
            {
                return i - 1;
            }
            return 0;
        }

        private int PreWeapon()
        {
            int i;
            bool found = false;
            for (i = actWepIndex - 1; i >= 0 && !found; i--)
            {
                if (playerWeapons.ContainsKey(i))
                {
                    WeaponStatus wep_stat = playerWeapons[i].GetComponent<WeaponStatus>();
                    if (wep_stat.Ammo > 0 || wep_stat.Has_infinite_ammo)
                        found = true;
                }
            }
            if (found)
            {
                return i + 1;
            }
            return 0;
        }

        private void CheckButtonPressed()
        {
            if (!status.IsChangingWeaponsNext && !status.IsChangingWeaponsPre)
            {
                changeButtonsPressed = false;
            }
            else
            {
                changeButtonsPressed = true;
            }
        }
        
        public float GetActiveWeaponShotDelay()
        {
            return ActiveWeapon.GetComponent<WeaponStatus>().Time_between_shot;
        }

        public int GetActiveWeaponAmmo()
        {
            return ActiveWeapon.GetComponent<WeaponStatus>().Ammo;
        }

        public float GetActiveWeaponDamage()
        {
            return activeWeapon.GetComponent<WeaponStatus>().Damage;
        }

        public float GetActiveWeaponRange()
        {
            return activeWeapon.GetComponent<WeaponStatus>().Range;
        }

        public int GetActiveWeaponMaxAmmo()
        {
            return activeWeapon.GetComponent<WeaponStatus>().Max_ammo;
        }

    }
}
