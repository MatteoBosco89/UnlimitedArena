using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Consumable : MonoBehaviour
    {

        [SerializeField] protected FeedbackColorOptions feedbackColor;

        public FeedbackColorOptions FeedbackColor
        {
            get { return feedbackColor; }
        }

        public virtual void Pickup(GameObject player) { }

        [System.Serializable]
        public class FeedbackColorOptions
        {
            public Color _feedbackColor;
            public bool _isFeedback;
        }

    }
}

