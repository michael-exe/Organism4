using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Player PlayerScript;
    public EnemyHealth EnemyScript;
    public CursorXhair CursorXhairScript;
    public GameObject[] popUps;
    private int popUpIndex;
    public float waitTime = 2f;

    public bool canTutorial = false;

    private void Update()
    {
        for(int i = 0; i < popUps.Length; i++)
        {
            if(i == popUpIndex)
            {
                popUps[i].SetActive(true);
            }
            else
            {
                popUps[i].SetActive(false);
            }
        }
        if(popUpIndex == 0)
        {
            if(waitTime <= 0)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                {
                    popUpIndex++;
                }
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        else if (popUpIndex == 1)
        {
            if (PlayerScript.objectGrabed.Count >= 1)
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 2)
        {
            if (CursorXhairScript.Explosives.Count >= 1)
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 3)
        {
            if (EnemyScript.enemyHealth <= 0)
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 4)
        {
            if (PlayerScript.objectGrabed.Count >= 2)
            {
                popUpIndex++;
            }
        }
    }
}

// Unity Tutorial https://www.youtube.com/watch?v=a1RFxtuTVsk
