using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Powerup : UAComponent
    {
        protected CountDownManager cdmanager;
        protected TickManager tm;
        protected string TIME = "TIME";
        protected string TICK = "TICK";
        protected int valuePerSecond = 1;

        public Powerup(string name, string path, CountDownManager countDownManager, TickManager tim) : base(name, path){
            cdmanager = countDownManager;
            tm = tim;
            AddToCountdown();
            AddToTick();
        }

        protected void AddToTick()
        {
            Tick t = new Tick(this, TICK, (int)m_features[TICK].CurrValue);
            t.DoTick();
            tm.AddTick(t);
            
        }

        protected void AddToCountdown()
        {
            cdmanager.AddCountDown(new CountDown(this, TIME, valuePerSecond));
        }
        
    }
}

