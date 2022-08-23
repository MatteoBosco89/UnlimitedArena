using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Linq;

namespace Character
{
    public class PlayerLifeManager : NetworkBehaviour
    {
        [SerializeField] protected int maxHealth = 100;
        [SerializeField] protected int maxArmor = 500;
        [SerializeField] protected Color damageReceivedColorFeedback;
        [SerializeField] protected int minArmorReductionOnHit;
        [SerializeField] protected string ARMORREDUCTION = "ARMOR_REDUCTION_ON_HIT";
        [SerializeField] protected string ARMORDURABILITY = "DURABILITY";
        [SerializeField] protected string ARMOR = "ARMOR";
        [SerializeField] protected string HEALTH = "HEALTH";
        [SerializeField] protected string HOT = "HOT";
        [SerializeField] protected string DOT = "DOT";
        [SerializeField] protected string DAMAGEREDUCTION = "DAMAGE_RECEIVED";
        [SerializeField] protected string LIFEFEATUREPATH;
        protected int initialArmor = 0;
        protected int initialHealth = 0;
        protected CharacterStatus characterStatus;
        protected InGameUIManager inGameUI;
        protected PlayerManagerScript playerManager;
        protected ComponentManager componentManager;
        protected FeatureManager featureManager;
        protected Dictionary<string, float> lifeFeatures = new Dictionary<string, float>();
        [SyncVar] protected int armor;
        [SyncVar] protected int health;
        [SyncVar] protected bool isDead = false;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManagerScript>();
            componentManager = playerManager.ComponentManager;
            characterStatus = GetComponent<CharacterStatus>();
            LoadParameters(LIFEFEATUREPATH, lifeFeatures);
        }

        public void Start()
        {
            inGameUI = playerManager.InGameUI;
            initialHealth = (int)playerManager.FeatureValue(HEALTH);
            initialArmor = (int)playerManager.FeatureValue(ARMOR);
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
            ReduceArmor(dmg);
            float damage = ComputateHealthReduction(dmg);          
            health -= Mathf.CeilToInt(damage);
            inGameUI.DoFeedback(damageReceivedColorFeedback);
        }

        private void FixedUpdate()
        {
            ComputeFeature();
            armor = (int)playerManager.FeatureValue(ARMOR);
            maxHealth = (int)playerManager.FeatureValue(HEALTH);
            ComputeDamageByComponent(DOT);
            ComputeHealByComponent(HOT);
            if (health <= 0)
            {
                health = 0;
                isDead = true;
                CmdDeath();
                characterStatus.IsAlive = false;
            }
            CheckIsOutOfBorder();
        }

        protected void ComputeFeature()
        {
            for(int i = 0; i < lifeFeatures.Count; i++)
            {
                string s = lifeFeatures.Keys.ElementAt(i);
                lifeFeatures[s] = componentManager.FeatureValue(s);
            }
        }

        public void ComputeDamageByComponent(string type)
        {
            Dictionary<string, float> filtered = playerManager.GetAllTicks(type);
            float damage = ComputeFeatureValue(filtered);
            if(damage > 0) TakeDamage(Mathf.CeilToInt(damage));
        }

        public void ComputeHealByComponent(string type)
        {
            Dictionary<string, float> filtered = playerManager.GetAllTicks(type);
            float healing = ComputeFeatureValue(filtered);
            if (healing > 0) HealMe(Mathf.CeilToInt(healing));
        }

        public float ComputeFeatureValue(Dictionary<string, float> received)
        {
            float res = 0;
            foreach(string s in received.Keys)
            {
                try
                {
                    float reduction = lifeFeatures[s];
                    res += received[s] * reduction;
                }
                catch (Exception) { }
            }
            return res;
        }

        protected void HealMe(int amount)
        {
            if (isDead) return;
            health += amount;
            if (health > maxHealth) health = maxHealth;
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

        protected void ReduceArmor(float dmg)
        {
            Dictionary<string, UAComponent> comp = componentManager.ComponentsByFeature(ARMOR);
            foreach(UAComponent c in comp.Values)
            {
                try
                {
                    float reduction = c.MyModifiers[ARMORREDUCTION].MultFactor;
                    reduction *= dmg;
                    if (reduction < minArmorReductionOnHit) reduction = minArmorReductionOnHit;
                    c.ReduceComponent(ARMORDURABILITY, reduction);
                }
                catch (Exception) { }
            }
        }

        protected float ComputateHealthReduction(float dmg)
        {
            float reduction = playerManager.PlayerFeatures.FeatureValue(DAMAGEREDUCTION);
            float healthReduction = dmg * reduction;
            return healthReduction;
        }

        protected void LoadParameters(string path, Dictionary<string, float> paramDict)
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string l in lines)
            {
                string[] items = l.Split(',');
                string param1 = items[0].Trim();
                float param2 = ParseFloatValue(items[1]);
                paramDict.Add(param1, param2);
            }
        }

        protected float ParseFloatValue(string val)
        {
            return float.Parse(val, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }

    }
}

