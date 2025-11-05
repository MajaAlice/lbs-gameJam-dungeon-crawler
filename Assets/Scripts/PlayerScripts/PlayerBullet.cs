using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float BulletSpeed = 5;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * BulletSpeed * Time.deltaTime;
    }
}
