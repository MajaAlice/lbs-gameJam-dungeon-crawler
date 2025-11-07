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
    //Animator EnemyAnimator;

    public GameObject Slash;
    public GameObject Bullet;
    public bool CanSlash = true;
    public bool CanShoot = true;
    public float AttackDelay;
    public Vector3 SlashSize;
    public float SlashDistance;
    public int CurrentRoom = 0; //current room the player is in
    public int EnemyRoomNr = 0; //which room the enemy belongs to - Maja
  

    bool IsDying = false;

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
        //EnemyAnimator = gameObject.GetComponent<Animator>();
    }
    private void Update() // Switches To The Right Int Then Uses It To Move
    {
        if(CurrentRoom == EnemyRoomNr)
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
                    if (CanSlash && Distance < SlashDistance && IsDying == false)
                    {
                        StartCoroutine(DelaySlash(AttackDelay));
                    }
                    // Moves Player So Long As Distance Is Less Then One -Lud
                    if (Distance > SlashDistance && IsDying == false)
                    {
                        MoveEnemyTowardsPlayer();
                    }

                    break;
                case EnemyLogic.TankMelee:

                    // Checks If The Enemy Is Looking Att The Player And If The CoolDown Is Off -Lud
                    if (CanSlash && Distance < SlashDistance + SlashSize.y && IsDying == false)
                    {
                        StartCoroutine(DelaySlash(AttackDelay));
                    }
                    // Moves Player So Long As Distance Is Less Then One -Lud
                    if (Distance > SlashDistance && IsDying == false)
                    {
                        MoveEnemyTowardsPlayer();
                    }

                    break;
                case EnemyLogic.MuskeetRanged:

                    // Checks If The Enemy Is Looking Att The Player And If The CoolDown Is Off -Lud
                    if (CanShoot && Distance < 8 && Distance > 5 && IsDying == false)
                    {
                        StartCoroutine(DelayShot(AttackDelay));
                    }
                    // Moves Player So Long As Distance Is Less Then One -Lud
                    if (Distance < 5 && IsDying == false)
                    {
                        MoveEnemyAwayFromPlayer();
                    }
                    else if (Distance > 8 && IsDying == false)
                    {
                        MoveEnemyTowardsPlayer();
                    }
                    break;

                default:
                    Debug.Log("What The Fuck have you done????");
                    break;
            }
        }

        //AnimatorBools();
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
    IEnumerator DelayShot(float delayTime)
    {
        CanShoot = false;
        Vector3 SlashPosition = transform.position + transform.up * SlashDistance;
        GameObject TempSlash = Instantiate(Bullet, SlashPosition, transform.rotation);
        TempSlash.transform.localScale = SlashSize;
        TempSlash.tag = "RangedEnemyAttack";
        yield return new WaitForSeconds(delayTime);
        CanShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MeleePlayerAttack"))
        {
            
            Health -= Mathf.RoundToInt(PlayerManager.AttackDamage * PlayerManager.MeleeDamageMult);
            if(PlayerManager.Blood != 9) PlayerManager.Blood++;
            //EnemyAnimator.SetBool("Hit", true);

            if (Health <= 0)
            {
                IsDying = true;
                //EnemyAnimator.SetBool("Death", true);
                Destroy(gameObject, 1f);
            }
        }
        else if (collision.CompareTag("RangedPlayerAttack"))
        {
            Health -= Mathf.RoundToInt(PlayerManager.AttackDamage * PlayerManager.RangedDamageMult);
            if (Health <= 0)
            {
                IsDying = true;
                //EnemyAnimator.SetBool("Death", true);
                Destroy(gameObject, 0f);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //EnemyAnimator.SetBool("Hit", false);
    }

    //void AnimatorBools()
    //{
    //    EnemyAnimator.SetBool("Attack", !CanSlash);
    //    EnemyAnimator.SetBool("Hit", false);
    //    EnemyAnimator.SetBool("Death", false);
    //}
}