using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class SpeedCalc : BuffHandler
    {
        public float CalcSpeed(float baseSpeed)
        {
            return CalcBuff(baseSpeed);
        }

    }
}

