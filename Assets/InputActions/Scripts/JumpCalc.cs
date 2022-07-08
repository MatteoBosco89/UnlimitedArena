using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class JumpCalc : MonoBehaviour
    {
        protected Dictionary<string, float> jumpBuffs = new Dictionary<string, float>();

        public void addBuff(string name, float buff)
        {
            jumpBuffs[name] = buff;
        }

        public float calcJump(float baseJump)
        {
            float jump = baseJump;
            foreach (KeyValuePair<string, float> entry in jumpBuffs)
            {
                jump *= entry.Value;
            }
            return jump;
        }
    }
}

