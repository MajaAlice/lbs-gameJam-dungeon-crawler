using UnityEngine;
using System.Collections;

public class PlayerSlash : MonoBehaviour
{

    //variables are all from the PlayerManager script -Maja
    public PlayerManager PlayerManagerBuh;
    public GameObject Player;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerManagerBuh = Player.GetComponent<PlayerManager>();
        Destroy(gameObject, 0.5f); //destroys the game object - Maja
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MeleeEnemyAttack"))
        {

        }

        //Adds bullets to the magazine per bullet parried with a cap - Maja
        if (collision.CompareTag("RangedEnemyAttack"))
        {
            if (PlayerManagerBuh.CurrentMag < PlayerManagerBuh.MagSize && PlayerManagerBuh.BulletsGrabbed < PlayerManagerBuh.MaxBulletGrab)
            {
                PlayerManagerBuh.BulletsGrabbed++;
            }
        }
    }

}
