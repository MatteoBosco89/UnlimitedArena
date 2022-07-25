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
        [SerializeField] protected float _noArmorReduction = 0.0f;
        [SerializeField] protected float _lightArmorReduction = 0.2f;
        [SerializeField] protected float _mediumArmorReduction = 0.3f;
        [SerializeField] protected float _heavyArmorReduction = 0.4f;
        protected int initialArmor = 0;
        protected float _armorReduction = 0;
        protected float _armorReductionOnHit = 0;
        protected PlayerLifeHandler.Armor.ArmorType _currentArmorType = PlayerLifeHandler.Armor.ArmorType.None;
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
            _armorReduction = _noArmorReduction;
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
            if (armor <= 0)
            {
                armor = 0;
                _currentArmorType = PlayerLifeHandler.Armor.ArmorType.None;
            }
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
            if (_currentArmorType != a._armorType)
            {
                _currentArmorType = a._armorType;
                armor = a._amount;
                _armorReduction = CalcReduction();
                _armorReductionOnHit = a._armorReducedOnHit;
            }
            else { armor += a._amount; }
            if (armor > maxArmor) armor = maxArmor;
        }

        protected float CalcReduction()
        {
            if (_currentArmorType == PlayerLifeHandler.Armor.ArmorType.Light) return _lightArmorReduction;
            if (_currentArmorType == PlayerLifeHandler.Armor.ArmorType.Medium) return _mediumArmorReduction;
            if (_currentArmorType == PlayerLifeHandler.Armor.ArmorType.Heavy) return _heavyArmorReduction;
            return _noArmorReduction;
        }

    }
}

