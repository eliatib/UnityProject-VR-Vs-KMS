using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

namespace Project.Entities
{
    public class Scientist : EntityBase
    {   

        protected override void Awake()
        {
            base.Awake();
            GameObject go = GameObject.Find("/FreeLookCameraRig");
            if(go != null)
            {
                go.GetComponent<FreeLookCam>().SetTarget(transform);
            }

            //disable UI of TPS for VR player
            if (!photonView.IsMine)
            {
                healthBar.gameObject.SetActive(false);
                reloadUI.gameObject.SetActive(false);
            }
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
                if (Input.GetButtonDown("Fire1"))
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



    }
}

