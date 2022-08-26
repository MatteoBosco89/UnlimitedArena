using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.IO;

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
        [SerializeField] protected string DAMAGEREDUCTION = "DAMAGE_RECEIVED";
        [SerializeField] protected string LIFEFEATUREPATH;
        [SerializeField] protected string TICKSPATH;
        [SerializeField] protected string buffBasePath = "Buff/";
        [SerializeField] protected string ext = ".csv";
        protected int initialArmor = 0;
        protected int initialHealth = 0;
        protected CharacterStatus characterStatus;
        protected InGameUIManager inGameUI;
        protected ScoreTableManager scoreTableManager;
        protected PlayerManagerScript playerManager;
        protected ComponentManager componentManager;
        protected FeatureManager featureManager;
        protected Dictionary<string, float> lifeFeatures = new Dictionary<string, float>();
        protected Dictionary<string, string> tickables = new Dictionary<string, string>();
        protected bool loading = true;
        protected int lastPlayerHitting;
        protected EffectManager effectManager;
        [SyncVar] protected int armor;
        [SyncVar] protected int health;
        [SyncVar] protected bool isDead = false;
        [SyncVar] protected int experience;
        [SyncVar] protected string effectJSON;

        private void Awake()
        {
            LIFEFEATUREPATH = Path.Combine(Application.streamingAssetsPath, LIFEFEATUREPATH);
            TICKSPATH = Path.Combine(Application.streamingAssetsPath, TICKSPATH);
            playerManager = GetComponent<PlayerManagerScript>();
            featureManager = playerManager.PlayerFeatures;
            componentManager = playerManager.ComponentManager;
            characterStatus = GetComponent<CharacterStatus>();
            effectManager = GetComponent<EffectManager>();
            LoadParameters(LIFEFEATUREPATH, lifeFeatures);
            LoadParameters(TICKSPATH, tickables);
        }

        public void Start()
        {
            inGameUI = playerManager.InGameUI;
            scoreTableManager = playerManager.ScoreTable;
            if (isLocalPlayer) CmdAlive();
            lastPlayerHitting = playerManager.ClientId;
        }

        protected void LoadFeatures()
        {
            initialHealth = (int)featureManager.FeatureValue(HEALTH);
            initialArmor = (int)featureManager.FeatureValue(ARMOR);
            if (isLocalPlayer) CmdHealth(initialHealth);
            if (isLocalPlayer) CmdAddArmor(initialArmor);
            isDead = false;
            characterStatus.IsAlive = true;
            loading = false;
            experience = 0;
        }

        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; }
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
            int finalDamage = Mathf.CeilToInt(damage);
            playerManager.ScoreTable.UpdateDamage(lastPlayerHitting, finalDamage);
            if (isLocalPlayer) CmdTakeDamage(finalDamage);
            inGameUI.DoFeedback(damageReceivedColorFeedback);
        }
        public void HealMe(int amount)
        {
            if (isDead) return;
            CmdHeal(amount);
            if (health > maxHealth) health = maxHealth;
        }
        public void AddExp(int amount)
        {
            if (isDead) return;
            experience += amount;
        }

        private void FixedUpdate()
        {
            if (isDead) return;
            if (!featureManager.Loaded) return;
            if (loading && featureManager.Loaded)
            {
                health = (int)playerManager.FeatureValue(HEALTH);
                LoadFeatures();
            }
            ComputeFeature();
            armor = (int)playerManager.FeatureValue(ARMOR);
            maxHealth = (int)playerManager.FeatureValue(HEALTH);
            DoAllTicks();
            CheckEffects();
            if (health <= 0 && !loading)
            {
                health = 0;
                isDead = true;
                if(isLocalPlayer) CmdDeath();
                characterStatus.IsAlive = false;
                scoreTableManager.UpdateKill(lastPlayerHitting);
            }
            CheckIsOutOfBorder();
        }

        protected void DoAllTicks()
        {
            foreach(KeyValuePair<string, string> t in tickables)
            {
                ComputeByComponent(t.Key, t.Value);
            }
        }

        public void ForeignDamage(string dmg)
        {
            ShootingScript.DamageDone damage = JsonUtility.FromJson<ShootingScript.DamageDone>(dmg);
            ShootingScript.EffectList effList = new ShootingScript.EffectList(effectManager.Effects(damage.Effects));
            Dictionary<string, float> damageList = damage.DamageList;
            lastPlayerHitting = damage.playerid;
            TakeDamage(Mathf.CeilToInt(ComputeFeatureValue(damageList)));
            effectJSON = JsonUtility.ToJson(effList);
        }

        protected void CheckEffects()
        {
            if (String.IsNullOrEmpty(effectJSON)) return;
            List<string> eff = JsonUtility.FromJson<ShootingScript.EffectList>(effectJSON).Effects;
            ApplyAruas(eff);
            if (isLocalPlayer)
            {
                effectJSON = null;
                CmdClearEffect();
            }
        }

        protected void ApplyAruas(List<string> effects)
        {
            foreach(string e in effects)
            {
                string path = buffBasePath + e + ext;
                componentManager.ComponentPickup("Powerup", e, path);
            }
        }

        protected void ComputeFeature()
        {
            for(int i = 0; i < lifeFeatures.Count; i++)
            {
                string s = lifeFeatures.Keys.ElementAt(i);
                lifeFeatures[s] = componentManager.FeatureValue(s);
            }
        }

        public void ComputeByComponent(string type, string func)
        {
            Dictionary<string, float> filtered = playerManager.GetAllTicks(type);
            float amount = ComputeFeatureValue(filtered);
            int famount = Mathf.CeilToInt(amount);
            if (famount > 0)
            {
                object[] p = { famount };
                Type thisType = this.GetType();
                MethodInfo theMethod = thisType.GetMethod(func);
                theMethod.Invoke(this, p);
            }
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

        public void SignalAlive()
        {
            CmdAlive();
            CmdHealth(100);
        }

        protected void CheckIsOutOfBorder()
        {
            if (transform.localPosition.y < -30) health = 0;
        }

        [Command]
        protected void CmdClearEffect()
        {
            effectJSON = null;
        }

        [Command]
        protected void CmdTakeDamage(int amount)
        {
            health -= amount;
        }

        [Command]
        protected void CmdHeal(int amount)
        {
            health += amount;
        }

        [Command]
        protected void CmdHealth(int amount)
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
                    string reductionName = c.ModifierNameByFeature(ARMORREDUCTION);
                    float reduction = c.MyModifiers[reductionName].MultFactor;
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

        protected void LoadParameters<T1, T2>(string path, Dictionary<T1, T2> paramDict)
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string l in lines)
            {
                string[] items = l.Split(',');
                object param1 = items[0].Trim();
                object param2 = items[1].Trim();
                if (typeof(T2) == typeof(float)) param2 = ParseFloatValue(items[1]);
                paramDict.Add((T1)param1, (T2)param2);
            }
        }

        protected float ParseFloatValue(string val)
        {
            return float.Parse(val, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }


    }
}

