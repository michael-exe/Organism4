using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentController : MonoBehaviour
    //really: HolderController
{
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

            FindObjectOfType<Player>().objectGrabed.Add(Ext2Int.collider.gameObject);
        }
    }
}
//https://youtu.be/U8gUnpeaMbQ Snake
//https://youtu.be/1uq43EIzo-U Grab
//https://youtu.be/cIeWhztKyAg Asteroids
//https://answers.unity.com/questions/1455956/using-function-return-in-if-statement.html 