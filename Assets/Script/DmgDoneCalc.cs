using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Character
{
    public class DmgDoneCalc : BuffHandler
    {
        public float CalcDmg(float baseDmg)
        {
            return CalcBuff(baseDmg);
        }
    }
}

