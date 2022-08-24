using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class ActivableManager : MonoBehaviour
    {
        protected Dictionary<string, Activable> activables = new Dictionary<string, Activable>();
        protected Activable selected;
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
            CheckTimer();
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
                Debug.Log("NEXT");
                timer = interval;
            }
            
        }

        protected void PreviousActivable()
        {
            if (action)
            {
                Debug.Log("PRE");
                timer = interval;
            }
        }

        protected void Activate()
        {
            if (action)
            {
                Debug.Log("ACTIVATE");
                timer = interval;
            }
        }

    }
}

