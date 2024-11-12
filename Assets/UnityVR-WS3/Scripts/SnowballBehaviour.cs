using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using WS3;

public class SnowballBehaviour : MonoBehaviour
{
    private int playerHealth;
    private GameObject playerTouched;
    
    private void Start(){
        playerHealth = 3;
    }

    private void OnCollisionEnter (Collision collider) {
        if (collider.transform.gameObject.tag == "Player") {
            playerHealth = collider.transform.gameObject.GetComponent<UserManagement>().Health - 1; 
            collider.transform.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.back * 5, ForceMode.Force);
            Destroy(this);
        }
    }

}
