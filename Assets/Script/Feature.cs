using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Feature
    {
        protected float base_value = 0;
        protected float curr_value = 0;
        protected string type;

        public float BaseValue
        {
            get { return base_value; }
        }
        public float CurrValue
        {
            get { return curr_value; }
            set { curr_value = value; }
        }
        public string Type
        {
            get { return type; }
        }

        public Feature(float b_value, string t_value)
        {
            curr_value = b_value;
            base_value = b_value;
            type = t_value;
        }

        public Feature(float b_value, float c_value, string t_value)
        {
            curr_value = c_value;
            base_value = b_value;
            type = t_value;
        }

        public Feature(string t_value)
        {
            type = t_value;
        }
    }
}

