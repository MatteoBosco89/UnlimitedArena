using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class PlayerManagerScript : MonoBehaviour
    {
        [SerializeField] protected List<GameObject> characters;
        [SerializeField] protected Camera mainCam;
        [SerializeField] protected ConsumableManager consumableManager;
        protected GameObject thisChar = null;
        protected Vector3 playerPosition;
        protected float yPos;

        public GameObject SpawnedChar
        {
            get { return thisChar; }
        }

        private void Start()
        {
            playerPosition = transform.position;
            playerPosition.y -= 1;
            thisChar = Instantiate(characters[PlayerPrefs.GetInt("character")], playerPosition, Quaternion.identity, transform);
            yPos = thisChar.transform.localPosition.y;
            thisChar.SetActive(true);
        }

        private void FixedUpdate()
        {
            thisChar.transform.localPosition = new Vector3(0, yPos, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            consumableManager.ApplyAura(other.gameObject);
        }

    }
}

