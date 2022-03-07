using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyHealth = 100f;

    public float FlashingTime = .05f;
    public float TimeInterval = .1f;
    float TotalTime = 2f;


    public void TakeDamage(float amount)
    {
        StartCoroutine(Flash(FlashingTime, TimeInterval));
        enemyHealth -= amount;

        if (enemyHealth <= 0) { EnemyDead(); }
    }

    public void EnemyDead()
    {
        Destroy(gameObject);
    }

    IEnumerator Flash(float time, float intervalTime)
    {
        //this counts up time until the float set in FlashingTime
        float elapsedTime = 0f;
        //This repeats our coroutine until the FlashingTime is elapsed
        while (elapsedTime < time)
        {
            //This gets an array with all the renderers in our gameobject's children
            Renderer[] RendererArray = GetComponentsInChildren<Renderer>();
            //this turns off all the Renderers
            foreach (Renderer r in RendererArray)
                r.enabled = false;
            //then add time to elapsedtime
            elapsedTime += Time.deltaTime;
            //then wait for the Timeinterval set
            yield return new WaitForSeconds(intervalTime);
            //then turn them all back on
            foreach (Renderer r in RendererArray)
                r.enabled = true;
            elapsedTime += Time.deltaTime;
            //then wait for another interval of time
            yield return new WaitForSeconds(intervalTime);
        }
    }
}
