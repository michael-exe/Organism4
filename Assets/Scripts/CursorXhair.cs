using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorXhair : MonoBehaviour
{
    public Player Player;

    // Start is called before the first frame update
    void Awake()
    {
       Cursor.visible = false;       
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouseCursorPos;
        Debug.DrawRay(Player.transform.position, mouseCursorPos);
        RaycastHit2D Player2Mouse = Physics2D.Raycast(Player.transform.position, mouseCursorPos);
    }
}
