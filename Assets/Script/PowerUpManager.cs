using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class PowerUpManager : MonoBehaviour
    {
        [SerializeField] protected List<GameObject> powerups;
        protected Dictionary<string, int> timeRemain;
        protected List<PowerupHandler> powerupList;
        protected List<string> powerupsId;
        protected PowerupHandler powerupHandler;

        private void Awake()
        {
            timeRemain = new Dictionary<string, int>();
            powerupsId = new List<string>();
            powerupList = new List<PowerupHandler>();
        }

        public void ResetPowerUps()
        {
            foreach (GameObject p in powerups)
            {
                string id = GetIdFromGameObject(p);
                timeRemain[id] = 0;
            }
        }

        public int GetTimeRemaining(string id)
        {
            int t = 0;
            try{ t = timeRemain[id]; }
            catch (System.Exception){ timeRemain.Add(id, 0); }
            return t;
        }

        public float GetAura(string id)
        {
            //PowerupHandler p = handlers[id];
            return 0f; //p.Multiplier;
        }

        public List<PowerupHandler> PowerUps
        {
            get { return powerupList; }
        }

        private void Start()
        {
            // powerup initialization
            // setting time remain to zero
            InvokeRepeating(nameof(CountDown), 0.1f, 1);
        }

        private string GetIdFromGameObject(GameObject o)
        {
            powerupHandler = o.GetComponent<PowerupHandler>();
            return powerupHandler.Id;
        }

        private int GetModifierFromGameObject(GameObject o)
        {
            powerupHandler = o.GetComponent<PowerupHandler>();
            return 0; //powerupHandler.ModifierTime;
        }

        public void PowerUpPickup(GameObject powerup)
        {
            powerupList.Add(powerup.GetComponent<PowerupHandler>());
            powerup.GetComponent<PowerupHandler>().Activate();
        }

        protected void CountDown()
        {
            for(int i = 0; i < powerupList.Count; i++)
            {
                powerupList[i].CountDown();
            }
            powerupList.RemoveAll(item => item.IsActive == false);
        }

    }
}

