using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Weapon;
using GameManager;
using System.IO;

namespace Character
{
    public class PlayerManagerScript : NetworkBehaviour
    {
        [SerializeField] protected List<GameObject> characters;
        [SerializeField] protected Camera mainCam;
        [SerializeField] protected GameObject animatorManagerObj;
        [SerializeField] protected float yPos = -2.0f;
        [SerializeField] protected float deathCooldown = 2.0f;
        [SerializeField] protected GameObject inGameUIObj;
        [SerializeField] protected List<string> collidersTags;
        protected FeatureManager featureManager;
        protected ComponentManager componentManager;
        protected float startDeathCooldown = 0;
        protected GameObject networkManagerObj;
        protected WeaponManager weaponManager;
        protected AnimatorManager animatorManager;
        protected PlayerLifeManager lifeManager;
        protected CountDownManager countDownManager;
        protected TickManager tickManager;
        protected GameObject thisChar = null;
        protected GameObject thisCharWeapon = null;
        protected Vector3 playerPosition;
        protected NetManager netManager;
        protected CharacterStatus characterStatus;
        protected MovePlayer mp;
        protected bool isDeathCooldown = false;
        protected InGameUIManager inGameUI;
        [SyncVar] protected int chosenPlayer;
        protected Vector3 theSpawnPosition = new Vector3(0, 0, 0);
        [SyncVar] protected int clientId = 0;

        public FeatureManager PlayerFeatures
        {
            get { return featureManager; }
        }

        public ComponentManager ComponentManager
        {
            get { return componentManager; }
        }

        public Vector3 TheSpawnPosition
        {
            get { return theSpawnPosition; }
            set { theSpawnPosition = value; }
        }

        public float YPos
        {
            set { yPos = value; }
        }
        public int ChosenPlayer
        {
            get { return chosenPlayer; }
            set { chosenPlayer = value; }
        }
        public int ClientId
        {
            get { return clientId; }
            set { clientId = value; }
        }
        public NetManager NetMan
        {
            get { return netManager; }
        }

        public GameObject SpawnedChar
        {
            get { return thisChar; }
            set { thisChar = value; }
        }

        public InGameUIManager InGameUI
        {
            get { return inGameUI; }
        }

        private void Awake()
        {
            featureManager = GetComponent<FeatureManager>();
            componentManager = GetComponent<ComponentManager>();
            countDownManager = GetComponent<CountDownManager>();
            tickManager = GetComponent<TickManager>();
            networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
            weaponManager = GetComponent<WeaponManager>();
            animatorManager = animatorManagerObj.GetComponent<AnimatorManager>();
            netManager = networkManagerObj.GetComponent<NetManager>();
            lifeManager = GetComponent<PlayerLifeManager>();
            characterStatus = GetComponent<CharacterStatus>();
            mp = GetComponent<MovePlayer>();
            inGameUI = inGameUIObj.GetComponent<InGameUIManager>();
            collidersTags = collidersTags.ConvertAll(s => s.ToUpper());
        }

        public void ActivateCam()
        {
            if (isLocalPlayer) mainCam.gameObject.SetActive(true);
        }

        void Start()
        {
            if (isLocalPlayer) netManager.PManager = GetComponent<PlayerManagerScript>();
            ActivateModel();
            thisCharWeapon = SearchByTag(thisChar, "WeaponContainer");
            weaponManager.WeaponContainer = thisCharWeapon;
            weaponManager.Spawn();
            Settings();
            if (isLocalPlayer) PlayerReset();
            SetAnimator(weaponManager.ActiveWeaponStatus.WeaponType);
            ActivateCam();      
        }

        private void Settings()
        {
            // poi vediamo
        }

        private void ActivateModel()
        {
           thisChar.SetActive(true);
        }

        private void PlayerReset()
        {
            lifeManager.ResetPlayerLife();
            weaponManager.ResetWeapons();
            //powerUpManager.ResetPowerUps();    
        }

        private void StartDeathCooldown()
        {
            isDeathCooldown = true;
            startDeathCooldown = Time.time;
        }

        private void FixedUpdate()
        {
            if (thisChar != null)
            {
                if (thisChar.activeSelf && !lifeManager.IsDead)
                {
                    thisChar.transform.localPosition = new Vector3(0, yPos, 0);
                    UpdateAnimator();
                }
            }

            if (lifeManager.IsDead && !isDeathCooldown)
            {
                SetDeathAnimator();
                StartDeathCooldown();
            }

            if (isLocalPlayer && lifeManager.IsDead && isDeathCooldown)
            {
                if (Time.time - startDeathCooldown >= deathCooldown && characterStatus.IsJumping)
                {
                    netManager.SignalDeath(clientId);
                    isDeathCooldown = false;
                }
            }

            // for debugging purpose
            if (isLocalPlayer && characterStatus.IsChangingWeaponsPre) lifeManager.TakeDamage(10);
            if (isLocalPlayer && characterStatus.IsChangingWeaponsNext) componentManager.Print();
        }

        private void OnTriggerEnter(Collider other)
        {
            string tag = other.gameObject.tag.ToUpper();
            if (InTagList(tag))
            {
                string path = other.gameObject.GetComponent<PathManager>().Path;
                string[] n = path.Split('.');
                string name =  Path.GetFileName(n[0]);
                componentManager.ComponentPickup(tag, name, path);
                inGameUI.CollectibleFeedback(other.gameObject);
            }  
        }

        protected bool InTagList(string s)
        {
            return collidersTags.Contains(s);
        }

        public void SetPvP(bool flag)
        {
            GetComponent<ShootingScript>().Pvp = flag;
        }

        protected GameObject SearchByTag(GameObject o, string tag)
        {
            foreach (Transform t in o.GetComponentsInChildren<Transform>())
            {
                if (t.CompareTag(tag)) return t.gameObject;
            }
            return null;
        }

        public void SetAnimator(string animatorId)
        {
            GetComponent<Animator>().runtimeAnimatorController = animatorManager.GetCombatAnimator(animatorId);
            thisChar.GetComponent<Animator>().runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
        }

        public void SetDeathAnimator()
        {
            mainCam.transform.localPosition = new Vector3(mainCam.transform.localPosition.x, mainCam.transform.localPosition.y, -1);
            mainCam.transform.localRotation = Quaternion.Euler(40, 0, 0);
            GetComponent<Animator>().runtimeAnimatorController = animatorManager.RandomDeathAnimator();
            thisChar.GetComponent<Animator>().runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
        }

        private void UpdateAnimator()
        {
            float speed = GetComponent<Animator>().GetFloat("speed");
            thisChar.GetComponent<Animator>().SetFloat("speed", speed);
        }

        public override void PreStartClient()
        {
            base.PreStartClient();
            thisChar = Instantiate(characters[chosenPlayer], transform.localPosition, gameObject.transform.rotation, transform);
        }

        public AnimatorManager AnimatorManager
        {
            get { return animatorManager; }
        }

        [Command]
        protected void CmdDeathCooldown(bool val)
        {
            isDeathCooldown = val;
        }

        public float FeatureValue(string f)
        {
            return PlayerFeatures.FeatureValue(f);
        }

        public float TickValue(string f)
        {
            return PlayerFeatures.TickValue(f);
        }

        public Dictionary<string, float> GetAllTicks(string type)
        {
            return ComponentManager.GetAllTicks(type);
        }

    }
}

