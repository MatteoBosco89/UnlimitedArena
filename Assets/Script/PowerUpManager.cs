using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class PowerUpManager : MonoBehaviour
    {
        [SerializeField] protected List<GameObject> powerups;
        protected string countdown = "CountDown";
        protected Dictionary<string, int> timeRemain;
        protected Dictionary<string, PowerupHandler> handlers;
        protected List<string> powerupsId;
        protected PowerupHandler powerupHandler;

        private void Awake()
        {
            timeRemain = new Dictionary<string, int>();
            handlers = new Dictionary<string, PowerupHandler>();
            powerupsId = new List<string>();
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
            PowerupHandler p = handlers[id];
            return p.Multiplier;
        }

        private void Start()
        {
            foreach(GameObject p in powerups)
            {
                string id = GetIdFromGameObject(p);
                timeRemain[id] = 0;
                powerupsId.Add(id);
                handlers[id] = p.GetComponent<PowerupHandler>();
            }
            //InvokeRepeating(countdown, 0.1f, 1);
        }

        private string GetIdFromGameObject(GameObject o)
        {
            powerupHandler = o.GetComponent<PowerupHandler>();
            return powerupHandler.Id;
        }

        private int GetModifierFromGameObject(GameObject o)
        {
            powerupHandler = o.GetComponent<PowerupHandler>();
            return powerupHandler.ModifierTime;
        }

        public void PowerUpPickup(GameObject gameObject)
        {
            timeRemain[GetIdFromGameObject(gameObject)] = GetModifierFromGameObject(gameObject);
        }

        protected void CountDown()
        {
            foreach (string p in powerupsId)
            {
                if (GetTimeRemaining(p) > 0) timeRemain[p] -= 1;
            }
        }

    }
}

