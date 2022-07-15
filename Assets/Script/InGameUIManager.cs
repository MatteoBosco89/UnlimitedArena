using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weapon;

namespace Character
{
    public class InGameUIManager : MonoBehaviour
    {
        [SerializeField] protected GameObject playerManager;
        [SerializeField] protected GameObject weaponManagerObj;
        [SerializeField] protected GameObject consumableManagerObj;
        [SerializeField] protected Text maxHealth;
        [SerializeField] protected Text health;
        [SerializeField] protected Text armor;
        [SerializeField] protected Image lifeBar;
        [SerializeField] protected Text weapon;
        [SerializeField] protected Text ammo;
        [SerializeField] protected Image placeHolder;
        protected PlayerLifeManager lifeManager;
        protected WeaponManager weaponManager;
        protected ConsumableManager consumableManager;
        protected float placeholderVal;
        protected float maxVal;

        void Awake()
        {
            lifeManager = playerManager.GetComponent<PlayerLifeManager>();
            weaponManager = weaponManagerObj.GetComponent<WeaponManager>();
            consumableManager = consumableManagerObj.GetComponent<ConsumableManager>();
            maxHealth.text = lifeManager.MaxHealth.ToString();
            placeholderVal = Mathf.CeilToInt(placeHolder.rectTransform.sizeDelta.x);
            maxVal = 1 / (float)lifeManager.MaxHealth;
        }

        private void FixedUpdate()
        {
            health.text = lifeManager.Health.ToString();
            armor.text = lifeManager.Armor.ToString();
            weapon.text = weaponManager.GetActiveWeaponName();
            if (weaponManager.ActiveWeaponStatus.Has_infinite_ammo) { ammo.text = "\u221E"; ammo.fontSize = 65; }
            else { ammo.text = weaponManager.GetActiveWeaponAmmo().ToString(); ammo.fontSize = 30; }
            lifeBar.rectTransform.sizeDelta = new Vector2(LifeBarCalc(lifeManager.Health), lifeBar.rectTransform.sizeDelta.y);
        }

        protected float LifeBarCalc(int val)
        {
            return (val * maxVal) * placeholderVal;
        }

    }
}

