using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Weapon;
using GameManager;

namespace Character
{
    public class PlayerManagerScript : NetworkBehaviour
    {
        [SerializeField] protected List<GameObject> characters;
        [SerializeField] protected Camera mainCam;
        [SerializeField] protected GameObject consumableManagerObj;
        [SerializeField] protected GameObject animatorManagerObj;
        [SerializeField] protected float yPos = -1.0f;
        protected GameObject networkManagerObj;
        protected ConsumableManager consumableManager;
        protected WeaponManager weaponManager;
        protected AnimatorManager animatorManager;
        protected PlayerLifeManager lifeManager;
        protected GameObject thisChar = null;
        protected GameObject thisCharWeapon = null;
        protected Vector3 playerPosition;
        protected NetManager netManager;
        protected int clientId;
        [SyncVar] protected int chosenPlayer;

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

        private void Awake()
        {
            networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
            consumableManager = consumableManagerObj.GetComponent<ConsumableManager>();
            weaponManager = GetComponent<WeaponManager>();
            animatorManager = animatorManagerObj.GetComponent<AnimatorManager>();
            netManager = networkManagerObj.GetComponent<NetManager>();
            lifeManager = GetComponent<PlayerLifeManager>();
            consumableManager.NetM = netManager;
        }

        public void ActivateCam()
        {
            if (isLocalPlayer) mainCam.gameObject.SetActive(true);
        }

        void Start()
        {
            thisCharWeapon = SearchByTag(thisChar, "WeaponContainer");
            weaponManager.WeaponContainer = thisCharWeapon;
            weaponManager.Spawn();
            ActivateCam();
            SetAnimator(weaponManager.ActiveWeaponStatus.WeaponType);
            Debug.LogError("PLAYER MANAGER START");
        }

        private void Update()
        {
            if (thisChar != null)
            {
                thisChar.transform.localPosition = new Vector3(0, yPos, 0);
                UpdateAnimator();
                if (lifeManager.IsDead)
                {
                    //Animazione di morte con setAnimator
                    //funzione reset di consumabili
                    //funzione di cooldown respawn
                    //funzione di respawn
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            consumableManager.ApplyAura(other.gameObject);
        }

        public void SetPvP(bool flag)
        {
            GetComponent<ShootingScript>().Pvp = flag;
        }

        protected GameObject SearchByTag(GameObject o, string tag)
        {
            foreach (Transform t in o.GetComponentsInChildren<Transform>()) if (t.CompareTag(tag)) return t.gameObject;
            return null;
        }

        public void SetAnimator(string animatorId)
        {
            animatorManager.LoadAnimators();
            GetComponent<Animator>().runtimeAnimatorController = animatorManager.GetAnimator(animatorId);
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
            thisChar = Instantiate(characters[chosenPlayer], transform.position, gameObject.transform.rotation, transform);
        }

        public AnimatorManager AnimatorManager
        {
            get { return animatorManager; }
        }

    }
}

