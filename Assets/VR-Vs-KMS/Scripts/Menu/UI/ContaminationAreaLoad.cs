using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContaminationAreaLoad : MonoBehaviour
{
    public Image circle;

    private float timeMax;
    private float timeRemaning;

    public void SetMaxCooldown(float time)
    {
        timeMax = time;
        timeRemaning = time;
    }

    public void SetCooldown()
    {
        if (timeRemaning > 0)
        {
            timeRemaning -= Time.deltaTime;
            circle.fillAmount = (timeMax - timeRemaning)/timeMax;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
