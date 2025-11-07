using Unity.VisualScripting;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public EnemyManager EnemyManager;
    public GameObject Enemy;

    public int RoomNr = 0;
    void Start()
    {
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EnemyManager.CurrentRoom = RoomNr;
        }
    }
}
