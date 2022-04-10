using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    public CursorXhair CursorXhair;
    
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    Vector2 movement;

    public float throwSpeed;
    public List<GameObject> objectGrabed;

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
    //public Collider2D membraneCollider;

    void Update()
    {
        //INPUT
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //DETECTOR(ext)  
        ALL_Attach(UR_Detector.position, UR_Holder);
        ALL_Attach(UL_Detector.position, UL_Holder);
        ALL_Attach(DR_Detector.position, DR_Holder);
        ALL_Attach(DL_Detector.position, DL_Holder);


        LevelRestart(); 
    }
    //once/frame independent from framerate
    private void FixedUpdate()
    {
        //MOVEMENT
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    //Player AttachmentController
    void ALL_Attach(Vector2 pos, Transform _holder)
    {

        RaycastHit2D Ext2Int = Physics2D.Raycast(pos, Vector2.zero, 0);

        if (Ext2Int.collider != null && Ext2Int.collider.tag == "Ext_Molecule")
        {
            Ext2Int.collider.transform.parent = _holder;
            Ext2Int.collider.gameObject.transform.position = _holder.position;
            Ext2Int.collider.gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            Ext2Int.collider.gameObject.tag = "Int_Molecule";
            Ext2Int.collider.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            
            //FindObjectOfType<Player>().objectGrabed.Add(Ext2Int.collider.gameObject);
            objectGrabed.Add(Ext2Int.collider.gameObject);
            Debug.Log("ObjectGrabed");

            CursorXhair.Explosives.Remove(Ext2Int.collider.gameObject);
            Debug.Log("Explosives removed");


            //check if object in a list is in another list unity

            //if (CursorXhair.Explosives.Count >= 1)
            //{
            //    CursorXhair.Explosives.Remove(CursorXhair.Explosives.Last());
            //    Debug.Log("Explosives removed");
            //}

            //if (CursorXhair.Explosives.Count >= 1)
            //{
            //    CursorXhair.Explosives.Remove(CursorXhair.Explosives.Last());
            //    Debug.Log("Explosives removed");
            //}

            //And remove from explosives
        }

       
    }

    void LevelRestart() {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Organism");
        }
    }
}
//https://youtu.be/whzomFgjT50 Movement
