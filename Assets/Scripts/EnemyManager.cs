using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public int Health;
    public int MaxHealth;
    public float Speed;

    public GameObject Player;

    // Enemy Enum Holders -Lud
    public EnemyIntellagence Int = EnemyIntellagence.None;
    public EnemyResistances Res = EnemyResistances.None;

    public enum EnemyIntellagence
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

    private void Update() // Switches To The Right Int Then Uses It To Move
    {
        switch (Int)
        {
            case EnemyIntellagence.None:
                Debug.Log("Value Missing");
                break;
            case EnemyIntellagence.TankMelee:

                Vector3 Diffrence = Player.transform.position - transform.position;
                Diffrence.Normalize();
                float RotaitionZ = Mathf.Atan2(Diffrence.y, Diffrence.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, RotaitionZ - 90);
                transform.position = transform.position + transform.up * Speed * Time.deltaTime;

                break;
            default:
                break;
        }
    }
}
