using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class UiFeedback : MonoBehaviour
    {
        [SerializeField] protected float feedbackTime = 1.0f;
        [SerializeField] protected float maxTransparency = 20.0f;

        public float FeedbackTime
        {
            get { return feedbackTime; }
        }

        public float MaxTransparency
        {
            get { return maxTransparency; }
        }

    }

}
