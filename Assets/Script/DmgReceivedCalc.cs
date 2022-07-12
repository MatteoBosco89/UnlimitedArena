using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class DmgReceivedCalc : MonoBehaviour
    {
        protected Dictionary<string, float> protectBuff = new Dictionary<string, float>();

        public void AddBuff(string name, float buff)
        {
            protectBuff[name] = buff;
        }

        public void RemoveBuff(string name)
        {
            protectBuff[name] = 1;
        }

        public int CalcDamageReceived(int dmg)
        {
            float partialDmg = dmg;
            foreach (KeyValuePair<string, float> entry in protectBuff)
            {
                partialDmg *= entry.Value;
            }
            int finalDmg = Mathf.FloorToInt(partialDmg);
            return finalDmg;
        }
    }
}

