using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Character
{
    public class DmgDoneCalc : BuffHandler
    {
        public int CalcDmg(float baseDmg)
        {
            return Mathf.CeilToInt(CalcBuff(baseDmg));
        }
    }
}

