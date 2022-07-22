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
        [SerializeField] protected Text maxHealth;
        [SerializeField] protected Text health;
        [SerializeField] protected Text armor;
        [SerializeField] protected Image lifeBar;
        [SerializeField] protected Text weapon;
        [SerializeField] protected Text ammo;
        [SerializeField] protected Image placeHolder;
        [SerializeField] protected Image healthFeedback;
        protected PlayerLifeManager lifeManager;
        protected WeaponManager weaponManager;
        protected ConsumableManager consumableManager;
        protected float placeholderVal;
        protected float maxVal;
        protected bool isFeedback = false;
        protected float feedbackTime = 0.0f;
        protected float maxTransparency = 0.0f;
        protected bool colorUp = false;

        void Awake()
        {
            lifeManager = playerManager.GetComponent<PlayerLifeManager>();
            weaponManager = playerManager.GetComponent<WeaponManager>();
            consumableManager = playerManager.GetComponent<ConsumableManager>();
            maxHealth.text = lifeManager.MaxHealth.ToString();
            placeholderVal = Mathf.CeilToInt(placeHolder.rectTransform.sizeDelta.x);
            maxVal = 1 / (float)lifeManager.MaxHealth;
            healthFeedback.color = new Color(0, 0, 0, 0);
            feedbackTime = healthFeedback.GetComponent<UiFeedback>().FeedbackTime;
            maxTransparency = healthFeedback.GetComponent<UiFeedback>().MaxTransparency;
        }

        private void FixedUpdate()
        {
            health.text = lifeManager.Health.ToString();
            armor.text = lifeManager.Armor.ToString();
            weapon.text = "";//weaponManager.GetActiveWeaponName();
            //if (weaponManager.ActiveWeaponStatus.Has_infinite_ammo) { ammo.text = "\u221E"; ammo.fontSize = 65; }  
            //else { ammo.text = weaponManager.GetActiveWeaponAmmo().ToString(); ammo.fontSize = 30; }
            ammo.text = "\u221E";
            lifeBar.rectTransform.sizeDelta = new Vector2(LifeBarCalc(lifeManager.Health), lifeBar.rectTransform.sizeDelta.y);
            if (isFeedback)
            {
                Color c = healthFeedback.color;
                if (colorUp){ 
                    if (healthFeedback.color.a < maxTransparency) c.a += feedbackTime; 
                    if (healthFeedback.color.a >= maxTransparency) colorUp = false; 
                }  
                if(!colorUp) c.a -= feedbackTime; 
                healthFeedback.color = c;
                if (healthFeedback.color.a <= 0) isFeedback = false;
            }
        }

        protected float LifeBarCalc(int val)
        {
            return (val * maxVal) * placeholderVal;
        }

        public void DoFeedback(Color color)
        {
            color.a = 0.0f;
            healthFeedback.color = color;
            colorUp = true;
            isFeedback = true;
        }
    }
}

