using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class DmgReceivedCalc : BuffHandler
    {
        public int CalcDamageReceived(int baseDmg)
        {
            float dmg = (float)baseDmg;
            LoadList();
            foreach (PowerupHandler ph in powerupList)
            {
                if (ph.DamageReceivedPowerup._isEnabled) dmg *= ph.DamageReceivedPowerup._multiplier;
            }

            return Mathf.CeilToInt(dmg);
        }
    }
}

