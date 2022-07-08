using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class PlayerManagerScript : MonoBehaviour
    {
        [SerializeField] protected List<GameObject> characters;
        [SerializeField] protected Camera mainCam;
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
            thisChar = Instantiate(characters[0], playerPosition, Quaternion.identity, transform);
            yPos = thisChar.transform.localPosition.y;
        }

        private void FixedUpdate()
        {
            thisChar.transform.localPosition = new Vector3(0, yPos, 0);
        }

    }
}

