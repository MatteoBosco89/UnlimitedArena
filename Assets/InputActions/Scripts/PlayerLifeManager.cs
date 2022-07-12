using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class PlayerLifeManager : MonoBehaviour
    {

        [SerializeField] protected int maxHealth = 100;
        [SerializeField] protected int health = 100;
        [SerializeField] protected int armor = 0;
        [SerializeField] protected float _armorReduction = 0.3f;
        [SerializeField] protected float _armorReductionOnHit = 0.2f;
        [SerializeField] protected GameObject _powerupManager;
        protected bool isDead = false;
        protected string normalizeHealth = "NormalizeHealth";
        protected DmgReceivedCalc dmgReceived;
        protected PowerUpManager powerUp;

        private void Start()
        {
            InvokeRepeating(normalizeHealth, 0.1f, 1f);
            dmgReceived = GetComponent<DmgReceivedCalc>();
            powerUp = _powerupManager.GetComponent<PowerUpManager>();
        }

        public bool IsDead
        {
            get { return isDead; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
        }

        public int Health
        {
            get { return health; }
        }

        public int Armor
        {
            get { return armor; }
        }

        public void DoDamage(int dmg)
        {
            health -= dmgReceived.CalcDamageReceived(dmg);
            if (armor > 0) ReduceArmor(dmg);
        }

        public void Heal(int healing)
        {
            health += healing;
            if (health > maxHealth) { InvokeRepeating(normalizeHealth, 0.1f, 1f); }
        }
        private void FixedUpdate()
        {
            if (health <= 0) {
                health = 0;
                isDead = true; 
            }
            if (armor > 0) dmgReceived.AddBuff("armor", _armorReduction);
            else dmgReceived.RemoveBuff("armor");

            if (powerUp.GetTimeRemaining("Invincibility") > 0)
            {
                dmgReceived.AddBuff("Invincibility", powerUp.GetAura("Invincibility"));
                Debug.Log(powerUp.GetTimeRemaining("Invincibility"));
            }
            else dmgReceived.RemoveBuff("Invincibility");


        }

        protected void NormalizeHealth()
        {
            if (health <= maxHealth) CancelInvoke(normalizeHealth);
            else health -= 1;
        }

        protected void ReduceArmor(int dmg)
        {
            float reduction = dmg * _armorReductionOnHit;
            armor -= Mathf.FloorToInt(reduction);
            if (armor <= 0) armor = 0;
        }

        protected void TestDamage()
        {
            DoDamage(20);
        }

    }
}

