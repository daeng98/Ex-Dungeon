using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    public GameObject uiResult;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float healthDecay;
    public float time;

    void Update()
    {
        health.Subtract(healthDecay * Time.deltaTime);

        if (!CharacterManager.Instance.Player.controller.isRun)
        {
            stamina.Add(stamina.passiveValue * Time.deltaTime);
        }

        if(stamina.curValue < CharacterManager.Instance.Player.controller.needStamina)
        {
            CharacterManager.Instance.Player.controller.isRun = false;
        }

        if (health.curValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amout)
    {
        if(health.curValue < health.maxValue)
        {
            health.Add(amout);

            if(health.curValue >= health.maxValue)
            {
                health.curValue = health.maxValue;
            }
        }
    }

    public void Die()
    {
        CharacterManager.Instance.Player.ShowResult();
    }

    public bool UseStamina(float amout)
    {
        if (stamina.curValue - amout < 0f)
        {
            return false;
        }

        stamina.Subtract(amout);
        return true;
    }

    public float curHealth()
    {
        return health.curValue;
    }

    public float curStamina()
    {
        return stamina.curValue;
    }
}
