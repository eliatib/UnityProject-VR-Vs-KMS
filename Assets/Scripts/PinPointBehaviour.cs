using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PinPointBehaviour : MonoBehaviour
{
    public ToggleGroup pinChoice;
    public GameObject[] pinPointPrefab;
    public GameObject[] pinPointTypes;
    public Text[] number;
    public DataManager dataManager;

    [HideInInspector]
    public Toggle selectedToggle;
    [HideInInspector]
    public MapData map;
    public List<GameObject> pinsList;

    private bool FirstOccurence;
    private int index;
    

    private void Update()
    {
        //Update Which PinPoint Type is Selected
        selectedToggle = pinChoice.ActiveToggles().FirstOrDefault();
        UpdateNumber();
    }

    private void OnMouseDown()
    {
        //When click on screen, send a raycast from the camera to map model and add selected PinPoint on the map at the place hit

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            PinData pinData = new PinData();
            GameObject pin = new GameObject();
            string removeType = null;

            pinData.position = hit.point;
            if (selectedToggle.name.Equals("ContaminationArea"))
            {
                if (pinPointTypes[0].transform.childCount > 2)
                {
                    FirstOccurence = true;
                    Destroy(pinPointTypes[0].transform.GetChild(0).gameObject);
                    removeType = "ContaminationArea";
                }
                pin = Instantiate(pinPointPrefab[0], hit.point, transform.rotation, pinPointTypes[0].transform);
                pinData.pinType = "ContaminationArea";
            }
            else if (selectedToggle.name.Equals("ThrowableObject"))
            {
                if (pinPointTypes[1].transform.childCount > 2)
                {
                    FirstOccurence = true;
                    Destroy(pinPointTypes[1].transform.GetChild(0).gameObject);
                    removeType = "ThrowableObject";
                }
                pin = Instantiate(pinPointPrefab[1], hit.point, transform.rotation, pinPointTypes[1].transform);
                pinData.pinType = "ThrowableObject";
            }
            else if (selectedToggle.name.Equals("SpawnPoint"))
            {
                if (pinPointTypes[2].transform.childCount > 3)
                {
                    FirstOccurence = true;
                    Destroy(pinPointTypes[2].transform.GetChild(0).gameObject);
                    removeType = "SpawnPoint";
                }
                pin = Instantiate(pinPointPrefab[2], hit.point, transform.rotation, pinPointTypes[2].transform);
                
                pinData.pinType = "SpawnPoint";
            }
            map.pins.Add(pinData);
            pinsList.Add(pin);
            if (removeType != null)
            {
                map.pins.ForEach(e => RemoveOldPoint(e, removeType));
                map.pins.RemoveAt(index);
            }
            dataManager.data = map;
        }
    }

    private void UpdateNumber()
    {
        number[0].text = pinPointTypes[0].transform.childCount + "/3";
        number[1].text = pinPointTypes[1].transform.childCount + "/3";
        number[2].text = pinPointTypes[2].transform.childCount + "/4";
    }

    //Load Json File PinPoint when call
    public void LoadPoint()
    {
        map = dataManager.data;
        foreach(GameObject pinType in pinPointTypes) 
        {
            foreach (Transform child in pinType.transform)
            {
                Destroy(child.gameObject);
            }
        }
        dataManager.Load();
        dataManager.data.pins.ForEach(PlacePoint);
    }

    public void RemoveAll() 
    {
        if (pinsList != null)
        {
            foreach (GameObject pin in pinsList)
            {
                Destroy(pin.gameObject);
            }
            pinsList.Clear();
        }
    }

    public void RemoveLast()
    {
        if (pinsList != null) 
        {
            GameObject lastPin = pinsList[pinsList.Count - 1];
            Destroy(lastPin.gameObject);
            pinsList.Remove(lastPin);
        }
    }

    //Remove Old point from Json file PinPoint List
    private void RemoveOldPoint(PinData data,string type)
    {
        if (data.pinType.Equals(type) && FirstOccurence == true) 
        {
            index = dataManager.data.pins.FindIndex(a => a == data);
            FirstOccurence = false;
        }
    }

    //Place PinPoint Sprites on the scene
    private void PlacePoint(PinData data) 
    {
        GameObject pin = new GameObject();
        if (data.pinType.Equals("ContaminationArea")) 
        {
            pin = Instantiate(pinPointPrefab[0], data.position, transform.rotation, pinPointTypes[0].transform);
        }
        if (data.pinType.Equals("ThrowableObject"))
        {
            pin = Instantiate(pinPointPrefab[1], data.position, transform.rotation, pinPointTypes[1].transform);
        }
        if (data.pinType.Equals("SpawnPoint"))
        {
            pin = Instantiate(pinPointPrefab[2], data.position, transform.rotation, pinPointTypes[2].transform);
        }
        pinsList.Add(pin);
    }

    public void BackToMenu()
    {
        PhotonNetwork.LoadLevel("MainMenu");
    }
}
