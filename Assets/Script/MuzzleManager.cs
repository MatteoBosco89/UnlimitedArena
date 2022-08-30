using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Weapon
{
    public class MuzzleManager : NetworkBehaviour
    {
        [SerializeField] protected GameObject muzzle;
        [SerializeField] protected float interval = 0.02f;
        protected float timer = 0;
        protected bool active = false;

        public IEnumerator Muzzle()
        {
            muzzle.SetActive(true);
            yield return new WaitForSeconds(interval);
            muzzle.SetActive(false);
        }

    }
}

