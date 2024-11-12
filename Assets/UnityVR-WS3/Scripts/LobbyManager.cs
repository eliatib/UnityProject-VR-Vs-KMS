using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WS3
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [SerializeField]
        private InputField ipInputField;

        [SerializeField]
        private InputField portInputField;

        [SerializeField]
        private InputField nickNameInputField;

        [SerializeField]
        private Toggle VRConnection;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// this parameter is true when we click on the button Start and false if we come back from a game to the Lobby.
        /// </summary>
        private bool isConnecting;

        #endregion Private Serializable Fields

        #region Private Fields

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        private string gameVersion = "1";

        #endregion Private Fields

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        private void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        private void Start()
        {
            // try connecting to the PUN server
            //Connect();
            // Now called by the UI.

            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

            isConnecting = false;

            ipInputField.text = "127.0.0.1";
            //ipInputField.text = "192.168.142.1";
            portInputField.text = "5058";
            nickNameInputField.text = "Player";
        }

        #endregion MonoBehaviour CallBacks

        #region Public Methods

        /// <summary>
        /// Start the connection process when click on the Start button.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            if (string.IsNullOrEmpty(nickNameInputField.text))
            {
                Debug.LogError("Nickname must be set to a value");
                nickNameInputField.placeholder.GetComponent<Text>().text = "!! Nickname Empty!!";
                return;
            }
            Debug.Log(string.Format("Connect event server PhotonNetwork.IsConnected {0}", PhotonNetwork.IsConnected));
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            isConnecting = true;
            PhotonNetwork.NickName = nickNameInputField.text;

            // TODO: put the code to start the connection process and connect to the master server, which manage Lobby process
            // Tells to the user that we are trying to connect
            isConnecting = true;
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room.
                // If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // Used for cloud Server.
                //PhotonNetwork.GameVersion = gameVersion;
                //PhotonNetwork.ConnectUsingSettings();

                // Use local server OnPremise
                // See this thread for more details https://forum.photonengine.com/discussion/comment/43218/#Comment_43218
                string ip = ipInputField.text;
                int port = int.Parse(portInputField.text);
                if (!string.IsNullOrWhiteSpace(ip) && port > 0)
                {
                    PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = false;
                    PhotonNetwork.PhotonServerSettings.AppSettings.Server = ip;
                    PhotonNetwork.PhotonServerSettings.AppSettings.Port = port;
                    PhotonNetwork.PhotonServerSettings.AppSettings.VRConnection = VRConnection.isOn;
                    Debug.LogFormat("Connect to {0}:{1}", ip, port);
                }
                else
                {
                    Debug.LogFormat("Connect cloud server to App Id {0}", PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime.Substring(0, 10) + "...");
                    PhotonNetwork.GameVersion = gameVersion;
                    PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = true;
                }
                PhotonNetwork.ConnectUsingSettings();
                // Not working with ConnectToMaster() with protocol 1.8+ due to protocol version not the same.
                // Except by setting the protocol version manually with the line just below, but it is not maintanable.
                //PhotonNetwork.NetworkingClient.LoadBalancingPeer.SerializationProtocolType = ExitGames.Client.Photon.SerializationProtocol.GpBinaryV16;
                //PhotonNetwork.NetworkingClient.LoadBalancingPeer.TransportProtocol = ExitGames.Client.Photon.ConnectionProtocol.Udp;
                //PhotonNetwork.ConnectToMaster(ip, port, "1381bfd4-902b-46c1-8858-2b659f3d09ce");
            }
        }

        #endregion Public Methods

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            // TODO: it means that we are connected to the master server, so try to join an exising room
            Debug.Log("OnConnectedToMaster() was called by PUN.");
            if (isConnecting)
                PhotonNetwork.JoinRandomRoom();
            Debug.Log("Connected to server");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            // TODO: joining the room has failed, so we try creating a new one
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayersPerRoom;
            PhotonNetwork.CreateRoom(null, roomOptions);
        }

        public override void OnJoinedRoom()
        {
            // TODO the room has been joined, so we can load the Scene for startig the application
            Debug.Log("Loading the scene");

            FindObjectOfType<AudioManager>().Stop("ThemeMenu");

            //#Critical
            //PhotonNetwork.LoadLevel("TestScene");
            PhotonNetwork.LoadLevel(("GameScene"));
        }

        #endregion MonoBehaviourPunCallbacks Callbacks

        public void BackToMenu()
        {
            PhotonNetwork.LoadLevel("MainMenu");
        }

        public void quitApp()
        {
            Application.Quit();
        }

        public void playSound(string name)
        {
            FindObjectOfType<AudioManager>().Play(name);
        }
    }
}