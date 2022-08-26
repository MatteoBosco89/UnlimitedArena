using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Character
{
    public class ScoreTableManager : MonoBehaviour
    {
        [SerializeField] protected Transform entriesContainer;
        [SerializeField] protected Transform tableEntry;
        [SerializeField] protected int initialGap = 0;
        [SerializeField] protected int gapValue = 100;
        protected Dictionary<int, TableEntry> tableEntries = new Dictionary<int, TableEntry>();
        protected Dictionary<int, ScoreEntry> scores = new Dictionary<int, ScoreEntry>();
        protected List<ScoreEntry> scoreList = new List<ScoreEntry>();
        protected string scoreTableJSON;
        protected int currentGap;

        public string ScoreTableState
        {
            get { return scoreTableJSON; }
            set { scoreTableJSON = value; }
        }

        public void FixedUpdate()
        {
            UpdateAllEntries();
        }

        public void UpdateAllEntries()
        {
            foreach (KeyValuePair<int, TableEntry> t in tableEntries) t.Value.UpdateMe(scores[t.Key]);
            SaveState();
        }

        public ScoreEntry MyEntry(int playerid)
        {
            try { return scores[playerid]; }
            catch (Exception) { return null; }
        }

        public void AddNewPlayer(string playerName, int playerId)
        {
            string jsonString = PlayerPrefs.GetString("SCORES");
            if (jsonString == "")
            { 
                ScoreEntry s = new ScoreEntry(0, 0, playerId, playerName);
                scores.Add(playerId, s);
                CreateEntry(playerId);
            }
            else
            {
                LoadState(jsonString);
                foreach (ScoreEntry s in scoreList) CreateEntry(s.playerid);
            }
            SaveState();
        }

        public void CreateEntry(int playerId)
        {
            Transform newEntry = Instantiate(tableEntry, entriesContainer);
            TableEntry newTableEntry = newEntry.GetComponent<TableEntry>();
            newTableEntry.PlayerId = playerId;
            newTableEntry.Gap = currentGap;
            currentGap += gapValue;
            newEntry.gameObject.SetActive(true);
            if (tableEntries.ContainsKey(playerId)) tableEntries[playerId] = newTableEntry;
            else tableEntries.Add(playerId, newTableEntry);
        }

        public void UpdateDamage(int id, int dmg)
        {
            try
            {
                scores[id].damage += dmg;
            }
            catch (Exception) { }
        }

        public void UpdateKill(int id)
        {
            try
            {
                scores[id].kills++;
            }
            catch (Exception) { }
        }
        protected void LoadState(string savedScore)
        {
            ScoreEntryList l = JsonUtility.FromJson<ScoreEntryList>(savedScore);
            scoreList = l.list;
            for (int i = 0; i < scoreList.Count; i++)
            {
                int id = scoreList.ElementAt(i).playerid;
                ScoreEntry e = scoreList.ElementAt(i);
                if (scores.ContainsKey(id)) scores[id] = e;
                else scores.Add(id, e);
            }
        }
        public void SaveState()
        {
            scoreList.Clear();
            foreach (KeyValuePair<int, ScoreEntry> k in scores)
            {
                scoreList.Add(k.Value);
            }
            ScoreEntryList l = new ScoreEntryList();
            l.List = scoreList;
            string jsonString = JsonUtility.ToJson(l);
            PlayerPrefs.SetString("SCORES", jsonString);
        }
        [Serializable]
        public class ScoreEntry
        {
            public int damage;
            public int kills;
            public int playerid;
            public string player;

            public ScoreEntry(int d, int k, int id, string p)
            {
                damage = d;
                kills = k;
                playerid = id;
                player = p;
            }
        }
        [Serializable]
        public class ScoreEntryList
        {
            public List<ScoreEntry> list = new List<ScoreEntry>();

            public List<ScoreEntry> List
            {
                get { return list; }
                set { list = value; }
            }
            public ScoreEntryList() { }
        }
    }
}

