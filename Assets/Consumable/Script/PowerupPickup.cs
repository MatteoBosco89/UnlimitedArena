using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class PowerupPickup : Consumable
    {
        public override void Pickup(GameObject player)
        {
            base.Pickup(player);
            player.GetComponent<PowerUpManager>().PowerUpPickup(gameObject);
        }
    }
}

