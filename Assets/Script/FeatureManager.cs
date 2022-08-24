using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using UnityEngine;
using Unity.IO;

namespace Character
{
    public class FeatureManager : MonoBehaviour
    {
        [SerializeField] protected string featuresDirectory; 
        protected Dictionary<string, Feature> features = new Dictionary<string, Feature>();
        protected Dictionary<string, Feature> baseFeatures = new Dictionary<string, Feature>();
        protected ComponentManager componentManager;

        public string FeaturesDirectory
        {
            get { return featuresDirectory; }
            set { featuresDirectory = value; }
        }

        public Dictionary<string, Feature> Features
        {
            get { return features; }
            set { features = value; }
        }

        public Dictionary<string, Feature> BaseFeatures
        {
            get { return baseFeatures; }
        }

        private void Awake()
        {
            featuresDirectory = Path.Combine(Application.streamingAssetsPath, featuresDirectory);
            componentManager = GetComponent<ComponentManager>();
            LoadFeatures();
            componentManager.ObjectFeatures = features;
        }

        protected void LoadFeatures()
        { 
            if (!Directory.Exists(featuresDirectory))
            {
                Debug.LogError("Features Directory Not Found");
                return;
            }
            string[] fileEntries = Directory.GetFiles(featuresDirectory, "*.csv");
            foreach (string fileName in fileEntries)
            {
                string[] lines = File.ReadAllLines(fileName);
                foreach (string l in lines)
                {
                    string[] items = l.Split(',');
                    string type = items[0].Trim();
                    float b_value = ParseFloatValue(items[1]);
                    Feature f = new Feature(b_value, type);
                    AddFeature(f);
                    AddBaseFeature(f);
                }
            }
        }

        protected float ParseFloatValue(string val)
        {
            return float.Parse(val, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
        }

        public float FeatureValue(string feature)
        {
            feature = feature.ToUpper();
            try { 
                return features[feature].CurrValue; 
            } catch (Exception) { 
                Feature f = new Feature(feature); AddFeature(f); return f.CurrValue; 
            }
        }

        public float TickValue(string feature)
        {
            componentManager.AddTickable(feature);
            feature = feature.ToUpper();
            float currValue;
            try {
                currValue = features[feature].CurrValue;
                features[feature].CurrValue = 0;
            }
            catch (Exception) {
                Feature f = new Feature(feature); AddFeature(f); currValue = f.CurrValue;
            }
            return currValue;
        }

        public Feature GetSingleFeature(string type)
        {
            return features[type];
        }

        public void AddFeature(Feature f)
        {
            features.Add(f.Type, f);
        }

        public void AddBaseFeature(Feature f)
        {
            baseFeatures.Add(f.Type, f);
        }

        public void AddFeature(string type, float val)
        {
            AddFeature(new Feature(val, type));
        }

        public void RemoveFeature(Feature f)
        {
            features.Remove(f.Type);
        }

        public void RemoveFeature(string f)
        {
            features.Remove(f);
        }

    }
}

