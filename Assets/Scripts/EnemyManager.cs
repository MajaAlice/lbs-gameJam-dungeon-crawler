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

    GameObject Slash;
    public bool CanSlash = true;
    public float SlashDelay;
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
            case EnemyLogic.TankMelee:

                // Checks If The Enemy Is Looking Att The Player And If The CoolDown Is Off -Lud
                if ((Mathf.Abs(transform.rotation.z - (RotationZ - 90)) < 75) && CanSlash)
                {
                    StartCoroutine(DelaySlash(SlashDelay));
                }
                // Moves Player So Long As Distance Is Less Then One -Lud
                if(Distance > 1)
                {
                    MoveEnemyTowardsPlayer();
                }

                break;
            case EnemyLogic.MuskeetRanged:
                break;
            default:
                break;
        }
        //if (PlayerManager.HitInfo == gameObject)
        //{
        //    Health -= Mathf.RoundToInt(PlayerManager.AttackDamage * PlayerManager.RangedDamageMult);
        //}
    }

    void MoveEnemyTowardsPlayer()
    {
        transform.position = Vector3.Lerp(Player.transform.position, transform.position, Speed * Time.deltaTime);

    }

    IEnumerator DelaySlash(float delayTime)
    {
        Vector3 SlashPosition = transform.position + transform.up * SlashDistance;
        GameObject TempSlash = Instantiate(Slash, SlashPosition, transform.rotation);
        TempSlash.transform.localScale = SlashSize;
        CanSlash = false;
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
            Health -= PlayerManager.AttackDamage;
        }
    }
}
