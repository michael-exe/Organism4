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
    public Vector2 movement;

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

    private CursorXhair cursorXhair;

    private void Start()
    {
        cursorXhair = FindObjectOfType<CursorXhair>();
    }
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
        //transform.position = transform.position + movement * moveSpeed * Time.fixedDeltaTime;
        //MOVEMENT
         rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    //Player AttachmentController
    void ALL_Attach(Vector2 pos, Transform _holder)
    {

        RaycastHit2D Ext2Int = Physics2D.Raycast(pos, Vector2.zero, 0);

        if (Ext2Int.collider != null && (Ext2Int.collider.tag == "Ext_Molecule"))
        {
            Ext2Int.collider.transform.parent = _holder;
            Ext2Int.collider.gameObject.transform.position = _holder.position;
            Ext2Int.collider.gameObject.tag = "Int_Molecule";
           
            Destroy(Ext2Int.collider.gameObject.GetComponent<Rigidbody2D>());
            
            AttachmentController[] MoleculeChildren = Ext2Int.collider.GetComponentsInChildren<AttachmentController>();


            // Loop throw the molecule and  remove them from the explosive list
            foreach (AttachmentController item in MoleculeChildren)
            {
                if (MoleculeChildren.Length > 0)
                {
                    for (int i = 0; i < cursorXhair.Explosives.Count; i++)
                    {
                        if (item.gameObject == cursorXhair.Explosives[i])
                        {
                            Debug.Log("Hi");
                            cursorXhair.Explosives.Remove(item.gameObject);
                            item.gameObject.GetComponent<MoleculeExplosion>().canExplode = false;
                            i = 0;
                        }
                    }
                }
            }
            // Checking if number of explosives is greater or equal to 1
            if (cursorXhair.Explosives.Count >= 1)
            {
                for (int i = 0; i < cursorXhair.Explosives.Count; i++) // through the loop we will look if the molecule it's in the explosive liste
                {                                                               // if yes then remove it from the list and make the the explode to false
                    if (cursorXhair.Explosives[i] == Ext2Int.collider.gameObject)
                    {
                        Debug.Log("true");
                        cursorXhair.Explosives.Remove(Ext2Int.collider.gameObject);
                        Ext2Int.collider.gameObject.GetComponent<MoleculeExplosion>().canExplode = false;
                    }

                }
            }

            objectGrabed.Add(Ext2Int.collider.gameObject);
            Debug.Log("ObjectGrabed");
        }
    }

    void LevelRestart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Organism");
        }
    }
}
//https://youtu.be/whzomFgjT50 Movement
