using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] KeyCode Up = KeyCode.W;
    [SerializeField] KeyCode Down = KeyCode.S;
    [SerializeField] KeyCode Right = KeyCode.D;
    [SerializeField] KeyCode Left = KeyCode.A;
    void Update()
    {
        Vector3 Diffrence = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Diffrence.Normalize();
        float RotaitionZ = Mathf.Atan2(Diffrence.y, Diffrence.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, RotaitionZ - 90);

        Vector2 MovementVector = Vector2.zero;
        if (Input.GetKeyDown(Up)) { MovementVector += Vector2.up; }
        if (Input.GetKeyDown(Down)) { MovementVector += Vector2.down; }
        if (Input.GetKeyDown(Right)) { MovementVector += Vector2.right; }
        if (Input.GetKeyDown(Up)) { MovementVector += Vector2.left; }

        gameObject.transform.position += new Vector3(MovementVector.x, MovementVector.y, 0);
    }
}
