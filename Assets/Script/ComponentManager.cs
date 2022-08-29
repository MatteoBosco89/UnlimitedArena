using System.Collections;
using System;
using Unity.IO;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character
{
    public class ComponentManager : MonoBehaviour
    {
        [SerializeField] protected string componentDirectory;
        protected Dictionary<string, UAComponent> components = new Dictionary<string, UAComponent>();
        protected Dictionary<string, UAComponent> baseComponents = new Dictionary<string, UAComponent>();
        protected Dictionary<string, Feature> objectFeatures = null;
        protected Dictionary<string, float> featureMulMod = new Dictionary<string, float>();
        protected Dictionary<string, float> featureAddMod = new Dictionary<string, float>();
        protected Dictionary<string, Modifier> allTicks = new Dictionary<string, Modifier>();
        protected List<string> tickable = new List<string>();
        protected CountDownManager countDownManager;
        protected TickManager tickManager;
        protected FeatureManager featureManager;

        public Dictionary<string, UAComponent> Components
        {
            get { return components; }
        }

        public Dictionary<string, Feature> ObjectFeatures
        {
            get { return objectFeatures; }
            set { objectFeatures = value; }
        }

        public TickManager TheTickManager
        {
            get { return tickManager; }
        }

        public CountDownManager TheCountDownManager
        {
            get { return countDownManager; }
        }

        private void Awake()
        {
            componentDirectory = Path.Combine(Application.streamingAssetsPath, componentDirectory);
            countDownManager = GetComponent<CountDownManager>();
            tickManager = GetComponent<TickManager>();
            featureManager = GetComponent<FeatureManager>();
            LoadComponentGroup(componentDirectory);
        }

        protected void ResetModifiers()
        {
            foreach(Feature f in objectFeatures.Values)
            {
                featureMulMod[f.Type] = 1.0f;
                featureAddMod[f.Type] = 0.0f;
            }
        }

        protected void ComputeModifiers()
        {
            foreach (KeyValuePair<string, UAComponent> keyValue in components)
            {
                Dictionary<string, Modifier> mod = components[keyValue.Key].MyModifiers;
                foreach (Modifier m in mod.Values)
                {
                    try
                    {
                        featureMulMod[m.Type] *= m.MultFactor;
                        featureAddMod[m.Type] += m.AddFactor;
                    }
                    catch (Exception) { }                    
                }
            }
        }

        protected void ComputeFeatures()
        {
            foreach(Feature f in objectFeatures.Values)
            {
                if (!tickable.Contains(f.Type))
                {
                    float midVal = f.BaseValue * featureMulMod[f.Type];
                    f.CurrValue = midVal + featureAddMod[f.Type];
                }
            }
        }

        protected void CheckIsActive()
        {
            for(int i = 0; i <= components.Count; i++)
            {
                try
                {
                    string s = components.ElementAt(i).Key;
                    if (!components[s].CheckIsActive()) RemoveComponent(s);
                }
                catch (Exception) { }
            }
        }

        protected void ComputeAllTicks()
        {
            foreach (UAComponent c in components.Values)
            {
                if (c.CheckTick())
                {
                    foreach (Modifier m in c.MyModifiers.Values)
                    {
                        if (allTicks.ContainsKey(m.Name)) allTicks.Remove(m.Name);
                        allTicks.Add(m.Name, m);
                    }
                    c.ResetTick();
                }
            }
        }

        private void FixedUpdate()
        {
            if (objectFeatures == null) return;
            CheckIsActive();
            ResetModifiers();
            ComputeModifiers();
            ComputeFeatures();
            ComputeAllTicks();
        }

        protected void LoadSingleComponent(string path)
        {
            if (!CheckFile(path)) return;
            string[] n = path.Split('.');
            UAComponent c = new UAComponent(Path.GetFileName(n[0].Trim()), path, this);
            AddBaseComponent(c);
            AddComponent(c);
        }

        protected void LoadComponentGroup(string path)
        {
            if (!CheckDirectory(path)) return;
            string[] fileEntries = Directory.GetFiles(path, "*.csv");
            foreach (string fileName in fileEntries)
            {
                LoadSingleComponent(fileName);
            }
        }

        public UAComponent SingleComponent(string component)
        {
            component = component.ToUpper();
            return components[component];
        }

        public void AddBaseComponent(UAComponent c)
        {
            if (baseComponents.ContainsKey(c.ComponentId)) RemoveComponent(c.ComponentId);
            baseComponents.Add(c.ComponentId, c);
        }

        public void AddComponent(UAComponent c)
        {
            if (components.ContainsKey(c.ComponentId)) RemoveComponent(c.ComponentId);
            components.Add(c.ComponentId, c);
        }

        public void RemoveComponent(UAComponent c)
        {
            RemoveComponent(c.ComponentId);
        }

        public void RemoveComponent(string c)
        {
            try
            {
                components.Remove(c);
            }
            catch (Exception) { }
        }

        protected bool CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.LogError("Directory Not Found");
                return false;
            }
            return true;
        }

        protected bool CheckFile(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("File Not Found");
                return false;
            }
            return true;
        }

        // for debug
        public void Print()
        {
            foreach(KeyValuePair<string, UAComponent> keyValuePair in components)
            {
                UAComponent c = components[keyValuePair.Key];
                Debug.LogError(c.ComponentId);
                string features = "";
                foreach (KeyValuePair<string, Feature> f in c.MyFeatures) features += c.MyFeatures[f.Key].Type + " BV: " + c.MyFeatures[f.Key].BaseValue + " CV: " + c.MyFeatures[f.Key].CurrValue;
                //string modifiers = "";
                //foreach (KeyValuePair<string, Modifier> m in c.MyModifiers) modifiers += c.MyModifiers[m.Key].Type + " MF: " + c.MyModifiers[m.Key].MultFactor + " AF: " + c.MyModifiers[m.Key].AddFactor;
                Debug.Log("Features: " + features);
                //Debug.Log("Modifiers: " + modifiers);
            }
        }

        public void PrintFeatures()
        {
            foreach (Feature f in objectFeatures.Values) Debug.Log(f.Type + " " + f.CurrValue);
        }

        public Dictionary<string, UAComponent> ComponentsByFeature(string feature)
        {
            return components.Where(x => x.Value.HasFeature(feature)).ToDictionary(x => x.Key, x=> x.Value);
        }

        public void AddTickable(string feature)
        {
            try
            {
                tickable.Add(feature);
            }
            catch (Exception) { }
        }

        public void ComponentPickup(string type, string name, string path)
        {
            path = Path.Combine(Application.streamingAssetsPath, path);
            type = type.ToLower();
            type = char.ToUpper(type[0]) + type.Substring(1);
            Type t = Type.GetType("Character." + type);
            AddComponent((UAComponent)Activator.CreateInstance(t, name, path, this));
        }

        public Dictionary<string, float> GetAllTicks(string type)
        {
            Dictionary<string, float> filtered = allTicks.Where(x => x.Value.Type == type).ToDictionary(x => x.Key, x => x.Value.AddFactor);
            foreach(string k in filtered.Keys)
            {
                allTicks.Remove(k);
            }
            return filtered;
        }

        public float FeatureValue(string feature)
        {
            float res = 0;
            try
            {
                res = objectFeatures[feature].CurrValue;
            }
            catch (Exception) { }
            return res;
        }

        public Dictionary<string, T> FilterByType<T>()
        {
            Dictionary<string, T> filtered = new Dictionary<string, T>();
            foreach(KeyValuePair<string, UAComponent> c in components)
            {
                if (c.Value is T t) filtered.Add(c.Key, t);
            }
            return filtered;
        }

        public bool IsFeaturePresent(string feature)
        {
            return objectFeatures.ContainsKey(feature);
        }

        public void AddComponents(Dictionary<string, UAComponent> com)
        {
            foreach (UAComponent c in com.Values) AddComponent(c);
        }

        public void UpdateComponents(Dictionary<string, UAComponent> com)
        {
            components.Clear();
            foreach (UAComponent c in com.Values) AddComponent(c);
            foreach (UAComponent c in baseComponents.Values) AddComponent(c);
        }

    }
}

