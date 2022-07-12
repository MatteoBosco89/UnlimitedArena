using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class WeaponConsumable : MonoBehaviour
{
    [SerializeField] protected int id;
    [SerializeField] protected int ammo;
    [SerializeField] protected float animation_amp;
    [SerializeField] protected float rotation_speed;
    [SerializeField] protected AudioClip pick_up_sound;
    [SerializeField] protected GameObject weapon;
    [SerializeField] protected Light spot_light;
    [SerializeField] protected float respawn_time;
    private CapsuleCollider capCollider;
    private Vector3 startPosition;
    private AudioSource audioS;
    private bool respawn_flag = false;

    public int Id
    {
        get { return id; }
    }
    public int Ammo
    {
        get { return ammo; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.activeSelf) 
        {
            capCollider = gameObject.GetComponent<CapsuleCollider>();
            capCollider.enabled = true;
            capCollider.isTrigger = true;
            startPosition = transform.position;
            audioS = GetComponent<AudioSource>();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        AnimateConsumable();
        RespownConsumable();
    }

    /*private void OnTriggerEnter(Collider other)
    { 
        GameObject player = other.gameObject;
        if (player.tag == "Player")
        {
            PlaySound();
            WeaponManager weaponManager = player.GetComponent<WeaponManager>();
            Debug.Log("Name: " + gameObject.name);
            weaponManager.PickUpWeapon(gameObject);
            capCollider.enabled = false;
            weapon.SetActive(false);
            spot_light.enabled = false;
            StartCoroutine(RespawnCooldown());
            //Destroy(gameObject, pick_up_sound.length);
        }
    }*/

    private void AnimateConsumable()
    {
        transform.position = startPosition +  new Vector3(0, Mathf.Sin(Time.time) * animation_amp, 0);
        transform.Rotate(new Vector3(0, rotation_speed, 0) * Time.deltaTime);
    }

    private void PlaySound()
    {
        audioS.clip = pick_up_sound;
        audioS.Play();
    }

    private void RespownConsumable()
    {
        if (respawn_flag)
        {
            capCollider.enabled = true;
            weapon.SetActive(true);
            spot_light.enabled = true;
            respawn_flag = false;
        }
    }

    private IEnumerator RespawnCooldown()
    {
        yield return new WaitForSeconds(respawn_time);
        respawn_flag = true;
    }
}
