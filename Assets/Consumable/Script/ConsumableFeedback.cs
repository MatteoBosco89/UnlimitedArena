using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class ConsumableFeedback : MonoBehaviour
    {
        [SerializeField] protected AudioClip onPickupSound;
        [SerializeField] protected FeedbackColorOptions feedbackColor;

        public AudioClip Clip
        {
            get { return onPickupSound; }
        }

        public FeedbackColorOptions FeedbackColor
        {
            get { return feedbackColor; }
        }

        [System.Serializable]
        public class FeedbackColorOptions
        {
            public Color _feedbackColor;
            public bool _isFeedback;
        }
    }
}

