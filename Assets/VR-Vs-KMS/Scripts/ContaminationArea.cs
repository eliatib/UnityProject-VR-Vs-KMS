using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

namespace vr_vs_kms
{
    public class ContaminationArea : MonoBehaviourPunCallbacks
    {
        [System.Serializable]
        public struct BelongToProperties
        {
            public Color mainColor;
            public Color secondColor;
            
        }

        public BelongToProperties nobody;
        public BelongToProperties virus;
        public BelongToProperties scientist;

        private float faerieSpeed;
        public float cullRadius = 5f;

        [Header("UI")]
        public ContaminationAreaLoad loadCircleUI;

        //[HideInInspector]
        public List<GameObject> inPoint;

        private float radius = 1f;
        private ParticleSystem pSystem;
        private WindZone windZone;
        private int remainingGrenades;
        private float currentTimer;
        private CullingGroup cullGroup;
        private float maxTimer;
        private string takeBy = "Nobody";
        

        void Awake()
        {
            GameConfig.Instance.LoadJson();
            maxTimer = GameConfig.Instance.timeToAreaContamination;
            currentTimer = maxTimer;
            populateParticleSystemCache();
            setupCullingGroup();
            photonView.RPC("BelongsToNobody", RpcTarget.All);
        }

        private void populateParticleSystemCache()
        {
            pSystem = this.GetComponentInChildren<ParticleSystem>();
        }


        /// <summary>
        /// This manage visibility of particle for the camera to optimize the rendering.
        /// </summary>
        private void setupCullingGroup()
        {
            Debug.Log($"setupCullingGroup {Camera.main}");
            cullGroup = new CullingGroup();
            cullGroup.targetCamera = Camera.main;
            cullGroup.SetBoundingSpheres(new BoundingSphere[] { new BoundingSphere(transform.position, cullRadius) });
            cullGroup.SetBoundingSphereCount(1);
            cullGroup.onStateChanged += OnStateChanged;
        }

        void OnStateChanged(CullingGroupEvent cullEvent)
        {
            Debug.Log($"cullEvent {cullEvent.isVisible}");
            if (cullEvent.isVisible)
            {
                pSystem.Play(true);
            }
            else
            {
                pSystem.Pause();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Virus" || other.tag == "Scientist")
            {
                currentTimer = maxTimer;
                inPoint.Remove(other.gameObject);
                if(inPoint.Count > 0)
                {
                    photonView.RPC("UpdateCircle", RpcTarget.All, inPoint[0].tag);
                }
                else
                {
                    if(takeBy == "Nobody") 
                    {
                        loadCircleUI.circle.color = Color.white;
                    }
                    else 
                    {
                        photonView.RPC("UpdateCircle", RpcTarget.All, takeBy);
                    }
                    loadCircleUI.SetMaxCooldown(maxTimer);
                    loadCircleUI.circle.fillAmount = 1;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.tag == "Virus")
            {
                loadCircleUI.SetMaxCooldown(currentTimer);
                inPoint.Add(other.gameObject);
            }
            if (other.tag == "Scientist")
            {
                loadCircleUI.SetMaxCooldown(currentTimer);
                inPoint.Add(other.gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if(inPoint.Find(i => other.gameObject) != null)
            {
                if (inPoint.Count > 0 && inPoint[0].tag != takeBy)
                {
                    takePoint(inPoint[0]);
                }
            }
        }

        private void takePoint(GameObject inPoint) 
        {
            currentTimer -= Time.deltaTime;
            loadCircleUI.SetCooldown();
            photonView.RPC("UpdateCircle", RpcTarget.All, inPoint.tag);

            if (currentTimer < 0)
            {
                if (inPoint.tag == "Scientist")
                {
                    photonView.RPC("BelongsToScientists", RpcTarget.All);
                    takeBy = "Scientist";
                    currentTimer = maxTimer;
                }

                if (inPoint.tag == "Virus")
                {
                    photonView.RPC("BelongsToVirus", RpcTarget.All);
                    takeBy = "Virus";
                    currentTimer = maxTimer;
                }
            }
        }

        [PunRPC]
        private void UpdateCircle(string other)
        {
            if (other == "Virus")
            {
                loadCircleUI.circle.color = Color.red;
            }
            if (other == "Scientist")
            {
                loadCircleUI.circle.color = Color.green;
            }
        }

        private void ColorParticle(ParticleSystem pSys, Color mainColor, Color accentColor)
        {
            // TODO: Solution to color particle 
            ParticleSystem.MainModule settings = pSys.main; 
            settings.startColor = new ParticleSystem.MinMaxGradient(mainColor, accentColor);
        }

        [PunRPC]
        public void BelongsToNobody()
        {
            ColorParticle(pSystem, nobody.mainColor, nobody.secondColor);
        }

        [PunRPC]
        public void BelongsToVirus()
        {
            ColorParticle(pSystem, virus.mainColor, virus.secondColor);
        }

        [PunRPC]
        public void BelongsToScientists()
        {
            ColorParticle(pSystem, scientist.mainColor, scientist.secondColor);
        }

        void OnDestroy()
        {
            if (cullGroup != null)
                cullGroup.Dispose();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, cullRadius);
        }
    }
}