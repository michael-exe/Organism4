using UnityEngine;

public class CursorXhair : MonoBehaviour
{
    public Player Player;
    public AttachmentController Molecule;
    public Sprite newSprite;
    public Sprite originalSprite;
    //[SerializeField] private GameObject m2pCollision;

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
            Debug.Log("mouse2Player is colliding");
            Molecule.spriteRenderer.sprite = newSprite;
        }
        else
        {
            Debug.Log("mouse2Player is NOT colliding");
            Molecule.spriteRenderer.sprite = originalSprite;
        }
    }
}
