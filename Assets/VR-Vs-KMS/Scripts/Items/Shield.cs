using Photon.Pun;
using Project.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Item
{
    [RequireComponent(typeof(Collider), typeof(PhotonView))]
    public class Shield : MonoBehaviourPunCallbacks
    {
        private int maxHit = 5;
        private int hitRemaining;

        private void Awake()
        {
            hitRemaining = maxHit;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 10)
            {
                EntityBase caster = collision.transform.GetComponent<BulletProjectile>().caster;

                if (caster is Scientist)
                {
                    hitRemaining -= 1;
                    photonView.RPC("UpdateShieldScale", RpcTarget.All);
                }

            }
        }
        [PunRPC]
        private void UpdateShieldScale()
        {
            if (hitRemaining == 0)
            {
                gameObject.SetActive(false);
                return;
            }
            transform.localScale = new Vector3(0.5f + hitRemaining * 0.1f, 0.5f + hitRemaining * 0.1f, 0.5f + hitRemaining * 0.1f);
        }

        public void ResetHit()
        {
            hitRemaining = maxHit;
        }
    }
}
