using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Project.Utility;

namespace Project.UI
{
    public class ReloadAmmo : MonoBehaviour
    {
        public Image circle;
        public Text textTime;
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
                circle.fillAmount = timeRemaning / timeMax;
                textTime.text = timeRemaning.OneDecimal().ToString() + "s";
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}

