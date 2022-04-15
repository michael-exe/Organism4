using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class AttachmentController : MonoBehaviour
    //really: HolderController
{
    public CursorXhair CursorXhair; 
    private CursorXhair cursorXhair;
    private Player player;

    public SpriteRenderer spriteRenderer;
    //DETECTOR(ext) 
    public Transform UR_Detector;
    public Transform UL_Detector;
    public Transform DR_Detector;
    public Transform DL_Detector;
    //HOLDER(int)
    public Transform UR_Holder;
    public Transform UL_Holder;
    public Transform DR_Holder;
    public Transform DL_Holder;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //DETECTOR(ext)  

        ALL_Attach(UR_Detector.position,UR_Holder);
        ALL_Attach(UL_Detector.position,UL_Holder);
        ALL_Attach(DR_Detector.position,DR_Holder);
        ALL_Attach(DL_Detector.position,DL_Holder);
    }

    void ALL_Attach(Vector2 pos,Transform _holder){

    RaycastHit2D Ext2Int = Physics2D.Raycast(pos, Vector2.zero, 0);

        if (Ext2Int.collider != null && Ext2Int.collider.tag == "Ext_Molecule" && gameObject.tag == "Int_Molecule") {
            Ext2Int.collider.transform.parent = _holder;
            Ext2Int.collider.gameObject.transform.position = _holder.position;
            Ext2Int.collider.gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            Ext2Int.collider.gameObject.tag = "Int_Molecule";
            Ext2Int.collider.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            if (Ext2Int.collider.gameObject.GetComponent<MoleculeExplosion>().canExplode)
            {
                Ext2Int.collider.gameObject.GetComponent<MoleculeExplosion>().canExplode = false;
            }
            FindObjectOfType<Player>().objectGrabed.Add(Ext2Int.collider.gameObject);

            //if (cursorXhair.Explosives.Count >= 1)
            //{
            //    for (int i = 0; i < cursorXhair.Explosives.Count; i++) // through the loop we will look if the molecule it's in the explosive liste
            //    {                                                               // if yes then remove it from the list and make the the explode to false
            //        if (cursorXhair.Explosives[i] == Ext2Int.collider.gameObject)
            //        {
            //            Debug.Log("ture");
            //            cursorXhair.Explosives.Remove(Ext2Int.collider.gameObject);
            //            Ext2Int.collider.gameObject.GetComponent<MoleculeExplosion>().canExplode = false;
            //        }

            //    }
            //}
            //player.objectGrabed.Add(Ext2Int.collider.gameObject);
            //FindObjectOfType<Player>().objectGrabed.Add(Ext2Int.collider.gameObject);
            //Debug.Log("ObjectGrabed");

            //CursorXhair.Explosives.Remove(Ext2Int.collider.gameObject);

            //if (CursorXhair.Explosives.Count >= 1)
            //{
            //    CursorXhair.Explosives.RemoveAt(CursorXhair.Explosives.Count - 1);
            //}

            //And remove from explosives
        }
    }
}
//https://youtu.be/U8gUnpeaMbQ Snake
//https://youtu.be/1uq43EIzo-U Grab
//https://youtu.be/cIeWhztKyAg Asteroids
//https://answers.unity.com/questions/1455956/using-function-return-in-if-statement.html 