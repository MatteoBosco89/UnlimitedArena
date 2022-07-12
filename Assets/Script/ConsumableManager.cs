using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Character
{
    public class ConsumableManager : MonoBehaviour
    {
        [SerializeField] protected PowerUpManager powerupManager;
        

        public void ApplyAura(GameObject o)
        {
            CheckManager(o);
        }

        public void CheckManager(GameObject o)
        {
            if (o.tag == "PowerUp") powerupManager.PowerUpPickup(o);
        }


    }
}

