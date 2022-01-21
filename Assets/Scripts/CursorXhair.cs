using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CursorXhair : MonoBehaviour
{
    public Player Player;
    public AttachmentController Molecule;
    //public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    public Sprite originalSprite;
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
        Vector2 direction = mousePos - playerPos;

        transform.position = mousePos;

        Debug.DrawRay(playerPos, direction);
        //longer definition instead of if
        //I declare and define an enumerable variables
        //_ => _ means convert any parameter in any parameter
        var hitObjects = Physics2D.LinecastAll(mousePos, playerPos, mask)
            .Select(_ => _.collider.GetComponent<SpriteRenderer>())
            .Where(_ => _ != null)
            .Where(_ => _.CompareTag("Int_Molecule"));

        //iterate enumerable 
        foreach (var spriteRenderer in hitObjects)
        {
            //use the iteration of the enumerable to look the absense of a component in the list
            if (!collidingObjects.Contains(spriteRenderer))
            {
                collidingObjects.Add(spriteRenderer);
                spriteRenderer.sprite = newSprite;
            }
        }

        //this is a sublist made with Linq new list method
        var notLongertHitObjects = collidingObjects.Except(hitObjects).ToList();
        //for needs [i]
        //for iterates a specific number of times
        //everytime loop add one to check all elements
        for (var i = 0; i < notLongertHitObjects.Count; i++)
        {
            notLongertHitObjects[i].sprite = originalSprite;
            collidingObjects.Remove(notLongertHitObjects[i]);
        }
    }
}