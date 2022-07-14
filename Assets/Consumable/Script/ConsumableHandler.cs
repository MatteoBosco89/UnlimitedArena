using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class ConsumableHandler : MonoBehaviour
    {
        [SerializeField] protected string id;
        [SerializeField] protected int value;

        public string Id
        {
            get { return id; }
        }

        public int Value
        {
            get { return value; }
        }
    }
}

