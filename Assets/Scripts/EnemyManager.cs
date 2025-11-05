using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{

    public int Health = 3;
    public byte Damage = 1;
    public float Speed = 7;
    public float AimSpeed = 10;

    GameObject Player;
    PlayerManager PlayerManager;

    public GameObject Slash;
    public bool CanSlash = true;
    public float AttackDelay;
    public Vector3 SlashSize;
    public float SlashDistance;

    // Enemy Enum Holders -Lud
    public EnemyLogic EL = EnemyLogic.None;
    public EnemyResistances Res = EnemyResistances.None;

    public enum EnemyLogic
    {
        None,
        MuskeetRanged,
        KatanaMelee,
        TankMelee,
        DashMelee,
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
        float RotationZ = Mathf.Atan2(Diffrence.y, Diffrence.x) * Mathf.Rad2Deg;
        // Makes The Enemy Rotate Slower Then Instantly -Lud
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, RotationZ - 90), AimSpeed * Time.deltaTime);

        float Distance = (Player.transform.position - transform.position).magnitude;
        switch (EL)
        {
            case EnemyLogic.None:
                Debug.Log("Value Missing");
                break;
            case EnemyLogic.KatanaMelee:

                // Checks If The Enemy Is Looking Att The Player And If The CoolDown Is Off -Lud
                if (CanSlash && Distance > 1)
                {
                    StartCoroutine(DelaySlash(AttackDelay));
                }
                // Moves Player So Long As Distance Is Less Then One -Lud
                if (Distance < SlashDistance + SlashSize.y)
                {
                    MoveEnemyTowardsPlayer();
                }

                break;
            case EnemyLogic.TankMelee:

                // Checks If The Enemy Is Looking Att The Player And If The CoolDown Is Off -Lud
                if (CanSlash && Distance > 1)
                {
                    StartCoroutine(DelaySlash(AttackDelay));
                }
                // Moves Player So Long As Distance Is Less Then One -Lud
                if(Distance < (SlashDistance + SlashSize.y))
                {
                    MoveEnemyTowardsPlayer();
                }

                break;

            case EnemyLogic.DashMelee:

                // Checks If The Enemy Is Looking Att The Player And If The CoolDown Is Off -Lud
                if (CanSlash && Distance > 1)
                {
                    StartCoroutine(DelaySlash(AttackDelay));
                }
                // Moves Player So Long As Distance Is Less Then One -Lud
                if (Distance < SlashDistance + SlashSize.y)
                {
                    MoveEnemyTowardsPlayer();
                }

                break;
            case EnemyLogic.MuskeetRanged:

                // Checks If The Enemy Is Looking Att The Player And If The CoolDown Is Off -Lud
                if (CanSlash && Distance > 15 && Distance > 12)
                {
                    StartCoroutine(DelaySlash(AttackDelay));
                }

                Debug.Log("Player Distance: " + Distance);
                // Moves Player So Long As Distance Is Less Then One -Lud
                if (Distance < 12)
                {
                    MoveEnemyAwayFromPlayer();
                }
                else if (Distance > 15)
                {
                    MoveEnemyTowardsPlayer();
                }
                    break;

            default:
                Debug.Log("What The Fuck have you done????");
                break;
        }
    }

    void MoveEnemyTowardsPlayer() // With Lerp Function -Lud
    {
        Vector3 Direction = Player.transform.position - transform.position;
        float Distance = Direction.magnitude;
        Direction = Direction.normalized;

        transform.position += Direction * (Speed * Time.deltaTime);
    }

    void MoveEnemyAwayFromPlayer() // With Lerp Function -Lud
    {
        Vector3 Direction = Player.transform.position - transform.position;
        float Distance = Direction.magnitude;
        Direction = Direction.normalized;

        transform.position -= Direction * (Speed * Time.deltaTime);
    }

    IEnumerator DelaySlash(float delayTime)
    {
        CanSlash = false;
        Vector3 SlashPosition = transform.position + transform.up * SlashDistance;
        GameObject TempSlash = Instantiate(Slash, SlashPosition, transform.rotation);
        TempSlash.transform.localScale = SlashSize;
        TempSlash.tag = "MeleeEnemyAttack";
        yield return new WaitForSeconds(delayTime);
        CanSlash = true;
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
            Health -= Mathf.RoundToInt(PlayerManager.AttackDamage * PlayerManager.RangedDamageMult);
            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}