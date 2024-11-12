using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;
using Random = System.Random;
using System.Collections.Generic;
using UnityStandardAssets.Cameras;

namespace WS3
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {

        public static NetworkManager Instance;

        [Tooltip("The prefab to use for representing the user on a PC. Must be in Resources folder")]
        public GameObject playerPrefabPC;

        [Tooltip("The prefab to use for representing the user in VR. Must be in Resources folder")]
        public GameObject playerPrefabVR;

        public GameObject spawnPoints;
        public Transform playersParent;

        private Random rand = new Random();

        private bool JoinPlayerHasBeenInstantiate = false;
        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. 
        /// </summary>
        public override void OnLeftRoom()
        {
            // TODO: load the Lobby Scene

        }

        /// <summary>
        /// Called when Other Player enters the room and Only other players
        /// </summary>
        /// <param name="other"></param>
        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
            // TODO: 

        }

        /// <summary>
        /// Called when Other Player leaves the room and Only other players
        /// </summary>
        /// <param name="other"></param>
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
            // TODO: 
        }
        #endregion


        #region Public Methods

        /// <summary>
        /// Our own function to implement for leaving the Room
        /// </summary>
        public void LeaveRoom()
        {
            // TODO: 
            PhotonNetwork.LeaveRoom();
        }

        private void updatePlayerNumberUI()
        {
            // TODO: Update the playerNumberUI

        }

        void Start()
        {
            Instance = this;
            Debug.Log("tryVR");
            bool tryVR = PhotonNetwork.PhotonServerSettings.AppSettings.VRConnection;
            Debug.Log(tryVR);
            FindObjectOfType<AudioManager>().Play("ThemeGame");

            if (playerPrefabPC == null || playerPrefabVR == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefabPC or playerPefabVR Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else if(PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("We are Instanciating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                GameObject prefabused;


                if (tryVR)
                    prefabused = UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(playerPrefabPC, playerPrefabVR);
                else
                    prefabused = playerPrefabPC;

                List<Transform> childs = new List<Transform>();
                for(int i = 0; i < spawnPoints.transform.childCount; i++)
                {
                    childs.Add(spawnPoints.transform.GetChild(i).transform);
                }
                //childs = spawnPoints.GetComponentsInChildren<Transform>();
               
                Transform spawnpoint = childs[rand.Next(childs.Count)];


               
                PhotonNetwork.Instantiate("Prefabs/" + prefabused.name, spawnpoint.position, Quaternion.identity).transform.SetParent(this.playersParent);
 
            }
        }

        private void Update()
        {

            if ( !PhotonNetwork.IsMasterClient && !JoinPlayerHasBeenInstantiate)
            {
                GameObject prefabused;
                bool tryVR = PhotonNetwork.PhotonServerSettings.AppSettings.VRConnection;


                if (tryVR)
                    prefabused = UserDeviceManager.GetPrefabToSpawnWithDeviceUsed(playerPrefabPC, playerPrefabVR);
                else
                    prefabused = playerPrefabPC;
                GameObject[] childs;
                childs = GameObject.FindGameObjectsWithTag("SpawnArea");

                if (childs.Length > 0)
                {
                    JoinPlayerHasBeenInstantiate = true;
                }
                Transform spawnpoint = childs[rand.Next(childs.Length)].transform;

                if (spawnpoint.position.y < 0)
                {
                    spawnpoint.position = new Vector3(spawnpoint.position.x, 0, spawnpoint.position.z);
                }


                GameObject newPlayer = PhotonNetwork.Instantiate("Prefabs/" + prefabused.name, spawnpoint.position, Quaternion.identity);
                newPlayer.transform.SetParent(this.playersParent);

                if (!tryVR)
                {
                    GameObject.Find("/FreeLookCameraRig").GetComponent<FreeLookCam>().SetTarget(newPlayer.transform);
                }

            }

            // Code to leave the room by pressing CTRL + the Leave button
            if (Input.GetButtonUp("Leave") && Input.GetKeyDown(KeyCode.LeftControl | KeyCode.RightControl))
            {
                Debug.Log("Leave event");
                LeaveRoom();
            }
        }
        #endregion
    }
}
