using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CursorXhair : MonoBehaviour
{
    public Player Player;
    public Sprite newSprite;
    public Sprite originalSprite;
    List<SpriteRenderer> collidingObjects = new List<SpriteRenderer>();
    private LayerMask mask;
    //public List<GameObject> objectGrabed;

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

        var hitObjects = Physics2D.LinecastAll(mousePos, playerPos, mask)
            .Select(_ => _.collider.GetComponent<SpriteRenderer>())
            .Where(_ => _ != null)
            .Where(_ => _.CompareTag("Int_Molecule"));

        var hitObject = Physics2D.Linecast(mousePos, playerPos, mask);

        if (hitObject.collider != null)
        {
            hitObject.collider.GetComponent<SpriteRenderer>();
        }

        foreach (var spriteRenderer in hitObjects)
        {
            if (!collidingObjects.Contains(spriteRenderer))
            {
                collidingObjects.Add(spriteRenderer);
            }
        }

        var notLongetHitObjects = collidingObjects.Except(hitObjects).ToList();
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
        Eject();
    }

    void Eject()
    {
        if (Player.objectGrabed.Count >= 1 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            //var obj = Player.objectGrabed.Last();
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var closestObject = collidingObjects.OrderBy(_ => (_.transform.position - (Vector3)mousePos).sqrMagnitude).First();
          
            closestObject.tag = "Mid_Molecule";
            closestObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            closestObject.GetComponent<Rigidbody2D>().AddForce(closestObject.transform.parent.up * Player.throwSpeed);
            closestObject.transform.SetParent(null);
            //MUST ADJUST CHANGETAG IN PLAYER AND EVENTUALLY MERGE THE TWO SCRIPTS
            //StartCoroutine(Player.ChangeTag());
            //Destroy(obj, 3f);
            //Player.objectGrabed.RemoveAt(Player.objectGrabed.Count - 1);

            //var obj = Player.objectGrabed.Last();
            //obj.tag = "Mid_Molecule";
            //obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            //obj.GetComponent<Rigidbody2D>().AddForce(obj.transform.parent.up * Player.throwSpeed);
            //obj.transform.SetParent(null);
            //StartCoroutine(Player.ChangeTag());
            ////Destroy(obj, 3f);
            //Player.objectGrabed.RemoveAt(Player.objectGrabed.Count - 1);
        }
    }
}