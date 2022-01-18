using UnityEngine;

public class CursorXhair : MonoBehaviour
{
    public Player Player;
    public AttachmentController Molecule;
    //public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    public Sprite originalSprite;

    // Start is called before the first frame update
    void Awake()
    {
       Cursor.visible = false;       
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = Player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - playerPos;

        transform.position = mousePos;

        Debug.DrawRay(playerPos, direction);
        RaycastHit2D mouse2Player = Physics2D.Linecast(mousePos, playerPos);

        if (mouse2Player.collider != null && mouse2Player.collider.tag == "Int_Molecule")
        {
            Debug.Log("mouse2Player is colling");
            Molecule.spriteRenderer.sprite = newSprite;
        }
        else
        {
            Molecule.spriteRenderer.sprite = originalSprite;
            Debug.Log("mouse2Player is NOT colling");
        }
    }
}
