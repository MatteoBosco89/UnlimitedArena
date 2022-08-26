

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour {

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    private void Awake() {

        // riferimenti di base
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        // inizialmente viene disattivato il modello 
        entryTemplate.gameObject.SetActive(false);

        //AddHighscoreEntry(5000000, "AUG");

        string jsonString = PlayerPrefs.GetString("highscoreTable");  
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) {
            // There's no stored table, initializ
               AddHighscoreEntry(10, 1000000, "CMK");
               AddHighscoreEntry(7, 897621, "JOE");
               AddHighscoreEntry(5, 872931, "DAV");
               AddHighscoreEntry(12, 785123, "CAT");
               AddHighscoreEntry(22, 542024, "MAX");
               AddHighscoreEntry(4, 68245, "AAA");
            // Reload
            jsonString = PlayerPrefs.GetString("highscoreTable");
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        }

        // Ordinamento della tabella dal punteggio più alto a quello più basso
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++) {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++) {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score) {
                    // Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList) {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }


    // Creazione di una singola Entry 
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList) {
        float templateHeight = 31f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
        default:
            rankString = rank + "TH"; break;

        case 1: rankString = "1ST"; break;
        case 2: rankString = "2ND"; break;
        case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        int score = highscoreEntry.kills;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        // Set background visible odds and evens, easier to read
        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);
        
        // Highlight First
        if (rank == 1) {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        }

        // Set tropy
        switch (rank) {
        default:
            entryTransform.Find("trophy").gameObject.SetActive(false);
            break;
        case 1:
            entryTransform.Find("trophy").GetComponent<Image>().color = new Color(237, 240, 45, 1); 
            break;
        case 2:
            entryTransform.Find("trophy").GetComponent<Image>().color = new Color(150, 150, 134, 1);
            break;
        case 3:
            entryTransform.Find("trophy").GetComponent<Image>().color = new Color(216, 176, 33, 1);
                break;

        }

        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int kills, int score, string name) {
        // Creazione HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { kills = kills, score = score, name = name };
        
        // carico il salavataggio della Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) { 
            highscores = new Highscores() {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }

        // Aggiungo nuova entry in Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Salataggio della Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    
    private class Highscores {
        public List<HighscoreEntry> highscoreEntryList;
    }

    // Viene rappresentata una singola entry 
    [System.Serializable] 
    private class HighscoreEntry {
        public int score;
        public int kills;
        public string name;
    }

}
