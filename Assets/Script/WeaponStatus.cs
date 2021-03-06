using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Weapon
{

    public class WeaponStatus : MonoBehaviour
    {
        [SerializeField] protected int id;
        [SerializeField] protected float damage;
        [SerializeField] protected bool has_infinite_ammo;
        [SerializeField] protected int ammo;
        [SerializeField] protected int max_ammo;
        [SerializeField] protected int ammo_per_shot;
        [SerializeField] protected float range;
        [SerializeField] protected float time_between_shot;
        [SerializeField] protected bool is_single_fire;
        [SerializeField] protected AudioClip fire_sound;
        [SerializeField] protected Vector3 position;
        [SerializeField] protected Vector3 rotation;
        [SerializeField] protected string weaponType;
        [SerializeField] protected string weaponName;
        
        public void ResetAmmo()
        {
            ammo = 0;
        }

        public string WeaponName
        {
            get { return weaponName; }
        }

        public string WeaponType
        {
            get { return weaponType; }
        }

        public Vector3 Position
        {
            get { return position; }
        }

        public Vector3 Rotation
        {
            get { return rotation; }
        }

        public AudioClip Fire_sound
        {
            get { return fire_sound; }
        }
        public int Id
        {
            get { return id; }
        }
        public float Damage
        {
            get { return damage; }
        }
        public bool Has_infinite_ammo
        {
            get { return has_infinite_ammo; }
        }
        public int Ammo
        {
            get { return ammo; }
        }
        public bool Is_single_fire
        {
            get { return is_single_fire; }
        }
        public int Max_ammo
        {
            get { return max_ammo; }
        }
        public int Ammo_per_shot
        {
            get { return ammo_per_shot; }
        }
        public float Time_between_shot
        {
            get { return time_between_shot; }
        }
        public float Range
        {
            get { return range; }
        }

        void Start()
        {
            if (ammo > max_ammo)
            {
                ammo = max_ammo;
            }

        }

        public void AddAmmo(int value)
        {
            if ((ammo + value) < max_ammo)
            {
                ammo += value;
            }
            else
            {
                ammo = max_ammo;
            }

        }
        public void Shoot()
        {
            if (!has_infinite_ammo)
            {
                if (ammo > ammo_per_shot)
                {
                    ammo -= ammo_per_shot;
                }
                else
                {
                    ammo = 0;
                }
            }
        }
    }
}
