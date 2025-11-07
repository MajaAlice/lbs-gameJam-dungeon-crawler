using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerSlash;
    public GameObject Dash;
    public GameObject PlayerBullet;
    public GameObject FUCK;

    // Player Values -Lud
    public int Health = 7; // Base 7
    public byte Sake = 0;
    public byte MaxSake = 6;
    public byte Blood = 0; // Max 9
    public byte AttackDamage = 1;
    public float SlashDistance = 1;
    public float SlashDelay = 0.5f;
    public float DashDelay = 1f;
    public float MeleeDamageMult = 1;
    public float RangedDamageMult = 1;
    public float OnHitIFrameDuration = 0.1f;
    public float DashIframeDuration = 0.1f;
    public float HealingDuration = 2f;

    // Fuck
    public float CamFuckDist = 1.5f;

    public TMP_Text HealthText;
    public TMP_Text SakeText;
    public TMP_Text BloodText;
    public TMP_Text BulletText;

    //These variables are controlled by the Player Slash script
    public int MagSize = 6;
    public int CurrentMag = 6;
    public int MaxBulletGrab = 4;
    public int BulletsGrabbed = 0;

    // Speed Values -Lud
    public float PlayerSpeed = 10;
    public float CameraSpeed = 5;
    public float DashLenght = 1;
    public Vector3 SlashSize = new Vector3(2,2,1);

    public float BulletSpeed = 10;

    // Bools -Lud
    bool CanDash = true;
    bool CanSlash = true;
    bool CanBeHurt = true;
    bool IsInteracting = false; //Controlls interacts and certain player functions like healing and sake creation - Maja

    // Movement Keys -Lud
    [SerializeField] KeyCode Up = KeyCode.W;
    [SerializeField] KeyCode Down = KeyCode.S;
    [SerializeField] KeyCode Right = KeyCode.D;
    [SerializeField] KeyCode Left = KeyCode.A;
    // Attacks Keys -Lud
    [SerializeField] KeyCode dash = KeyCode.LeftShift;
    [SerializeField] KeyCode Slash = KeyCode.Mouse0;
    [SerializeField] KeyCode Aim = KeyCode.Mouse1;
    //Misc Keybinds
    [SerializeField] KeyCode Heal = KeyCode.F;
    [SerializeField] KeyCode MakeSake = KeyCode.E;

    private void Start()
    {
        FUCK = GameObject.FindGameObjectWithTag("Fuck");
    }
    void Update()
    {
        // Player Rotation To Mouse -Lud
        Vector3 Diffrence = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Diffrence.Normalize();
        float RotationZ = Mathf.Atan2(Diffrence.y, Diffrence.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, RotationZ - 90);

        // Player Movement -Lud
        Vector2 MovementVector = Vector2.zero;
        if (Input.GetKey(Up) && IsInteracting == false) { MovementVector += Vector2.up; }
        if (Input.GetKey(Down) && IsInteracting == false) { MovementVector += Vector2.down; }
        if (Input.GetKey(Right) && IsInteracting == false) { MovementVector += Vector2.right; }
        if (Input.GetKey(Left) && IsInteracting == false) { MovementVector += Vector2.left; }

        MovementVector = MovementVector.normalized * (PlayerSpeed * Time.deltaTime);
        gameObject.transform.position += new Vector3(MovementVector.x, MovementVector.y, 0);
           
        //Dash - Maja
        if (Input.GetKeyDown(dash) && CanDash && IsInteracting == false)
        {
            StartCoroutine(IFrames(DashIframeDuration));
            StartCoroutine(DelayDash(DashDelay));
        }

        

        // Switches Player into aim mode - Maja
        if (Input.GetKey(Aim) && IsInteracting == false)
        {
            if (Input.GetKeyDown(Slash) && CurrentMag > 0)
            {
                Instantiate(PlayerBullet, transform.position, transform.rotation);
                CurrentMag--;
            }
            
        }
        else if (Input.GetKeyDown(Slash) && CanSlash && IsInteracting == false)
        {
            StartCoroutine(DelaySlash(SlashDelay));
        }

        //healing  -Maja
        if (Input.GetKeyDown(Heal) && Sake >= 1 && IsInteracting == false)
        {
            StartCoroutine(Healing(HealingDuration));
        }
        //Making Sake - Maja
        if (Input.GetKeyDown(MakeSake) && Blood >= 3 && (Sake < MaxSake) &&  IsInteracting == false)
        {
            StartCoroutine(CreateSake(HealingDuration));
        }
        //death - Maja
        if (Health <= 0) StartCoroutine(Death(1f));

        HealthText.text = "Health: " + Health.ToString();
        SakeText.text = "Sake: " + Sake.ToString();
        BloodText.text = "Blood: " + Blood.ToString();
        BulletText.text = "Ammo: " + CurrentMag.ToString(); 
    }

    void LateUpdate() // Lerps The Camera Based On The Player -Lud
    {


        Vector3 Direction = FUCK.transform.position - transform.position;
        float Distance = Direction.magnitude;
        Direction = Direction.normalized;

        Camera.main.transform.position = Vector3.Lerp( Camera.main.transform.position, new Vector3(transform.position.x + (Direction.x * CamFuckDist), transform.position.y + (Direction.y * CamFuckDist), -10), Time.deltaTime * CameraSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Takes Damage and enables Iframes
        if (collision.CompareTag("MeleeEnemyAttack") && CanBeHurt || collision.CompareTag("RangedEnemyAttack") && CanBeHurt)
        {
            Health--;
            StartCoroutine(IFrames(OnHitIFrameDuration));
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
    IEnumerator DelaySlash(float delayTime)                     //handles the slash and it's cooldown and parry reloading -Maja
    {
        Vector3 SlashPosition = transform.position + transform.up * SlashDistance;
        GameObject TempSlash = Instantiate(PlayerSlash, SlashPosition, transform.rotation);
        TempSlash.transform.localScale = SlashSize;
        CanSlash = false;
        yield return new WaitForSeconds(delayTime);
        CanSlash = true;
        CurrentMag += BulletsGrabbed;
        BulletsGrabbed = 0;
        if (CurrentMag > MagSize) { CurrentMag = MagSize; }
    }
    IEnumerator IFrames (float delayTime)                     //momenterily makes the player invincible - Maja
    {
        CanBeHurt = false;
        yield return new WaitForSeconds(delayTime);
        CanBeHurt = true;
    }
    IEnumerator Healing(float delayTime)                     //stops all actions and adds health - Maja
    {
        IsInteracting = true;
        Sake--;
        yield return new WaitForSeconds(delayTime);
        Health++;
        IsInteracting = false;
    }
    IEnumerator CreateSake(float delayTime)                     //stops all actions and adds sake - Maja
    {
        IsInteracting = true;
        Blood -= 3;
        yield return new WaitForSeconds(delayTime);
        Sake++;
        IsInteracting = false;
    }
    IEnumerator Death(float delayTime)                     //Dies inside looking at this code :lesanae: - Maja
    {
        IsInteracting = true;
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(false);
    }
}

