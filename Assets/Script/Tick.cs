using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Tick : OverTime
    {
        protected int tickPerSecond = 1;
        protected float timer = 0;

        public float Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public Tick(UAComponent component, string param, int val) : base(component) 
        {
            parameter = param;
            valuePerSecond = val;
        }

        public float TickInInterval
        {
            get { return valuePerSecond; }
        }

        public override void Activate()
        {
            if(valuePerSecond >= 0) c.TickFeature();
            valuePerSecond -= tickPerSecond;
        }

        public void DoTick()
        {
            Activate();
        }
    }
}
