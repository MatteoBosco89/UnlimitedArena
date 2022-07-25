using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Consumable : MonoBehaviour
    {
        public virtual void Pickup(GameObject player) { }
    }
}

