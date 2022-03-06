using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoleculeExplosion : MonoBehaviour
{
    public bool canExplode = false;
    public float circleRadius = 6f;
    public GameObject explosionFX;
    //public Player Player;
    //public CursorXhair CursorXhair;

    private void Update()
    {
        //Collider2D[] RadioRange = Physics2D.OverlapCircleAll(Player.transform.position, 6f);
        //var ExplosiveMolecules = CursorXhair.Explosives.Last();

        //if (CursorXhair.Explosives.Count >= 1)
        //{
        //    Debug.Log("There are explosives in the array");

        //    foreach (Collider2D myMolecules in RadioRange)
        //    {
        //        if (ExplosiveMolecules.GetComponent<MoleculeExplosion>())
        //        {
        //            ExplosiveMolecules.gameObject.GetComponent<MoleculeExplosion>().canExplode = true;
        //        }
        //    }
        //}

        if (Input.GetMouseButton(1))
        { 
            CheckForDestructible();
            if (gameObject.tag == "Mid_Molecule")
            {
                Destroy(gameObject);
            }
        }
    }

    private void CheckForDestructible()
    {
        //collider array
        Collider2D[] overlappingColliders = Physics2D.OverlapCircleAll(transform.position, circleRadius);

        foreach (Collider2D colliding in overlappingColliders)
        {
            //must add PlayerHealth as well
            if (canExplode == true)
            {
                //Vector2 moleculePos = transform.position;
                var newExplosion = Instantiate(explosionFX, transform.position, Quaternion.identity);
                Destroy(newExplosion, 0.5f);
                Destroy(gameObject);
                if (colliding.GetComponent<EnemyHealth>())
                {
                    Debug.Log("boom, I killed an enemy");
                    colliding.GetComponent<EnemyHealth>().EnemyDead();
                }
            }
        }
    }
}
