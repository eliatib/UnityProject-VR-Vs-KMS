using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Project.Entities;

public class LoadElementsBehaviour : MonoBehaviour
{
    public GameObject[] elements;
    public GameObject[] pinPointTypes;
    public LoadManager lm;

    [Header("Create or Join room")]
    public Transform playersParent;

    //Method call after Awake so after the instantiation of the players
    private void Awake()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            lm.Load();
            lm.data.pins.ForEach(PlacePoint);
        }
    }

    private void PlacePoint(PinData data)
    {
        if (data.pinType.Equals("ContaminationArea"))
        {
            PhotonNetwork.Instantiate("Prefabs/" + elements[0].name, data.position, elements[0].transform.rotation).transform.SetParent(pinPointTypes[0].transform);
        }
        else if (data.pinType.Equals("ThrowableObject"))
        {
            PhotonNetwork.Instantiate("Prefabs/"+elements[1].name, data.position, elements[1].transform.rotation).transform.SetParent(pinPointTypes[1].transform);
        }
        else if (data.pinType.Equals("SpawnPoint"))
        {
            PhotonNetwork.Instantiate("Prefabs/" + elements[2].name, data.position, elements[2].transform.rotation).transform.SetParent(pinPointTypes[2].transform);
        }
    }
}
