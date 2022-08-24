using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Activable : UAComponent
    {
        protected CountDownManager cdmanager;
        protected TickManager tm;
        protected string powerupName;
        protected string activableSuffix = "activable";

        public Activable(string name, string path, ComponentManager comm) : base(name, path, comm)
        {
            powerupName = name;
            componentId = name + activableSuffix; 
            tm = cm.TheTickManager;
            cdmanager = cm.TheCountDownManager;
        }

        protected override void LoadMe(string path)
        {
            //base.LoadMe(path);
        }

        public void Activate()
        {
            Powerup p = new Powerup(powerupName, componentPath, cm);
            cm.AddComponent(p);
            cm.RemoveComponent(componentId);
        }

    }
}

