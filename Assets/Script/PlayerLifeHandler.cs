using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class PlayerLifeHandler : MonoBehaviour
    {
        [SerializeField] protected string _id;
        [SerializeField] protected Medikit _medikit;
        [SerializeField] protected Armor _armor;
        
        public string Id
        {
            get { return _id; }
        }

        

        public Medikit MedikitItem
        {
            get { return _medikit; }
        }

        public Armor ArmorItem
        {
            get { return _armor; }
        }

        public class CustomizedLifeConsumable
        {
            public bool _isEnabled;
            public int _amount;
        }

        [System.Serializable]
        public class Medikit : CustomizedLifeConsumable { }
        [System.Serializable]
        public class Armor : CustomizedLifeConsumable
        {
            public ArmorType _armorType;
            public float _armorReducedOnHit = 0.3f;
            public enum ArmorType { Light, Medium, Heavy, None};

            public ArmorType ArmorTypeOption
            {
                get { return _armorType; }
            }

        }


    }
}

