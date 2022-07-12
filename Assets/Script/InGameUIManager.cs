using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class InGameUIManager : MonoBehaviour
    {
        [SerializeField] protected GameObject playerManager;
        [SerializeField] protected Text maxHealth;
        [SerializeField] protected Text health;
        [SerializeField] protected Text armor;
        protected PlayerLifeManager lifeManager;

        void Awake()
        {
            lifeManager = playerManager.GetComponent<PlayerLifeManager>();
        }

        private void FixedUpdate()
        {
            maxHealth.text = lifeManager.MaxHealth.ToString();
            health.text = lifeManager.Health.ToString();
            armor.text = lifeManager.Armor.ToString();
        }
    }
}

