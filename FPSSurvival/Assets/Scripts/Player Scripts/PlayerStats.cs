using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private Image health_Stats, stamina_stat;

    public void DisplayHealth_Stats(float healthValue)
    {
        healthValue /= 100f;

        health_Stats.fillAmount = healthValue;
    }

    public void DisplayStamina_Stats(float StaminaValue)
    {
        StaminaValue /= 100f;

        stamina_stat.fillAmount = StaminaValue;
    }
}
