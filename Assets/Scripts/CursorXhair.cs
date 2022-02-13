using System;
using System.Collections.Generic;
//for enum and quering
using System.Linq;
using UnityEngine;

public class CursorXhair : MonoBehaviour
{
    public Player Player;
    public AttachmentController Molecule;
    public Sprite newSprite;
    public Sprite originalSprite;
    //bool hasCollided;
    List<SpriteRenderer> collidingObjects = new List<SpriteRenderer>();
    private LayerMask mask;
    


    private void Awake()
    {
        mask = ~(1 << LayerMask.NameToLayer("Player"));
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = Player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 debugDirection = mousePos - playerPos;

        transform.position = mousePos;

        Debug.DrawRay(playerPos, debugDirection);
        //longer linq definition instead of multiple && if conditions/list
        //I declare and define an enumerable variable
        //_ => _ means to pass any viable parameter to consider it
        //Similar to Linecast except that all Colliders are reported
        var hitObjects = Physics2D.LinecastAll(mousePos, playerPos, mask)
            .Select(_ => _.collider.GetComponent<SpriteRenderer>())
            .Where(_ => _ != null)
            .Where(_ => _.CompareTag("Int_Molecule"));

        var hitObject = Physics2D.Linecast(mousePos, playerPos, mask);

        if (hitObject.collider != null)
        {
            hitObject.collider.GetComponent<SpriteRenderer>();
        }
        
        //iterate enumerable 
        foreach (var spriteRenderer in hitObjects)
        {
            //use the iteration of the enumerable to check and update the list
            if (!collidingObjects.Contains(spriteRenderer))
            {
                collidingObjects.Add(spriteRenderer);
            }
        }

        var notLongetHitObjects = collidingObjects.Except(hitObjects).ToList();
        //"for" needs [i]
        //"for" iterates a specific number of times
        //everytime it loops add one to check all elements
        for (var i = 0; i < notLongetHitObjects.Count; i++)
        {
            notLongetHitObjects[i].sprite = originalSprite;
            collidingObjects.Remove(notLongetHitObjects[i]);
        }

        if (collidingObjects.Count == 0)
        {
            return;
        }

        var closestObject = collidingObjects.OrderBy(_ => (_.transform.position - (Vector3)mousePos).sqrMagnitude).First();
        closestObject.sprite = newSprite;

        foreach (var spriteRenderer in collidingObjects.Where(_ => _ != closestObject))
        {
            spriteRenderer.sprite = originalSprite;
        }
    }
}