using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

namespace Character
{
    public class UAComponent : ObjectWithFeatures
    {
        protected string componentId;
        protected string componentPath;
        protected ComponentManager cm;
        protected bool isActive = true;
        protected bool nextTick = false;
        protected Dictionary<string, Modifier> modifiers = new Dictionary<string, Modifier>();
        protected Dictionary<string, Feature> m_features = new Dictionary<string, Feature>();
        protected Dictionary<string, string> wrapper = new Dictionary<string, string>();


        public string ComponentId
        {
            get { return componentId; }
        }

        public void ReduceComponent(string type, float n)
        {
            try
            {
                if (m_features[type].CurrValue < 0) return;
                m_features[type].CurrValue -= n;
                if (m_features[type].CurrValue < 0) isActive = false;
            }
            catch (Exception) { }
        }

        public void TickFeature()
        {
            nextTick = true;
        }

        public void ResetTick()
        {
            nextTick = false;
        }

        public bool CheckTick()
        {
            return nextTick;
        }

        public bool HasFeature(string feature)
        {
            foreach (string s in m_features.Keys) if (s == feature) return true;
            return false;
        }

        public bool CheckIsActive()
        {
            return isActive;
        }

        public void AddModifier(Modifier m)
        {
            modifiers[m.Name] = m;
        }

        public void AddModifier(string name, string m, float mul, float add)
        {
            AddModifier(new Modifier(name, m, mul, add));
        }

        public Dictionary<string, Modifier> MyModifiers
        {
            get { return modifiers; }
        }

        public Dictionary<string, Feature> MyFeatures
        {
            get { return m_features; }
            set { m_features = value; }
        }

        public UAComponent(string name, string path, ComponentManager comm)
        {
            componentId = name;
            componentPath = path;
            cm = comm;
            LoadMe(path);
        }

        protected virtual void LoadMe(string path)
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string l in lines)
            {
                string[] items = l.Split(',');
                string name = items[0].Trim();
                string feature = items[1].Trim();
                float mul_val = ParseFloatValue(items[2]);
                float add_val = ParseFloatValue(items[3]);
                Modifier m = new Modifier(name, feature, mul_val, add_val);
                Feature f = new Feature(add_val, feature);
                AddWrapper(name, feature);
                AddModifier(m);
                AddFeature(f);
                
            }
        }

        public string ModifierNameByFeature(string feature)
        {
            foreach (KeyValuePair<string, string> w in wrapper) if (w.Value == feature) return w.Key;
            return null;
        }

        protected float ParseFloatValue(string val)
        {
            return float.Parse(val, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }

        public override void AddFeature(Feature f)
        {
            m_features[f.Type] = f;
        }

        public override void AddFeature(string f, float val)
        {
            AddFeature(new Feature(val, f));
        }

        public void AddWrapper(string name, string feature)
        {
            wrapper.Add(name, feature);
        }

        public override Feature GetFeature(string f)
        {
            foreach (KeyValuePair<string, Feature> keyValuePair in m_features)
            {
                Feature feature = m_features[keyValuePair.Key];
                if (feature.Type == f) return feature;
            }
            return null;
        }

        public Modifier GetModifier(string m) {
            foreach (KeyValuePair<string, Modifier> keyValuePair in modifiers)
            {
                Modifier modifier = modifiers[keyValuePair.Key];
                if (modifier.Type == m) return modifier;
            }
            return null;
        }

    }
}

