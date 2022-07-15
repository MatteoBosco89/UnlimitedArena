using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class DmgReceivedCalc : BuffHandler
    {
        public int CalcDamageReceived(int dmg)
        {
            return Mathf.FloorToInt(CalcBuff(dmg));
        }
    }
}

