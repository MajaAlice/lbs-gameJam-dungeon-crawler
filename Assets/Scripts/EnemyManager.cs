using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public int Health = 3;
    public byte Damage = 1;
    public float Speed = 7;

    GameObject Player;
    PlayerManager PlayerManager;

    // Enemy Enum Holders -Lud
    public EnemyLogic EL = EnemyLogic.None;
    public EnemyResistances Res = EnemyResistances.None;

    public enum EnemyLogic
    {
        None,
        MuskeetRanged,
        KatanaMelee,
        TankMelee,
    }
    public enum EnemyResistances
    {
        None,
        Katana,
        Gun,
        Sake,
    }
    private void Start()
    {
        // Make Sure The Player Has The Player Tag -Lud
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerManager = Player.GetComponent<PlayerManager>();
    }
    private void Update() // Switches To The Right Int Then Uses It To Move
    {
        // Gets The Rotation To Point Att The Player On The Z Axis -Lud
        Vector3 Diffrence = (Player.transform.position - transform.position).normalized;
        float RotaitionZ = Mathf.Atan2(Diffrence.y, Diffrence.x) * Mathf.Rad2Deg;
        // Makes The Enemy Rotate Slower Then Instantly -Lud
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, RotaitionZ - 90), 10);

        float Distance = (Player.transform.position - transform.position).magnitude;
        switch (EL)
        {
            case EnemyLogic.None:
                Debug.Log("Value Missing");
                break;
            case EnemyLogic.TankMelee:
                transform.position = transform.position + transform.up * (Speed * Time.deltaTime); // Dumb

                break;
            case EnemyLogic.MuskeetRanged:
                if (Distance < 10)
                {
                    transform.position = transform.position - transform.up * (Speed * Time.deltaTime); // Dumb
                }
                else if (Distance > 15)
                {
                    transform.position = transform.position + transform.up * (Speed * Time.deltaTime); // Dumb
                }
                    break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MeleePlayerAttack"))
        {
            
            Health -= Mathf.RoundToInt(PlayerManager.AttackDamage * PlayerManager.MeleeDamageMult);
            PlayerManager.Blood++;
            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("RangedPlayerAttack"))
        {
            Health -= PlayerManager.AttackDamage;
        }
    }
}
