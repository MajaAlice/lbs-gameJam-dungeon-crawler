using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public PlayerManager PlayerManager;
    public GameObject Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerManager = Player.GetComponent<PlayerManager>();
    }

    //bullet speed is controlled by the PlayerManager
    void Update()
    {
        transform.position += transform.up * PlayerManager.BulletSpeed * Time.deltaTime;
    }
}
