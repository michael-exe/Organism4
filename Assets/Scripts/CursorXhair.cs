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
    //public Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    private LayerMask mask;
    //Should find a way to decleare it here
    //SpriteRenderer closestObject = collidingObjects.OrderBy(_ => (_.transform.position - (Vector3)mousePos).sqrMagnitude).First();
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

            //closestObject.GetComponentInChildren<Transform>().tag = "Mid_Molecule";
            Transform[] MeAndMyKids = closestObject.GetComponentsInChildren<Transform>();
            foreach (Transform item in MeAndMyKids)
            {
                item.tag = "Mid_Molecule";
            }
            closestObject.tag = "Mid_Molecule";
            closestObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            closestObject.GetComponent<Rigidbody2D>().AddForce(closestObject.transform.parent.up * Player.throwSpeed);
            closestObject.transform.SetParent(null);
            StartCoroutine(ChangeTag());
            //Destroy(obj, 3f);
            Player.objectGrabed.RemoveAt(Player.objectGrabed.Count - 1);
        }  
    }
    IEnumerator ChangeTag()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var closestObject = collidingObjects.OrderBy(_ => (_.transform.position - (Vector3)mousePos).sqrMagnitude).First();
        yield return new WaitForSeconds(3f);
        //closestObject.GetComponentInChildren<AttachmentController>().tag = "Ext_Molecule";
        Transform[] MeAndMyKids = closestObject.GetComponentsInChildren<Transform>();
        foreach (Transform item in MeAndMyKids)
        {
            item.tag = "Ext_Molecule";
        }
        closestObject.tag = "Ext_Molecule";        
    }

}

// Getting grandchildren with depth-firts research GetComponentInChildren https://answers.unity.com/questions/908455/how-do-i-get-components-in-grandchildren.html
// foreach and for [i] loop to get children https://answers.unity.com/questions/594210/get-all-children-gameobjects.html
// Apply the same tag to all children of an object: https://answers.unity.com/questions/167644/is-there-an-easy-way-to-apply-the-same-tag-to-all.html