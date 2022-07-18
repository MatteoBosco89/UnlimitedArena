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
        [SerializeField] protected GameObject weaponManagerObj;
        [SerializeField] protected GameObject animatorManagerObj;
        protected GameObject networkManagerObj;
        protected ConsumableManager consumableManager;
        protected WeaponManager weaponManager;
        protected AnimatorManager animatorManager;
        protected GameObject thisChar = null;
        protected GameObject thisCharWeapon = null;
        protected Vector3 playerPosition;
        protected float yPos;
        protected NetManager netManager;

        public GameObject SpawnedChar
        {
            get { return thisChar; }
        }

        private void Awake()
        {
            networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
            consumableManager = consumableManagerObj.GetComponent<ConsumableManager>();
            weaponManager = weaponManagerObj.GetComponent<WeaponManager>();
            animatorManager = animatorManagerObj.GetComponent<AnimatorManager>();
            netManager = networkManagerObj.GetComponent<NetManager>();
            consumableManager.NetM = netManager;
        }

        private void Start()
        {
            playerPosition = transform.position;
            playerPosition.y -= 1;
            thisChar = Instantiate(characters[PlayerPrefs.GetInt("character")], playerPosition, Quaternion.identity, transform);
            yPos = thisChar.transform.localPosition.y;
            thisChar.SetActive(true);
            thisCharWeapon = SearchByTag(thisChar, "WeaponContainer");
            weaponManager.WeaponContainer = thisCharWeapon;
            weaponManager.Spawn();
            CmdSpawnPlayer();
        }

        [Command]
        void CmdSpawnPlayer()
        {
            NetworkServer.SpawnWithClientAuthority(thisChar, connectionToClient);
        }

        [Command]
        public void CmdSpawnWeapon(GameObject weapon)
        {
            NetworkServer.SpawnWithClientAuthority(weapon, connectionToClient);
        }

        private void FixedUpdate()
        {
            thisChar.transform.localPosition = new Vector3(0, yPos, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            consumableManager.ApplyAura(other.gameObject);
        }

        protected GameObject SearchByTag(GameObject o, string tag)
        {
            foreach (Transform t in o.GetComponentsInChildren<Transform>()) if (t.CompareTag(tag)) return t.gameObject;
            return null;
        }

        public void SetAnimator(string animatorId)
        {
            animatorManager.LoadAnimators();
            thisChar.GetComponent<Animator>().runtimeAnimatorController = animatorManager.GetAnimator(animatorId);
        }

        public AnimatorManager AnimatorManager
        {
            get { return animatorManager; }
        }

    }
}

