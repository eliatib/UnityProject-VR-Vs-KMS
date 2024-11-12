using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    #region Buttons Setup

    public Button quitBtn;
    public Button backBtn;
    public Button applyBtn;
    public InputField lifeNumberInput;
    public InputField delayShootInput;
    public InputField delayTeleportInput;
    public InputField colorShotVirusInput;
    public InputField colorShotKMSInput;
    public InputField nbContaminedPlayerToVictoryInput;
    public InputField radiusExplosionInput;
    public InputField timeToAreaContaminationInput;

    #endregion
    
    void Awake()
    {
        GameConfig.Instance.LoadJson();
        lifeNumberInput.text = "" + GameConfig.Instance.lifeNumber;
        delayShootInput.text = "" + GameConfig.Instance.delayShoot;
        delayTeleportInput.text = "" + GameConfig.Instance.delayTeleport;
        colorShotVirusInput.text = GameConfig.Instance.colorShotVirus;
        colorShotKMSInput.text = GameConfig.Instance.colorShotKMS;
        nbContaminedPlayerToVictoryInput.text = "" + GameConfig.Instance.nbContaminedPlayerToVictory;
        radiusExplosionInput.text = "" + GameConfig.Instance.radiusExplosion;
        timeToAreaContaminationInput.text = "" + GameConfig.Instance.timeToAreaContamination;
    }

    void saveSettings()
    {
        if (lifeNumberInput.text != "")
            GameConfig.Instance.lifeNumber = int.Parse(lifeNumberInput.text);
        if (delayShootInput.text != "")
            GameConfig.Instance.delayShoot = int.Parse(delayShootInput.text);
        if (delayTeleportInput.text != "")
            GameConfig.Instance.delayTeleport = int.Parse(delayTeleportInput.text);
        if (colorShotVirusInput.text != "")
            GameConfig.Instance.colorShotVirus = colorShotVirusInput.text;
        if (colorShotKMSInput.text != "")
            GameConfig.Instance.colorShotKMS = colorShotKMSInput.text;
        if (nbContaminedPlayerToVictoryInput.text != "")
            GameConfig.Instance.nbContaminedPlayerToVictory = int.Parse(nbContaminedPlayerToVictoryInput.text);
        if (radiusExplosionInput.text != "")
            GameConfig.Instance.radiusExplosion = int.Parse(radiusExplosionInput.text);
        if (timeToAreaContaminationInput.text != "")
            GameConfig.Instance.timeToAreaContamination = int.Parse(timeToAreaContaminationInput.text);
        GameConfig.Instance.OverWriteJson();
    }

    public void pressedBtn(Button pressed)
    {
        if (pressed == quitBtn)
        {
            Application.Quit();
        } else if (pressed == backBtn)
        {
            PhotonNetwork.LoadLevel("MainMenu");
        } else if (pressed == applyBtn)
        {
            saveSettings();
        }
    }
    public void playSound(string name)
    {
        FindObjectOfType<AudioManager>().Play(name);
    }
}
