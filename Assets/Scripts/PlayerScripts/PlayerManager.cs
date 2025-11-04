using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerSlash;
    public GameObject Dash;
    // Speed Values -Lud
    public float PlayerSpeed;
    public float CameraSpeed;

    // Dash Values -Lud
    [SerializeField] KeyCode dash = KeyCode.LeftShift;
    [SerializeField] float DashLenght = 5;
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

        Vector3 DashPosition = transform.position + (DashLenght / 2) * transform.up;
        GameObject DashThing = Instantiate(Dash, DashPosition, transform.rotation);
        DashThing.transform.localScale = new Vector3(0.7f, DashLenght, 1);
        transform.position += transform.up * DashLenght; 
        CanDash = false;
        yield return new WaitForSeconds(delayTime);
        CanDash = true;
    }
    IEnumerator DelaySlash(float delayTime)                     //handles the slash and it's cooldown
    {
        Vector3 SlashPosition = transform.position + transform.up * 0.7f;
        Instantiate(PlayerSlash, SlashPosition, transform.rotation);
        CanSlash = false;
        yield return new WaitForSeconds(delayTime);
        CanSlash = true;
    }
}
