using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoleculeExplosion : MonoBehaviour
{
    public bool canExplode = false;
    public float circleRadius = 2;
    float explosionDamage = 50f;
    public GameObject explosionFX;
    public EnemyHealth enemyHealth;
    private List<Collider2D> overlappingDestructibles = new List<Collider2D>();

    //https://forum.unity.com/threads/2d-area-of-effect-script-overlapcircle-not-working.1249492/#post-7946443
    //then you can finally do kinematic+kinematic collision


    private void Update()
    {
        //RIGHT CLICK TO EXPLODE
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
        // There no need to go any further here and waste our time if we cannot "explode".
        if (!canExplode)
            return;

        // This could be a public field and configured in the Inspector.
        // We can use this to filter by layer, contact normals, triggers etc.
        // Here we're doing no filtering at all so everything is returned.
        var contactFilter = new ContactFilter2D().NoFilter();

        // Technically you don't need to check the return count because if there are no results, the list will be empty!
        if (Physics2D.OverlapCircle(transform.position, circleRadius, contactFilter, overlappingDestructibles) > 0)
        {
            foreach (Collider2D colliding in overlappingDestructibles)
            {
                var newExplosion = Instantiate(explosionFX, transform.position, Quaternion.identity);
                Destroy(newExplosion, 0.5f);
                Destroy(gameObject);

                if (colliding.GetComponent<EnemyHealth>())
                {
                    Debug.Log("boom, I damaged an enemy");
                    EnemyHealth enemyHealthScript = colliding.GetComponent<EnemyHealth>();
                    enemyHealthScript.TakeDamage(explosionDamage);
                }
            }
        }
    }
}
