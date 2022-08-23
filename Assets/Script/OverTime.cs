using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class OverTime
    {
        protected float valuePerSecond;
        protected string parameter;
        protected UAComponent c;

        public UAComponent MyComponent
        {
            get { return c; }
            set { c = value; }
        }

        public OverTime(UAComponent component)
        {
            c = component;
        }

        public virtual void Activate() { }

    }
}

