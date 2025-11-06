using UnityEngine;
using System.Collections;

public class PlayerSlash : MonoBehaviour
{

    //variables are all from the PlayerManager script -Maja
    public PlayerManager PlayerManager;
    public GameObject Player;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerManager = Player.GetComponent<PlayerManager>();
        Destroy(gameObject, 0.2f); //destroys the game object - Maja
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MeleeEnemyAttack"))
        {
            collision.gameObject.tag = "MeleePlayerAttack";
        }

        //Adds bullets to the magazine per bullet parried with a cap and bounces bullets at the cursor with  - Maja
        if (collision.CompareTag("RangedEnemyAttack"))
        {
            if (PlayerManager.CurrentMag < PlayerManager.MagSize && PlayerManager.BulletsGrabbed < PlayerManager.MaxBulletGrab)
            {
                PlayerManager.BulletsGrabbed++;
                Destroy(collision.gameObject);
            }
            else
            {
                Vector3 Diffrence = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                Diffrence.Normalize();
                float RotationZ = Mathf.Atan2(Diffrence.y, Diffrence.x) * Mathf.Rad2Deg;
                collision.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, RotationZ - 90);
                collision.gameObject.tag = "RangedPlayerAttack";
            }
        }
    }

}
