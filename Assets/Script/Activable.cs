using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Activable : UAComponent
    {
        protected CountDownManager cdmanager;
        protected TickManager tm;

        public Activable(string name, string path, ComponentManager comm) : base(name, path, comm)
        {
            tm = cm.TheTickManager;
            cdmanager = cm.TheCountDownManager;
        }

        protected override void LoadMe(string path)
        {
            //base.LoadMe(path);
        }

        protected void Activate()
        {
            Powerup p = new Powerup(componentId, componentPath, cm);
            cm.AddComponent(p);
            cm.RemoveComponent(this);
        }

    }
}

