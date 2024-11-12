using Project.UI;
using Project.Utility;
using Photon.Pun;
using UnityEngine;

namespace Project.Entities
{
    public abstract class EntityBase : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Header("Stats")]
        
        protected int maxHealth;
        
        protected int currentHealth = 10;
        
        public float TimeBetweenShot { get; protected set; }

        public Cooldown betweenShotCooldown;
        [Header("References Shot")]
        public GameObject bulletPrefab;
        public Transform bulletSpawnPoint;
        [Header("UI References ")]
        public HealthBar healthBar;
        public ReloadAmmo reloadUI;

        protected virtual void Awake()
        {
            InitializeWithGameSettings();

            betweenShotCooldown = new Cooldown(TimeBetweenShot);
            currentHealth = maxHealth;
            if (healthBar)
            {
                healthBar.SetMaxHealth(this.maxHealth);
            }
            if(reloadUI != null)
            {
                reloadUI.gameObject.SetActive(false);
            }

        }

        protected virtual void Update()
        {
        }



        private void InitializeWithGameSettings()
        {
            GameConfig.Instance.LoadJson();
            maxHealth = GameConfig.Instance.lifeNumber;
            TimeBetweenShot = GameConfig.Instance.delayShoot;
        }

        protected abstract void CheckShoot();
       
        [PunRPC]
        public virtual void TakeDamage(int damage)
        {
            this.currentHealth -= damage;
            if(IsDead())
            {
                Debug.Log(this.gameObject.name +" is dead x_x");
                photonView.RPC("Die",RpcTarget.All);
               
            }
            if(healthBar != null)
            {
                healthBar.SetHealth(this.currentHealth);

            }
        }

        [PunRPC]
        protected virtual void Die()
        {
            FindObjectOfType<AudioManager>().Play("Mort");
            SpawnManager.Instance.Respawn(this.gameObject);
        }

        [PunRPC]
        public virtual void Respawn()
        {
            this.currentHealth = maxHealth;
            this.healthBar.SetHealth(currentHealth);
        }

        private bool IsDead()
        {
            return currentHealth <= 0;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                Debug.Log("writing");
                stream.SendNext(currentHealth);
            }
            else
            {
                Debug.Log("receiving");
                currentHealth = (int)stream.ReceiveNext();
                if (healthBar != null)
                {
                    healthBar.SetHealth(this.currentHealth);

                }
            }


        }
    }
}

