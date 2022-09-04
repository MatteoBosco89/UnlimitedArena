using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Gear : UAComponent
    {
        protected CountDownManager cdmanager;
        protected TickManager tm;
        protected string TIME = "TIME";
        protected string TICK = "TICK";
        protected int valuePerSecond = 1;

        public Gear(string name, string path, ComponentManager comm) : base(name, path, comm)
        {
            cdmanager = cm.TheCountDownManager;
            tm = cm.TheTickManager;
            AddToCountdown();
            AddToTick();
        }

        protected void AddToTick()
        {
            if (!CheckFeature(TICK)) return;
            Tick t = new Tick(this, TICK, (int)m_features[TICK].CurrValue);
            t.DoTick();
            tm.AddTick(t);
        }

        protected void AddToCountdown()
        {
            if (!CheckFeature(TIME)) return;
            cdmanager.AddCountDown(new CountDown(this, TIME, valuePerSecond));
        }
    }
}

