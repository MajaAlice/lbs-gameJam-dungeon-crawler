using UnityEngine;


public class DashCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 0.5f); //destroys the game object after 500 miliseconds - Maja
    }

}

