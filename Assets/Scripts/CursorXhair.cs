using System;
using System.Collections.Generic;
//for enum and quering
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

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = Player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - playerPos;

        transform.position = mousePos;

        Debug.DrawRay(playerPos, direction);
        //Linecast already stops at first collision unlike LinecastAll
        RaycastHit2D hitObjects = Physics2D.Linecast(mousePos, playerPos);

        if (hitObjects.collider != null && hitObjects.collider.tag == "Int_Molecule")
        {
            Debug.Log("Sprite Changed and Added to List");
            hitObjects.collider.GetComponent<SpriteRenderer>().sprite = newSprite;
            collidingObjects.Add(hitObjects.collider.GetComponent<SpriteRenderer>());

        }
        else if (collidingObjects.Count >= 1 && hitObjects.collider.tag == "Player")
        {
            Debug.Log("Sprite Changed and Removed from List");
            //linq method
            collidingObjects.Last().GetComponent<SpriteRenderer>().sprite = originalSprite;
            collidingObjects.RemoveAt(collidingObjects.Count - 1);
        }
        //else if (collidingObjects.Count > 1)
        //{
        //    collidingObjects.Last().GetComponent<SpriteRenderer>().sprite = originalSprite;
        //    collidingObjects.RemoveAt(collidingObjects.Count - 1);
        //}
    }
}
