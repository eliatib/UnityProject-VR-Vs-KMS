using Photon.Pun;
using Project.Entities;
using Project.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpawnManager : Singleton<SpawnManager>
{ 
    public GameObject spawnPoints;

    private float timer = 2f;
    private Random rand = new Random();

    public void Respawn(GameObject entity)
    {
        Transform[] childs;
        childs = spawnPoints.GetComponentsInChildren<Transform>();
        Transform spawnpoint = childs[rand.Next(childs.Length)];
        if (spawnpoint.position.y < 0)
        {
            spawnpoint.position = new Vector3(spawnpoint.position.x, 0, spawnpoint.position.z);
        }
        entity.transform.position = spawnpoint.position;
        //Hide player and wait spawn time (2s)
        entity.SetActive(false);
        StartCoroutine(spawnTime(entity));
        //AudioManager.Instance.Play("Respawn");
    }

    private IEnumerator spawnTime(GameObject entity) 
    {
       
        yield return new WaitForSeconds(timer);
        entity.SetActive(true);
        
        EntityBase e = entity.GetComponent<EntityBase>();
        if(e != null)
        {
            e.Respawn();
            if (e is Virus)
            {    
                if (!e.GetComponent<PhotonView>().IsMine) 
                {
                    Debug.Log("I am the tps and virus die");
                    entity.SetActive(false);
                    //entity.transform.parent.GetChild(1).gameObject.SetActive(false);
                }
                else if (e.GetComponent<PhotonView>().IsMine)
                {
                   // entity.SetActive(false);
                   // entity.transform.parent.GetChild(1).gameObject.SetActive(false);
                }
            }
        }

    }
}

