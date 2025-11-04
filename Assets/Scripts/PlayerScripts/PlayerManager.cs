using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerSlash;
    public GameObject Dash;

    // Player Values -Lud
    public int Health = 7; // Base 7
    public byte Sake = 0; // Max 6
    public byte Blood = 0; // Max 9
    public byte AttackDamage = 1;
    public float MeleeDamageMult = 1;
    public float RangedDamageMult = 1;
    // Speed Values -Lud
    public float PlayerSpeed = 10;
    public float CameraSpeed = 5;
    public float DashLenght = 1;
    public Vector3 SlashSize = new Vector3(2,2,1);
    public float SlashDistance = 1;

    // Bools -Lud
    bool CanDash = true;
    bool CanSlash = true;

    // Movement Keys -Lud
    [SerializeField] KeyCode Up = KeyCode.W;
    [SerializeField] KeyCode Down = KeyCode.S;
    [SerializeField] KeyCode Right = KeyCode.D;
    [SerializeField] KeyCode Left = KeyCode.A;
    // Attacks Keys -Lud
    [SerializeField] KeyCode dash = KeyCode.LeftShift;
    [SerializeField] KeyCode Slash = KeyCode.Mouse0;
    [SerializeField] KeyCode Aim = KeyCode.Mouse1;

    //raycast variables
    public RaycastHit2D HitInfo;

    void Update()
    {
        // Player Rotation To Mouse -Lud
        Vector3 Diffrence = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Diffrence.Normalize();
        float RotationZ = Mathf.Atan2(Diffrence.y, Diffrence.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, RotationZ - 90);

        // Player Movement -Lud
        Vector2 MovementVector = Vector2.zero;
        if (Input.GetKey(Up)) { MovementVector += Vector2.up; }
        if (Input.GetKey(Down)) { MovementVector += Vector2.down; }
        if (Input.GetKey(Right)) { MovementVector += Vector2.right; }
        if (Input.GetKey(Left)) { MovementVector += Vector2.left; }

        MovementVector = MovementVector.normalized * (PlayerSpeed * Time.deltaTime);
        gameObject.transform.position += new Vector3(MovementVector.x, MovementVector.y, 0);

        if (Input.GetKeyDown(dash) && CanDash)
        {
            StartCoroutine(DelayDash(1));
        }



        // Switches Player between aim mode - Maja
        if (Input.GetKey(Aim))
        {
            //controls the shooting - Maja
            //if (Input.GetKey(Slash))
            //{
            //    var HitInfo = Physics2D.Raycast(transform.position, transform.up);
            //}
        }
        else if (Input.GetKeyDown(Slash) && CanSlash)
        {
            StartCoroutine(DelaySlash(0.5f));
        }

    }

    void LateUpdate() // Lerps The Camera Based On The Player -Lud
    {
        Camera.main.transform.position = Vector3.Lerp( Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, -10), Time.deltaTime * CameraSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyManager EnemyManager = collision.GetComponent<EnemyManager>();
        if (collision.CompareTag("MeleeEnemyAttack"))
        {
            Health -= EnemyManager.Damage;
        }
    }

    IEnumerator DelayDash(float delayTime) // Does A Dash And Delays It -Lud & Maja
    {
        transform.position += transform.up * DashLenght;
        Vector3 DashPosition = transform.position - transform.up * (DashLenght / 2);
        GameObject TempDash = Instantiate(Dash, DashPosition, transform.rotation);
        TempDash.transform.localScale = new Vector3(0.7f, DashLenght, 1);
        CanDash = false;
        yield return new WaitForSeconds(delayTime);
        CanDash = true;
    }
    IEnumerator DelaySlash(float delayTime)                     //handles the slash and it's cooldown -Maja
    {
        Vector3 SlashPosition = transform.position + transform.up * SlashDistance;
        GameObject TempSlash = Instantiate(PlayerSlash, SlashPosition, transform.rotation);
        TempSlash.transform.localScale = SlashSize;
        CanSlash = false;
        yield return new WaitForSeconds(delayTime);
        CanSlash = true;
    }
}
