using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class SpeedCalc : MonoBehaviour
    {
        protected Dictionary<string, float> speedBuffs = new Dictionary<string, float>();

        public void addBuff(string name, float buff)
        {
            speedBuffs[name] = buff;
        }

        public float calcSpeed(float baseSpeed)
        {
            float speed = baseSpeed;
            foreach (KeyValuePair<string, float> entry in speedBuffs)
            {
                speed *= entry.Value;
            }
            return speed;
        }

    }
}

