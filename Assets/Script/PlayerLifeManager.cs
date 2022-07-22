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
        [SerializeField] protected int initialHealth = 100;
        [SerializeField] protected int initialArmor = 25;
        [SerializeField] protected float _armorReduction = 0.7f;
        [SerializeField] protected float _armorReductionOnHit = 0.5f;
        protected DmgReceivedCalc dmgReceived;
        protected PowerUpManager powerUp;
        protected ConsumableHandler consumableHandler;
        protected CharacterStatus characterStatus;
        [SyncVar] protected int armor;
        [SyncVar] protected int health;
        [SyncVar] protected bool isDead = false;

        private void Awake()
        {
            dmgReceived = GetComponent<DmgReceivedCalc>();
            powerUp = GetComponent<PowerUpManager>();
            characterStatus = GetComponent<CharacterStatus>();
            armor = initialArmor;
            health = initialHealth;
        }

        public void ResetPlayerLife()
        {
            isDead = false;
            CmdAlive();
            CmdAddArmor(initialArmor);
            CmdHeal(initialHealth);
            characterStatus.IsAlive = true;
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
            if (isDead) return;
            health -= dmgReceived.CalcDamageReceived(dmg);
            if (armor > 0) ReduceArmor(dmg);
        }

        private void Heal(int healing)
        {
            if (isDead) return;
            health += healing;
            if (health > maxHealth) health = maxHealth;
        }

        private void AddArmor(int a)
        {
            if (isDead) return;
            armor += a;
            if (armor > maxArmor) armor = maxArmor;
        }

        private void FixedUpdate()
        {
            if (health <= 0)
            {
                health = 0;
                isDead = true;
                CmdDeath();
                characterStatus.IsAlive = false;
            }

            if (armor > 0) dmgReceived.AddBuff("armor", _armorReduction);
            else dmgReceived.RemoveBuff("armor");

            if (powerUp.GetTimeRemaining("Invincibility") > 0)
            {
                dmgReceived.AddBuff("Invincibility", powerUp.GetAura("Invincibility"));
            }
            else dmgReceived.RemoveBuff("Invincibility");
        }

        [Command]
        protected void CmdHeal(int amount)
        {
            Heal(amount);
        }

        [Command]
        protected void CmdAddArmor(int amount)
        {
            AddArmor(amount);
        }

        [Command]
        protected void CmdDeath()
        {
            isDead = true;
        }

        [Command]
        protected void CmdAlive()
        {
            isDead = false;
        }

        protected void ReduceArmor(int dmg)
        {
            float reduction = dmg * _armorReductionOnHit;
            armor -= Mathf.CeilToInt(reduction);
            if (armor <= 0) armor = 0;
        }

        public void PickConsumable(GameObject o)
        {
            consumableHandler = o.GetComponent<ConsumableHandler>();
            if (consumableHandler.Id.Equals("Medikit")) Heal(consumableHandler.Value);
            if (consumableHandler.Id.Equals("Armor")) AddArmor(consumableHandler.Value);
        }

    }
}

