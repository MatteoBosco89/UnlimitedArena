using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Activable : UAComponent
    {
        protected CountDownManager cdmanager;
        protected ComponentManager compManager;
        protected TickManager tm;

        public Activable(string name, string path, CountDownManager countDownManager, TickManager tim, ComponentManager componentManager) : base(name, path)
        {
            cdmanager = countDownManager;
            compManager = componentManager;
            tm = tim;
        }

        protected override void LoadMe(string path)
        {
            //base.LoadMe(path);
        }

        protected void Activate()
        {
            Powerup p = new Powerup(componentId, componentPath, cdmanager, tm);
            compManager.AddComponent(p);
            compManager.RemoveComponent(this);
        }

    }
}

