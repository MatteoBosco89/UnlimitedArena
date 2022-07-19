using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class NameSelection : MonoBehaviour
    {

        [SerializeField] protected InputField playerName;
        protected SceneChangeManager scm;

        void Awake()
        {
            scm = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<SceneChangeManager>();
        }

        void Start()
        {
            playerName.onValueChanged.AddListener(delegate { RemoveSpaces(); });
        }

        void RemoveSpaces()
        {
            playerName.text = playerName.text.Replace(" ", "");
        }

        public void ConnectToGame()
        {
            if (playerName.text.Equals("")) playerName.text = "GuestPlayer" + Time.time.ToString();
            PlayerPrefs.SetString("Name", playerName.text);
            scm.LoadingScreen("UnlimitedArena_Menu");
        }

    }
}

