using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;

namespace WS3
{
    public class UserManagement : MonoBehaviourPunCallbacks //, IPunObservable
    {
        public int Health = 3;

        public static GameObject UserMeInstance;
        /// <summary>
        /// Represents the GameObject on which to change the color for the local player
        /// </summary>
        public GameObject GameObjectLocalPlayerColor;
        


        /*#region Snwoball Spawn
        /// <summary>
        /// The Transform from which the snow ball is spawned
        /// </summary>
        [SerializeField] Transform snowballSpawner;
        /// <summary>
        /// The prefab to create when spawning
        /// </summary>
        [SerializeField] GameObject SnowballPrefab;



        // Use to configure the throw ball feature
        [Range(0.2f, 100.0f)] public float MinSpeed;
        [Range(0.2f, 100.0f)] public float MaxSpeed;
        [Range(0.2f, 100.0f)] public float MaxSpeedForPressDuration;
        private float pressDuration = 0;

        #endregion*/

        void Awake()
        {
            if (photonView.IsMine)
            {
                GameConfig.Instance.LoadJson();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Color MyColour = Color.clear;
            ColorUtility.TryParseHtmlString(GameConfig.Instance.colorShotKMS, out MyColour);
            Debug.Log("THIS IS THE COLOR THE GLASSES SHOULD BE :");
            Debug.Log(GameConfig.Instance.colorShotKMS);
            GameObjectLocalPlayerColor.GetComponent<Renderer>().material.SetColor(null, MyColour);
        }


        /*#region Snwoball Spawn
        // Update is called once per frame
        void Update()
        {
            // Don't do anything if we are not the UserMe isLocalPlayer
            //...
            if (photonView.IsMine)
            {    
                if (Input.GetButtonDown("Fire1"))
                {
                    // Start Loading time when fire is pressed
                    pressDuration = 0.0f;
                }
                else if (Input.GetButton("Fire1"))
                {
                    // count the time the Fire1 is pressed
                    pressDuration += 0.1f; 
                    //...
                }

                else if (Input.GetButtonUp("Fire1"))
                {
                    // When releasing Fire1, spawn the ball
                    // Define the initial speed of the Snowball between MinSpeed and MaxSpeed according to the duration the button is pressed
                    var speed = MinSpeed + ((pressDuration/MaxSpeedForPressDuration) * (MaxSpeed - MinSpeed)); //... update with the right value
                    speed = (speed > MaxSpeed) ? MaxSpeed : speed;
                    //Debug.LogFormat(string.Format("time {0:F2} <  {1} => speed {2} < {3} < {4}", pressDuration, MaxSpeedForPressDuration, MinSpeed, speed, MaxSpeed));
                    //ThrowBall(snowballSpawner.transform.position, snowballSpawner.transform.forward * speed, new PhotonMessageInfo());
                    photonView.RPC("ThrowBall", RpcTarget.All, snowballSpawner.transform.position, snowballSpawner.transform.forward * speed, null);
                    
                }
            }
        }

        [PunRPC] // de mes couilles
            void ThrowBall(Vector3 position, Vector3 directionAndSpeed, PhotonMessageInfo info)
        {
            GameObject snowball = null;

            // Tips for Photon lag compensation. Il faut compenser le temps de lag pour l'envoi du message.
            // donc décaler la position de départ de la balle dans la direction
            float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

            //position.x += lag;

            // Instantiate the Snowball from the Snowball Prefab at the position of the Spawner
            //...
            //snowball = PhotonNetwork.Instantiate(SnowballPrefab.name, position, snowballSpawner.transform.rotation);
            snowball = Instantiate(SnowballPrefab, position, snowballSpawner.transform.rotation) as GameObject;
            Player Owner = photonView.Owner;
            
            // Set velocity to the snowballRigidBody direction and speed
            snowball.GetComponent<Rigidbody>().velocity += directionAndSpeed;

            // Instantiate the snow ball
            //...

            // Destroy the Snowball after 5 seconds
            Destroy(snowball, 5);
            //PhotonNetwork.Destroy(snowball);
            }
        #endregion*/

        /*#region Health

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            Debug.Log("HeaderAttribute");
            Debug.Log(Health);
            if(stream.IsWriting)
            {
                Debug.Log("writing");
                stream.SendNext(Health);
            }
            else
            {
                Debug.Log("receiving");
                Health = (int)stream.ReceiveNext();
            }

            switch (Health) {
                case 3:
                    PlayerLocalMat.color = Color.green;
                    break;
                
                case 2:
                    PlayerLocalMat.color = Color.yellow;
                    break;
                
                case 1:
                    PlayerLocalMat.color = Color.red;
                    break;
                
                case 0:
                    break;
            }

        }
        #endregion */

    }
}
