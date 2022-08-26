using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class EffectManager : MonoBehaviour
{
    [SerializeField] protected string effectsWrapperPath = "EffectWrapper";
    protected Dictionary<string, List<string>> effectsWrapper = new Dictionary<string, List<string>>();

    private void Awake()
    {
        effectsWrapperPath = Path.Combine(Application.streamingAssetsPath, effectsWrapperPath);
        LoadEffectWrapper();
    }

    public List<string> Effects(List<string> components)
    {
        List<string> effects = new List<string>();
        foreach(string s in components)
        {
            try
            {
                foreach (string e in effectsWrapper[s])
                {
                    if(!effects.Contains(e)) effects.Add(e);
                }
                
            }catch(Exception) { }
        }
        return effects;
    }

    protected void AddEffect(string component, string effect)
    {
        if (!effectsWrapper.ContainsKey(component)) effectsWrapper.Add(component, new List<string>());
        effectsWrapper[component].Add(effect);
    }

    protected void LoadEffectWrapper()
    {
        if (!CheckDirectory(effectsWrapperPath)) return;
        string[] fileEntries = Directory.GetFiles(effectsWrapperPath, "*.csv");
        foreach (string fileName in fileEntries)
        {
            LoadSingleFile(fileName);
        }
    }

    protected void LoadSingleFile(string fileName)
    {
        if (!CheckFile(fileName)) return;
        string[] lines = File.ReadAllLines(fileName);
        foreach (string l in lines)
        {
            string[] items = l.Split(',');
            string component = items[0].Trim();
            string effect = items[1].Trim();
            AddEffect(component, effect);
        }
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

}
