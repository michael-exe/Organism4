using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public Player Playerscript; 
    public Collider2D membraneCollider;
    void Update()
    {
        Level_1();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.tag == "Player")
        {
            SceneManager.LoadScene("Organism");
        }
    }

    void Level_1()
    {
        if (Playerscript.UR_Holder.childCount == 1 && Playerscript.UL_Holder.childCount == 1 && Playerscript.DR_Holder.childCount == 1 && Playerscript.DL_Holder.childCount == 1)
        {
            membraneCollider.enabled = true;
        }
        else
        {
            membraneCollider.enabled = false;
        }
    }
}
