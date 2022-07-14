using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Weapon
{
    public class AmmoBox : MonoBehaviour
    {
        [SerializeField] protected int id;
        [SerializeField] protected int ammo;
        public int Id
        {
            get { return id; }
        }
        public int Ammo
        {
            get { return ammo; }
        }
        // Start is called before the first frame update
    }
}

