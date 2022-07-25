using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class SpeedCalc : BuffHandler
    {
        public float CalcSpeed(float baseSpeed)
        {
            LoadList();
            foreach (PowerupHandler ph in powerupList)
            {
                if (ph.MovementPowerup._isEnabled) baseSpeed *= ph.MovementPowerup._multiplier;
            }
            return baseSpeed;
        }

    }
}

