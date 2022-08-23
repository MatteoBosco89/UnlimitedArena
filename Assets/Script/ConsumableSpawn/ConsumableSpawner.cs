using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConsumableSpawner : NetworkBehaviour
{
    [SerializeField] protected GameObject objectToSpawn;
    [SerializeField] protected float cooldown = 30.0f;
    protected bool isCooldown = false;
    protected float startCooldown = 0;
    protected GameObject spawnedObj;

    public void SpawnConsumable()
    {
        if (isServer)
        {
            spawnedObj = Instantiate(objectToSpawn, transform.position, transform.rotation, transform);
            CmdSpawnConsumable();
        }
    }

    [Command]
    void CmdSpawnConsumable()
    {
        NetworkServer.Spawn(spawnedObj);
    }

    public GameObject SpawnedObject
    {
        get { return spawnedObj; }
    }

    private void FixedUpdate()
    {
        if (isCooldown)
        {
            if (Time.time - startCooldown >= cooldown)
            {
                isCooldown = false;
                spawnedObj.SetActive(true);
            }
        }
    }

    public void SetCooldown()
    {
        spawnedObj.SetActive(false);
        isCooldown = true;
        startCooldown = Time.time;
    }

    public void PickedUp()
    {

    }

}
