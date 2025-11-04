using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerSlash;
    // Player Values -Lud
    public byte Sake; // Max 6
    public byte Blood; // Max 9
    public byte AttackDamage;
    public float PlayerSpeed;
    public float CameraSpeed;
    
    // Dash Values -Lud
    [SerializeField] KeyCode dash = KeyCode.LeftShift;
    float DashLenght = 1;
    bool CanDash = true;
    bool CanSlash = true;
    KeyCode Slash = KeyCode.Mouse0;


    [SerializeField] KeyCode Up = KeyCode.W;
    [SerializeField] KeyCode Down = KeyCode.S;
    [SerializeField] KeyCode Right = KeyCode.D;
    [SerializeField] KeyCode Left = KeyCode.A;

    void Update()
    {
        // Player Rotation To Mouse -Lud
        Vector3 Diffrence = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Diffrence.Normalize();
        float RotaitionZ = Mathf.Atan2(Diffrence.y, Diffrence.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, RotaitionZ - 90);

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

        if (Input.GetKeyDown(Slash) && CanSlash)
        {
            StartCoroutine(DelaySlash(0.5f));
        }
    }

    void LateUpdate() // Lerps The Camera Based On The Player -Lud
    {
        Camera.main.transform.position = Vector3.Lerp( Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, -10), Time.deltaTime * CameraSpeed);
    }

    IEnumerator DelayDash(float delayTime) // Does A Dash And Delays It -Lud & Maja
    {
        transform.position += transform.up * DashLenght;
        CanDash = false;
        yield return new WaitForSeconds(delayTime);
        CanDash = true;
    }
    IEnumerator DelaySlash(float delayTime)                     //handles the slash and it's cooldown -Maja
    {
        Vector3 SlashPosition = transform.position + transform.up * 2;
        Instantiate(PlayerSlash, SlashPosition, transform.rotation);
        CanSlash = false;
        yield return new WaitForSeconds(delayTime);
        CanSlash = true;
    }
}
