using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class SpeedCalc : MonoBehaviour
    {
        protected Dictionary<string, float> speedBuffs = new Dictionary<string, float>();

        public void AddBuff(string name, float buff)
        {
            speedBuffs[name] = buff;
        }

        public void RemoveBuff(string name)
        {
            speedBuffs[name] = 1;
        }

        public float CalcSpeed(float baseSpeed)
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

