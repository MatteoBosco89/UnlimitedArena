using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Character
{
    public class DmgDoneCalc : BuffHandler
    {
        public int CalcDmg(float baseDmg)
        {
            LoadList();
            foreach (PowerupHandler ph in powerupList)
            {
                if (ph.DamageDonePowerup._isEnabled) baseDmg *= ph.DamageDonePowerup._multiplier;
            }

            return Mathf.CeilToInt(baseDmg);
        }
    }
}

