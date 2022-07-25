using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Consumable : MonoBehaviour
    {

        [SerializeField] protected Color _feedbackColor;

        public Color FeedbackColor
        {
            get { return _feedbackColor; }
        }

        public virtual void Pickup(GameObject player) { }


    }
}

