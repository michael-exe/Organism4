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
    private float holdDownStartTime;
    public float maxForce = 800f;
    public float minForce = 100f;
    public SpriteMask forceSpriteMask;
    //this won't do because molecule explosion is a script on a prefab!
    public MoleculeExplosion moleculeExplosion;

    private void Awake()
    {
        mask = ~(1 << LayerMask.NameToLayer("Player"));
        HideForce();
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
    private void HideForce()
    {
        forceSpriteMask.alphaCutoff = 1;
    }

    void Eject()
    {
        if (Player.objectGrabed.Count >= 1 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            holdDownStartTime = Time.time;
        }

        if (Player.objectGrabed.Count >= 1 && Input.GetKeyUp(KeyCode.Mouse0))
        {
            //var obj = Player.objectGrabed.Last();
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var closestObject = collidingObjects.OrderBy(_ => (_.transform.position - (Vector3)mousePos).sqrMagnitude).First();

            //Maybe this part does not work well
            AttachmentController[] MoleculeChildren = closestObject.GetComponentsInChildren<AttachmentController>();
            foreach (AttachmentController item in MoleculeChildren)
            {
                item.gameObject.tag = "Mid_Molecule";
                Explosives.Add(item.gameObject);
                Player.objectGrabed.Remove(item.gameObject);
                item.GetComponent<MoleculeExplosion>().canExplode = true;
            }

            closestObject.tag = "Mid_Molecule";
            closestObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            float holdDownTime = Time.time - holdDownStartTime;
            //closestObject.GetComponent<Rigidbody2D>().AddForce(closestObject.transform.parent.up * Player.throwSpeed);
            closestObject.GetComponent<Rigidbody2D>().AddForce(closestObject.transform.parent.up * CalculateHoldDownForce(holdDownTime));
            closestObject.transform.SetParent(null);
            StartCoroutine(ChangeTag());
        }

        if (Player.objectGrabed.Count >= 1 && Input.GetKey(KeyCode.Mouse0))
        {
            float holdDownTime = Time.time - holdDownStartTime;
            ShowForce(CalculateHoldDownForce(holdDownTime));
        }

    }

    private float CalculateHoldDownForce(float holdTime)
    {
        float maxForceHoldDownTime = 2f;
        float holdTimeNormalized = Mathf.Clamp01(holdTime / maxForceHoldDownTime);
        float force = holdTimeNormalized * maxForce;        
        if (force < minForce)
        {
            return minForce;
        }
        return force;
    }
    IEnumerator ChangeTag()
    {
       
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var closestObject = collidingObjects.OrderBy(_ => (_.transform.position - (Vector3)mousePos).sqrMagnitude).First();
        yield return new WaitForSeconds(3f);

        // NEW // checking if still exsting and didn't explode 
        if(closestObject){
             Transform[] MoleculeChildren = closestObject.GetComponentsInChildren<Transform>();
        foreach (Transform item in MoleculeChildren)
        {
            item.tag = "Ext_Molecule";
        }
        closestObject.tag = "Ext_Molecule";  
        }
             
    }

    public void ShowForce(float force)
    {
        forceSpriteMask.alphaCutoff = 1 - force / maxForce;
    }

    //void RadioExplosionRange()
    //{
    //    if (Explosives.Last().GetComponent<MoleculeExplosion>())
    //    {
    //        Explosives.Last().gameObject.GetComponent<MoleculeExplosion>().canExplode = true;
    //    }
    //}

    //This is currently not very useful because it is always satisfied as it happens exactly when you eject
    //meaning it instantly checks the range and from then they can always explode
    //Still I need a way to turn canExplode true
    void RadioExplosionRange()
    {
        Collider2D[] RadioRange = Physics2D.OverlapCircleAll(Player.transform.position, 6f);

        var ExplosiveMolecules = Explosives.Last();

        foreach (Collider2D myMolecules in RadioRange)
        {
            //I am not using myMolecules... why NOT?!
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
// Throw Force: https://www.youtube.com/watch?v=2BJyG54eP4w
// Physics Materials: https://gruman.co/2d-pool-in-unity-game-dev-tutorial/