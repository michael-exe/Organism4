using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{

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

        //EJECT
        if (objectGrabed.Count >= 1 && Input.GetKeyDown(KeyCode.X))
        {
            var obj = objectGrabed[objectGrabed.Count - 1];
            obj.tag = "Mid_Molecule";
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            obj.GetComponent<Rigidbody2D>().AddForce(obj.transform.parent.up * throwSpeed);
            obj.transform.SetParent(null);
            StartCoroutine(ChangeTag());
            //Destroy(obj, 3f);
            objectGrabed.RemoveAt(objectGrabed.Count - 1);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Organism");
        }

        //DETECTOR(ext)  
        ALL_Attach(UR_Detector.position, UR_Holder);
        ALL_Attach(UL_Detector.position, UL_Holder);
        ALL_Attach(DR_Detector.position, DR_Holder);
        ALL_Attach(DL_Detector.position, DL_Holder);
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

            FindObjectOfType<Player>().objectGrabed.Add(Ext2Int.collider.gameObject);
        }      

    }
    //After ejection make attachable again
    IEnumerator ChangeTag()
    {
        var obj = objectGrabed[objectGrabed.Count - 1];
        yield return new WaitForSeconds(3f);
        obj.tag = "Ext_Molecule";
        //yield return new WaitForSeconds(5f);
        //obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }
}
//https://youtu.be/whzomFgjT50 Movement
