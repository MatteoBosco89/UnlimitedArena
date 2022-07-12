using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class PowerupHandler : MonoBehaviour
    {
        [SerializeField] protected string _id;
        [SerializeField] protected int _modifierTime;
        [SerializeField] protected float _multiplier;

        public string Id
        {
            get { return _id; }
        }

        public int ModifierTime
        {
            get { return _modifierTime; }
        }

        public float Multiplier
        {
            get { return _multiplier; }
        }

    }
}
