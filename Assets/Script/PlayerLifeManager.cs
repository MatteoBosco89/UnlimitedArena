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
        [SerializeField] protected Color damageReceivedColorFeedback;
        protected int initialArmor = 0;
        protected float _armorReduction = 0;
        protected float _armorReductionOnHit = 0;
        protected DmgReceivedCalc dmgReceived;
        protected PowerUpManager powerUp;
        protected ConsumableHandler consumableHandler;
        protected CharacterStatus characterStatus;
        protected InGameUIManager inGameUI;
        [SyncVar] protected int armor;
        [SyncVar] protected int health;
        [SyncVar] protected bool isDead = false;

        private void Awake()
        {
            dmgReceived = GetComponent<DmgReceivedCalc>();
            powerUp = GetComponent<PowerUpManager>();
            characterStatus = GetComponent<CharacterStatus>();
            inGameUI = GetComponent<PlayerManagerScript>().InGameUI;
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
            float damage = dmgReceived.CalcDamageReceived(dmg);
            if (armor > 0) damage = ReduceArmor(damage);
            health -= Mathf.CeilToInt(damage);
            inGameUI.DoFeedback(damageReceivedColorFeedback);
        }

        private void Heal(PlayerLifeHandler.Medikit m)
        {
            if (!m._isEnabled) return;
            if (isDead) return;
            health += m._amount;
            if (health > maxHealth) health = maxHealth;
        }

        private void AddArmor(PlayerLifeHandler.Armor a)
        {
            if (!a._isEnabled) return;
            if (isDead) return;
            CalcArmor(a);
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

            CheckIsOutOfBorder();
        }

        protected void CheckIsOutOfBorder()
        {
            if (transform.localPosition.y < -30) health = 0;
        }

        [Command]
        protected void CmdHeal(int amount)
        {
            health = amount;
        }

        [Command]
        protected void CmdAddArmor(int amount)
        {
            armor = amount;
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

        protected float ReduceArmor(float dmg)
        {
            float reduction = dmg * _armorReductionOnHit;
            dmg *= _armorReduction;
            armor -= Mathf.CeilToInt(reduction);
            if (armor <= 0) armor = 0;
            return dmg;
        }

        public void PickupLife(GameObject consumable)
        {
            PlayerLifeHandler plh = consumable.GetComponent<PlayerLifeHandler>();
            Heal(plh.MedikitItem);
            AddArmor(plh.ArmorItem);
        }

        public void CalcArmor(PlayerLifeHandler.Armor a)
        {
            if (a._reduction == _armorReduction) armor += a._amount;
            else
            {
                armor = a._amount;
                _armorReduction = a._reduction;
                _armorReductionOnHit = a._reductionOnHit;
            }
            if (armor > maxArmor) armor = maxArmor;
        }

    }
}

