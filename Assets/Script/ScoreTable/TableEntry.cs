using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class TableEntry : MonoBehaviour
    {
        [SerializeField] public Transform tableEntry;
        [SerializeField] public Text player;
        [SerializeField] public Text kills;
        [SerializeField] public Text damage;
        protected int playerId;
        protected int gap;

        public int PlayerId
        {
            get { return playerId; }
            set { playerId = value; }
        }

        public int Gap
        {
            get { return gap; }
            set { gap = value; UpdateGap(); }
        }

        private void Start()
        {
            kills.text = "0";
            damage.text = "0";
        }

        protected void UpdateGap()
        {
            float newY = transform.position.y - gap;
            transform.position = new Vector2(transform.position.x, newY);
        }

        public void UpdateMe(ScoreTableManager.ScoreEntry entry)
        {
            UpdateName(entry.player);
            UpdateKills(entry.kills);
            UpdateDamage(entry.damage);
        }

        public void UpdateName(string p)
        {
            player.text = p;
        }

        public void UpdateKills(int k)
        {
            kills.text = k.ToString();
        }

        public void UpdateDamage(int d)
        {
            damage.text = d.ToString();
        }

    }
}

