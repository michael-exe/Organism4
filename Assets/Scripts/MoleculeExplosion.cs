using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoleculeExplosion : MonoBehaviour
{
    public bool canExplode = false;
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            CheckForDestructible();
        }  
    }

    private void CheckForDestructible()
    {
        //collider array
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        
        foreach(Collider2D c in colliders)
        {
            //must add PlayerHealth as well
            if (canExplode = true && c.GetComponent<EnemyHealth>())
            {
                Debug.Log("I do boom-boom!");
                c.GetComponent<EnemyHealth>().EnemyDead();
            }
        }

    }
}
