using UnityEngine;

public class BotNormalShooting : MonoBehaviour
{

    public Transform player;
    public float range = 50.0f;
    public float bulletImpulse = 20.0f;

    private bool onRange = false;

    public Rigidbody projectile;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float rand = Random.Range(1.0f, 2.0f);
        InvokeRepeating("Shoot", 2, rand);
    }

    void Shoot()
    {
        if (onRange)
        {
            Rigidbody bullet = (Rigidbody)Instantiate(projectile, transform.position + transform.forward, transform.rotation);
            bullet.AddForce(transform.forward * bulletImpulse, ForceMode.Impulse);

            Destroy(bullet.gameObject, 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        onRange = Vector3.Distance(transform.position, player.position) < range;

        if (onRange) transform.LookAt(player);
    }
}

