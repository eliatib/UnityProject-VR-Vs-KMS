using Photon.Pun;
using Project.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Project.Entities
{
    public class Virus : EntityBase
    {
        [Header("ShootHand")]
        public SteamVR_Behaviour_Pose m_pose;
        public SteamVR_Action_Boolean m_shootAction;
        SteamVR_Input_Sources handSource;

        [Header("Shield Reference")]
        public GameObject Shield;

        protected override void Awake()
        {
            base.Awake();
            handSource = m_pose.inputSource;
        }

        protected override void Update()
        {
            base.Update();
            CheckShoot();
        }
   

        protected override void CheckShoot()
        {
            betweenShotCooldown.CooldownUpdate();
            reloadUI.SetCooldown();
            if (!betweenShotCooldown.IsOnCooldown())
            {
                if (m_shootAction.GetStateDown(handSource))
                {
                    betweenShotCooldown.StartCooldown();
                    reloadUI.gameObject.SetActive(true);
                    reloadUI.SetMaxCooldown(this.TimeBetweenShot);

                    photonView.RPC("Shoot", RpcTarget.All);
                }
            }

        }


        [PunRPC]
        protected void Shoot()
        {
            BulletProjectile bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<BulletProjectile>();
            bullet.caster = this;
        }

        [PunRPC]
        public override void Respawn()
        {
            base.Respawn();
            if (!Shield.activeInHierarchy)
            {
                Shield.SetActive(true);
            }
            Shield.GetComponent<Shield>().ResetHit();
            Shield.transform.localScale = Vector3.one;


        }
    }
}

