using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Character
{
    public class PlayerLifeManager : NetworkBehaviour
    {

        [SerializeField] protected int maxHealth = 100;
        [SerializeField] protected int maxArmor = 200;
        [SyncVar][SerializeField] protected int health = 100;
        [SyncVar][SerializeField] protected int armor = 50;
        [SerializeField] protected float _armorReduction = 0.7f;
        [SerializeField] protected float _armorReductionOnHit = 0.5f;
        [SerializeField] protected GameObject _powerupManager;
        protected bool isDead = false;
        protected DmgReceivedCalc dmgReceived;
        protected PowerUpManager powerUp;
        protected ConsumableHandler consumableHandler;

        private void Start()
        {
            //InvokeRepeating("TestDamage", 0.1f, 1f);
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
            set { health = value; }
        }

        public int Armor
        {
            get { return armor; }
            set { armor = value; }
        }
        public void TakeDamage(int dmg)
        {
            health -= dmgReceived.CalcDamageReceived(dmg);
            if (armor > 0) ReduceArmor(dmg);
        }

        private void Heal(int healing)
        {
            health += healing;
            if (health > maxHealth) health = maxHealth;
        }

        private void AddArmor(int a)
        {
            armor += a;
            if (armor > maxArmor) armor = maxArmor;
        }

        private void FixedUpdate()
        {
            if (health <= 0)
            {
                health = 0;
                isDead = true;
            }

            if (armor > 0) dmgReceived.AddBuff("armor", _armorReduction);
            else dmgReceived.RemoveBuff("armor");

            if (powerUp.GetTimeRemaining("Invincibility") > 0)
            {
                dmgReceived.AddBuff("Invincibility", powerUp.GetAura("Invincibility"));
            }
            else dmgReceived.RemoveBuff("Invincibility");

        }

        protected void NormalizeHealth()
        {
            if (health <= maxHealth) CancelInvoke("NormalizeHealth");
            else health -= 1;
        }

        protected void ReduceArmor(int dmg)
        {
            float reduction = dmg * _armorReductionOnHit;
            armor -= Mathf.CeilToInt(reduction);
            if (armor <= 0) armor = 0;
        }

        protected void TestDamage()
        {
            //TakeDamage(20);
        }

        public void PickConsumable(GameObject o)
        {
            consumableHandler = o.GetComponent<ConsumableHandler>();
            if (consumableHandler.Id.Equals("Medikit")) Heal(consumableHandler.Value);
            if (consumableHandler.Id.Equals("Armor")) AddArmor(consumableHandler.Value);
        }

    }
}

