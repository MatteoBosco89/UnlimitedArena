using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Character
{
    public class LifePickup : Consumable
    {
        public override void Pickup(GameObject player)
        {
            base.Pickup(player);
            player.GetComponent<PlayerLifeManager>().PickupLife(gameObject);
        }
    }

}
