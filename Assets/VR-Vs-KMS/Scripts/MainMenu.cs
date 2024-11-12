using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    #region buttons

    public Button playBtn;
    
    public Button settingsBtn;

    public Button lvlEditorBtn;

    public Button commandsBtn;

    public Button rulesBtn;

    public Button quitBtn;

    #endregion

    public void Awake()
    {
        FindObjectOfType<AudioManager>().Play("ThemeMenu");
    }

    public void startProgram(Button pressed)
    {
        if (pressed == playBtn)
        {
            PhotonNetwork.LoadLevel("LobbyScene");
        } else if (pressed == settingsBtn)
        {
            PhotonNetwork.LoadLevel("Settings");
        }
        else if (pressed == lvlEditorBtn)
        {
            PhotonNetwork.LoadLevel("AR-Scene");
        }
        else if (pressed == commandsBtn)
        {
            PhotonNetwork.LoadLevel("Commands");
        }
        else if (pressed == rulesBtn)
        {
            PhotonNetwork.LoadLevel("Tuto");
        } else if (pressed == quitBtn)
        {
            Application.Quit();
        }
    }

    public void playSound(string name)
    {
        FindObjectOfType<AudioManager>().Play(name);
    }
}
