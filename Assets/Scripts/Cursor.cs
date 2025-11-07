using UnityEngine;

public class Cursor : MonoBehaviour
{
    public GameObject Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Player.transform.position;
        transform.position = new Vector3(Difference.x, Difference.y, -2);
        
    }
}
