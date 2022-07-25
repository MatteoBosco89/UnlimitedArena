using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class PowerupHandler : MonoBehaviour
    {
        [SerializeField] protected string _id;
        [SerializeField] protected int _modifierTime;
        [SerializeField] protected MovementCustomizedPowerup _movementPowerup;
        [SerializeField] protected DamageDoneCustomizedPowerup _damageDonePowerup;
        [SerializeField] protected DamageReceivedCustomizedPowerup _damageReceivedPowerup;
        private int _currentTime;
        private bool _isActive;

        public string Id
        {
            get { return _id; }
        }

        public MovementCustomizedPowerup MovementPowerup
        {
            get { return _movementPowerup; }
        }

        public DamageDoneCustomizedPowerup DamageDonePowerup
        {
            get { return _damageDonePowerup; }
        }

        public DamageReceivedCustomizedPowerup DamageReceivedPowerup
        {
            get { return _damageReceivedPowerup; }
        }
        public int GetCurrentTime()
        {
            return _currentTime;
        }
        public void CountDown()
        {
            if (!_isActive) return;
            _currentTime -= 1;
            if (_currentTime <= 0)
            {
                _isActive = false;
                _currentTime = 0;
            }
        }
        public void Activate()
        {
            _isActive = true;
            _currentTime = _modifierTime;
        }
        public bool IsActive
        {
            get { return _isActive; }
        }

        public class CustomizedPowerup
        {
            public bool _isEnabled;
            public float _multiplier;
        }

        [System.Serializable]
        public class MovementCustomizedPowerup : CustomizedPowerup { }
        [System.Serializable]
        public class DamageDoneCustomizedPowerup : CustomizedPowerup { }
        [System.Serializable]
        public class DamageReceivedCustomizedPowerup : CustomizedPowerup { }

    }
}
