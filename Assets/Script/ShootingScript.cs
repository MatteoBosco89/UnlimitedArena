using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Linq;

namespace Character
{
    public class ShootingScript : NetworkBehaviour
    {
        
        [SerializeField] protected float range = 100.0f;
        [SerializeField] protected Camera _camera;
        [SerializeField] protected string DAMAGEDONE = "DAMAGE_DONE";
        [SerializeField] protected string DEFAULTDAMAGEDONE = "PHYSICALDAMAGEDONE";
        [SerializeField] protected string DEFAULTDAMAGERESIST = "PHYSICALDAMAGE";
        [SerializeField] protected string damageWrapperDirectory;
        protected Dictionary<string, string> damageWrapper = new Dictionary<string, string>();
        protected CharacterStatus status;
        protected WeaponManager weaponManager;
        private bool canShoot = true;
        protected PlayerManagerScript playerManager;
        [SyncVar] protected bool pvp = true;


        public bool Pvp
        {
            get { return pvp; }
            set { pvp = value; }
        }

        private void Awake()
        {
            damageWrapperDirectory = Path.Combine(Application.streamingAssetsPath, damageWrapperDirectory);
            playerManager = GetComponent<PlayerManagerScript>();
            status = GetComponent<CharacterStatus>();
            weaponManager = GetComponent<WeaponManager>();
            LoadDamageWrapper();
        }


        void FixedUpdate()
        {
            if (!isLocalPlayer) return;
            if (status.IsFiring && canShoot && !status.IsPaused) DoShoot();
        }

        private void DoShoot()
        {
            DamagePacket();
            canShoot = false;
            weaponManager.TriggerShoot();
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hitInfo, range))
            {
                GameObject enemy = hitInfo.transform.gameObject;
                if (enemy.CompareTag("Player") && pvp)
                {
                    DamageDone dmg = new DamageDone(playerManager.PlayerName, playerManager.ClientId);
                    dmg.DamageList = DamagePacket();
                    dmg.effects = weaponManager.MyComponentManager.Components.Keys.ToList();
                    string jsonString = JsonUtility.ToJson(dmg);
                    Debug.Log(jsonString);
                    CmdShootEnemyPlayer(enemy, jsonString);
                }
            }
            StartCoroutine(ShootCooldown());
        }

        protected void LoadDamageWrapper()
        {
            if (!CheckDirectory(damageWrapperDirectory))
            {
                damageWrapper.Add(DEFAULTDAMAGEDONE, DEFAULTDAMAGERESIST);
                return;
            }
            string[] fileEntries = Directory.GetFiles(damageWrapperDirectory, "*.csv");
            foreach (string fileName in fileEntries)
            {
                LoadSingleFile(fileName);
            }
        }

        protected void LoadSingleFile(string fileName)
        {
            if (!CheckFile(fileName))
            {
                damageWrapper.Add(DEFAULTDAMAGEDONE, DEFAULTDAMAGERESIST);
                return;
            }
            string[] lines = File.ReadAllLines(fileName);
            foreach (string l in lines)
            {
                string[] items = l.Split(',');
                string damage = items[0].Trim();
                string resist = items[1].Trim();
                damageWrapper.Add(damage, resist);
            }
        }

        protected bool CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.LogError("Directory Not Found");
                return false;
            }
            return true;
        }

        protected bool CheckFile(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("File Not Found");
                return false;
            }
            return true;
        }

        protected Dictionary<string, float> DamagePacket()
        {
            Dictionary<string, float> damagePacket = new Dictionary<string, float>();
            FeatureManager fm = weaponManager.MyFeatureManager;
            foreach(KeyValuePair<string, Feature> kvf in fm.Features)
            {
                if(damageWrapper.ContainsKey(kvf.Key)) damagePacket.Add(damageWrapper[kvf.Key], kvf.Value.CurrValue);
            }
            return damagePacket;
        }

        [Command]
        private void CmdShootEnemyPlayer(GameObject enemy, string dmg)
        {
            enemy.GetComponent<PlayerLifeManager>().ForeignDamage(dmg);
        }

        private IEnumerator ShootCooldown()
        {
            yield return new WaitForSeconds(weaponManager.GetActiveWeaponShootDelay());
            canShoot = true;
        }

        protected int CalcDamage(float baseDmg)
        {
            return Mathf.CeilToInt(playerManager.PlayerFeatures.FeatureValue(DAMAGEDONE) * baseDmg);
        }

        [Serializable]
        public class DamageDone
        {
            public string player;
            public int playerid;
            protected Dictionary<string, float> damageDone;
            public List<string> damageType = new List<string>();
            public List<float> damageValue = new List<float>();
            public List<string> effects = new List<string>();
            public DamageDone(string p, int id)
            {
                player = p;
                playerid = id;
            }
            public Dictionary<string, float> DamageList
            {
                get { DeserializeDmg();  return damageDone; }
                set { damageDone = value; SerializeDmg(); }
            }
            public string Player
            {
                get { return player; }
                set { player = value; }
            }
            public List<string> Effects
            {
                get { return effects; }
                set { effects = value; }
            }
            protected void DeserializeDmg()
            {
                damageDone = new Dictionary<string, float>();
                for(int i = 0; i < damageType.Count; i++)
                {
                    damageDone.Add(damageType.ElementAt(i), damageValue.ElementAt(i));
                }
            }

            protected void SerializeDmg()
            {
                foreach(KeyValuePair<string, float> k in damageDone)
                {
                    damageType.Add(k.Key);
                    damageValue.Add(k.Value);
                }
            }

        }
        [Serializable]
        public class EffectList
        {
            public List<string> effects;
            public List<string> Effects
            {
                get { return effects; }
                set { effects = value; }
            }
            public EffectList(List<string> eff) { effects = eff; }
        }

    }
}

