using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class AnimatorManager : MonoBehaviour
    {
        [SerializeField] protected List<RuntimeAnimatorController> animators;
        protected Dictionary<string, RuntimeAnimatorController> playerAnimators;

        private void Awake()
        {
            playerAnimators = new Dictionary<string, RuntimeAnimatorController>();
        }

        public void LoadAnimators()
        {
            foreach (RuntimeAnimatorController a in animators)
            {
                string name = a.name.ToString();
                playerAnimators[name] = a;
            }
        }

        public RuntimeAnimatorController GetAnimator(string name)
        {
            RuntimeAnimatorController a = null;
            try { a = playerAnimators[name]; }
            catch (System.Exception) { }
            return a;
        }

    }
}

