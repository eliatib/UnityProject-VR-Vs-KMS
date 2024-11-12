using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TutoManager : MonoBehaviour
{
    public Button backBtn, quitBtn;

    public void pressed (Button pressedBtn)
    {
        if (pressedBtn == quitBtn)
            Application.Quit();
        else if (pressedBtn == backBtn)
            Debug.Log("Should go back");
            PhotonNetwork.LoadLevel("MainMenu");
    }
    public void playSound(string name)
    {
        FindObjectOfType<AudioManager>().Play(name);
    }
}
