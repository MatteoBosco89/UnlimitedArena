using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character
{
    public abstract class ObjectWithFeatures
    {
        public abstract void AddFeature(Feature f);

        public abstract void AddFeature(string f, float val);

        public abstract Feature GetFeature(string f);

    }

}

