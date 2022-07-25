using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Character
{
    public class BuffHandler : MonoBehaviour
    {
        protected Dictionary<string, float> buffs = new Dictionary<string, float>();
        protected List<PowerupHandler> powerupList;

        protected void LoadList()
        {
            powerupList = GetComponent<PowerUpManager>().PowerUps;
        }

        public void AddBuff(string name, float buff)
        {
            buffs[name] = buff;
        }

        public void RemoveBuff(string name)
        {
            buffs[name] = 1;
        }

        public float CalcBuff(float b)
        {
            return 0.0f;
        }

    }
}

