using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class AnimatorManager : MonoBehaviour
    {
        [SerializeField] protected List<RuntimeAnimatorController> combatAnimations;
        [SerializeField] protected List<RuntimeAnimatorController> deathAnimations;
        protected SortedList<string, RuntimeAnimatorController> playerCombatAnimators;
        protected SortedList<string, RuntimeAnimatorController> playerDeathAnimators;

        private void Awake()
        {
            playerCombatAnimators = new SortedList<string, RuntimeAnimatorController>();
            playerDeathAnimators = new SortedList<string, RuntimeAnimatorController>();
            LoadDeathAnimations();
            LoadCombatAnimators();
        }

        protected void LoadDeathAnimations()
        {
            foreach (RuntimeAnimatorController a in deathAnimations)
            {
                string name = a.name.ToString();
                playerDeathAnimators[name] = a;
            }
        }

        protected void LoadCombatAnimators()
        {
            foreach (RuntimeAnimatorController a in combatAnimations)
            {
                string name = a.name.ToString();
                playerCombatAnimators[name] = a;
            }
        }

        public RuntimeAnimatorController RandomDeathAnimator()
        {
            int rng = Random.Range(0, playerDeathAnimators.Count);
            return playerDeathAnimators.Values[rng];
        }

        public RuntimeAnimatorController GetDeathAnimator(string name)
        {
            RuntimeAnimatorController a = null;
            try { a = playerDeathAnimators[name]; }
            catch (System.Exception) { }
            return a;
        }

        public RuntimeAnimatorController GetCombatAnimator(string name)
        {
            RuntimeAnimatorController a = null;
            try { a = playerCombatAnimators[name]; }
            catch (System.Exception) { }
            return a;
        }

    }
}

