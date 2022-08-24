using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Character
{
    public class ActivableManager : MonoBehaviour
    {
        protected Dictionary<string, Activable> activables = new Dictionary<string, Activable>();
        protected int current = 0;
        protected ComponentManager cm;
        protected CharacterStatus cs;
        protected float timer = 0;
        protected bool action = true;
        [SerializeField] protected float interval = 1;

        private void Awake()
        {
            cm = GetComponent<ComponentManager>();
            cs = GetComponent<CharacterStatus>();
        }
        
        private void FixedUpdate()
        {
            activables = cm.FilterByType<Activable>();
            if (activables.Count <= 0) return;
            CheckTimer();
            CheckActivablesBound();
            if (cs.Activate) Activate();
            else if (cs.ActivateNext) NextActivable();
            else if (cs.ActivatePre) PreviousActivable();
        }

        protected void CheckTimer()
        {
            timer -= Time.deltaTime;    
            if (timer <= 0)
            {
                timer = 0;
                action = true;
            }
            else  action = false;
        }

        protected void NextActivable()
        {
            if (action)
            {
                current += 1;
                if (current >= activables.Count) current = 0;
                timer = interval;
            }
            
        }

        protected void PreviousActivable()
        {
            if (action)
            {
                current -= 1;
                if (current < 0) current = activables.Count - 1;
                timer = interval;
            }
        }

        protected void Activate()
        {
            if (action)
            {
                activables.ElementAt(current).Value.Activate();
                timer = interval;
            }
        }

        protected void CheckActivablesBound()
        {
            if (current >= activables.Count) current = activables.Count - 1;
        }

        public string GetCurrent()
        {
            try
            {
                return activables.ElementAt(current).Key;
            }
            catch (Exception) { return ""; }
        }

    }
}

