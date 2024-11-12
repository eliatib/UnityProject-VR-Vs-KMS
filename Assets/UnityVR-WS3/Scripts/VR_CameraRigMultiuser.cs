using Photon.Pun;
using Project.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace WS3
{
    /// <summary>
    /// Make the Steam VR available in multiplayer by deactivating script for UserOther
    /// Support version # SteamVR Unity Plugin - v2.0.1
    /// </summary>
    public class VR_CameraRigMultiuser : MonoBehaviourPunCallbacks
    {

        // reference to SteamController
        public GameObject teleporting,teleportAreas;
        public GameObject VRReplica, VRPlayer;
        public Transform VRHead, VRLeftHand, VRRightHand;
        public Transform ReplicaHead, ReplicaLeftHand, ReplicaRightHand;
        private GameObject goFreeLookCameraRig;

        // Use this for initialization
        void Start()
        {
            updateGoFreeLookCameraRig();
            steamVRactivation();
        }

        private void Update()
        {
            SynchonizeVRPlayer();
        }

        /// <summary>
        /// deactivate the FreeLookCameraRig since we are using the HTC version
        /// Execute only in client side
        /// </summary>
        protected void updateGoFreeLookCameraRig()
        {
            // Client execution ONLY LOCAL
            if (!photonView.IsMine) return;
            Debug.Log("I am a VR");
            goFreeLookCameraRig = null;

            try
            {
                // Get the Camera to set as the follow camera
                goFreeLookCameraRig = GameObject.Find("/FreeLookCameraRig");
                // Deactivate the FreeLookCameraRig since we are using the SteamVR camera
                goFreeLookCameraRig.SetActive(false);
                Instantiate(teleporting);
                Instantiate(teleportAreas, new Vector3(0f, 0.05f, 0f),Quaternion.identity);
                
                
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
            }
        }


        /// <summary>
        /// If we are the client who is using the HTC, activate component of SteamVR in the client using it
        /// If we are not the client using this specific HTC, deactivate some scripts.
        /// </summary>
        protected void steamVRactivation()
        {
          
           
            if (!photonView.IsMine)
            {
                Debug.Log("I am a 3rd person");
                VRReplica.SetActive(true);
            }
            else
            {
                VRPlayer.SetActive(true);
            }
        }

        void SynchronizePlayerAndReplica(Transform VRPlayerTransform, Transform ReplicaTransform)
        {
            ReplicaTransform.position = VRPlayerTransform.position;
            ReplicaTransform.rotation = VRPlayerTransform.rotation;
        }

        void SynchronizeScalePlayerAndReplica(Transform VRPlayerTransform, Transform ReplicaTransform)
        {
            ReplicaTransform.localScale = VRPlayerTransform.localScale;

        }

        void SynchonizeVRPlayer()
        {
            if (!photonView.IsMine)
            {
                if (VRHead.position != ReplicaHead.position)
                {
                    SynchronizePlayerAndReplica(VRHead, ReplicaHead);
                }
                if (VRLeftHand.position != ReplicaLeftHand.position)
                {
                    SynchronizePlayerAndReplica(VRLeftHand, ReplicaLeftHand);

                    Shield shield = VRLeftHand.GetComponentInChildren<Shield>();
                    if (shield != null)
                    {
                        SynchronizeScalePlayerAndReplica(shield.transform, ReplicaLeftHand.Find("Shield"));

                    }
                }
                if (VRRightHand.position != ReplicaRightHand.position)
                {
                    SynchronizePlayerAndReplica(VRRightHand, ReplicaRightHand);
                }
            }
        }
        

    }
}