using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class CountDown : OverTime
    {
        public CountDown(UAComponent component, string param, int val) : base(component) 
        {
            parameter = param;
            valuePerSecond = val;
        }

        public override void Activate()
        {
            c.ReduceComponent(parameter, valuePerSecond);
        }

        public void DoCountdown()
        {
            Activate();
        }
    }
}

