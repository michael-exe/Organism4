using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CursorXhair : MonoBehaviour
{
    public Player Player;
    public Sprite newSprite;
    public Sprite originalSprite;
    public List<SpriteRenderer> collidingObjects = new List<SpriteRenderer>();
    private LayerMask mask;
    public List<GameObject> Explosives;
    //this won't do because molecule explosion is a script on a prefab!!!!!!!!!!!!!!!!
    public MoleculeExplosion moleculeExplosion;
    //EXPLOSION
    //public float expRadius = 10f;            // Radius within which enemies are damaged.
    //public float expForce = 100f;            // Force that enemies are thrown from the blast.
    //public GameObject soundPrefab;            // Audioclip of explosion.
    //public GameObject explosionPrefab;        // Prefab of explosion effect.
    //public float Damage = 100;


    private void Awake()
    {
        mask = ~(1 << LayerMask.NameToLayer("Player"));
    }

    void Update()
    {
        
        Vector2 playerPos = Player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 debugDirection = mousePos - playerPos;

        transform.position = mousePos;

        Debug.DrawRay(playerPos, debugDirection);

       //SELECT ALL SPRITES

       //Select the Component SpriteRenderer (2)
       //of the objects hit by the LinecastAll hitObjects (1-3)
       //that have tag "Int_Molecule" (4)
        var hitObjects = Physics2D.LinecastAll(mousePos, playerPos, mask)
            .Select(_ => _.collider.GetComponent<SpriteRenderer>())
            .Where(_ => _ != null)
            .Where(_ => _.CompareTag("Int_Molecule"));

        //SELECT ONE SPRITE

        var hitObject = Physics2D.Linecast(mousePos, playerPos, mask);

        //Get the Component SpriteRenderer (3)
        //of the object hit by Linecast hitObject (1-2)
        if (hitObject.collider != null)
        {
            hitObject.collider.GetComponent<SpriteRenderer>();
        }
        
        //PASS HIT OBJECTS' SPRITES TO LIST

        //Add a variable "spriteRenderer" as element in the list collidingObjects (3)
        //for every objects that LineCastAll hitObjects has already hit (1)
        //if such variable is not present in the list already (2)
        foreach (var spriteRenderer in hitObjects)
        {
            if (!collidingObjects.Contains(spriteRenderer))
            {
                collidingObjects.Add(spriteRenderer);
            }
        }
       
        //RESTORE SPRITES OF OBJECTS NO LONGER HIT

        //Listed as notLongerHitObjects are the objects in the list collidingObjects except for the ones that are currently being hit by LinecastAll hitObjects
        //as long as there is at least one noLongerHitObjects check every new object in list
        //Turn the sprite of noLongerObject in question into its originalSprite
        //Remove it from the list
        var notLongerHitObjects = collidingObjects.Except(hitObjects).ToList();
        for (var i = 0; i < notLongerHitObjects.Count; i++)
        {
            notLongerHitObjects[i].sprite = originalSprite;
            collidingObjects.Remove(notLongerHitObjects[i]);
        }
        //Stop when there are no more colliding objects AKA the list is empty      
        if (collidingObjects.Count == 0)
        {
            return;
        }
        
        //CHANGE SPRITE OF CLOSEST OBJECT

        //Arrange colldingObjects in order of distance shorter to longer between the colliding object and the mouse and pick the first one
        //Take that closestObject and update its sprite
        var closestObject = collidingObjects.OrderBy(_ => (_.transform.position - (Vector3)mousePos).sqrMagnitude).First();
        closestObject.sprite = newSprite;

        //RESTORE ORIGINAL SPRITE OF LISTED OBJECTS

        //for every varible "spriteRenderer" in list collidingObject that is not the closestObject
        //Keep it as the original sprite
        foreach (var spriteRenderer in collidingObjects.Where(_ => _ != closestObject))
        {
            spriteRenderer.sprite = originalSprite;
        }
        
        Eject();

        if (Explosives.Count >= 1)
        {
            RadioExplosionRange();
        }
    }

    void Eject()
    {
        if (Player.objectGrabed.Count >= 1 && Input.GetMouseButton(0))
        {
            //var obj = Player.objectGrabed.Last();
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var closestObject = collidingObjects.OrderBy(_ => (_.transform.position - (Vector3)mousePos).sqrMagnitude).First();

            Transform[] MoleculeChildren = closestObject.GetComponentsInChildren<Transform>();
            foreach (Transform item in MoleculeChildren)
            {
                item.tag = "Mid_Molecule";
            }
            closestObject.tag = "Mid_Molecule";
            closestObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            closestObject.GetComponent<Rigidbody2D>().AddForce(closestObject.transform.parent.up * Player.throwSpeed);
            closestObject.transform.SetParent(null);
            StartCoroutine(ChangeTag());
            
            //ADD TO EXPLOSIVE LIST
            if (Player.objectGrabed.Count >= 1)
            {
                Explosives.Add(Player.objectGrabed.Last());
            }
            //ONLY THEN remove
            Player.objectGrabed.RemoveAt(Player.objectGrabed.Count - 1);         
        }  
    }
    IEnumerator ChangeTag()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var closestObject = collidingObjects.OrderBy(_ => (_.transform.position - (Vector3)mousePos).sqrMagnitude).First();
        yield return new WaitForSeconds(3f);
        Transform[] MoleculeChildren = closestObject.GetComponentsInChildren<Transform>();
        foreach (Transform item in MoleculeChildren)
        {
            item.tag = "Ext_Molecule";
        }
        closestObject.tag = "Ext_Molecule";        
    }

    //This is currently not very useful because it is always satisfied as it happens exactly when you eject
    //meaning it instantly checks the range and from then they can always explode
    //Still I need a way to turn canExplode true
    void RadioExplosionRange()
    {
        Collider2D[] RadioRange = Physics2D.OverlapCircleAll(Player.transform.position, 6f);

        var ExplosiveMolecules = Explosives.Last();

        foreach (Collider2D myMolecules in RadioRange)
        {
            if (ExplosiveMolecules.GetComponent<MoleculeExplosion>())
            {
                ExplosiveMolecules.gameObject.GetComponent<MoleculeExplosion>().canExplode = true;
            }
        }
    }
}

// MUST FIX "Object reference not set to an instance of an object" caused by Destroy

// AoE https://answers.unity.com/questions/1116039/2d-area-of-effect-script.html

// Getting grandchildren with depth-firts research GetComponentInChildren https://answers.unity.com/questions/908455/how-do-i-get-components-in-grandchildren.html
// foreach and for [i] loop to get children https://answers.unity.com/questions/594210/get-all-children-gameobjects.html
// Apply the same tag to all children of an object: https://answers.unity.com/questions/167644/is-there-an-easy-way-to-apply-the-same-tag-to-all.html