using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomTrigger : MonoBehaviour
{

    public GameObject Enemy;

    public int RoomNr = 0;
    public int CurrentRoom = 0;

    bool WasUsed = false;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && CurrentRoom != RoomNr && WasUsed == false)
        {
            Vector3 SpawnPoint = transform.position;
            CurrentRoom = RoomNr;
            Instantiate(Enemy, SpawnPoint, Quaternion.Euler(0,0,0));
        }
    }
}
