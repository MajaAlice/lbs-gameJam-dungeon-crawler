using UnityEngine;

public class CameraManager : MonoBehaviour
{

   public GameObject Player;

    float LerpSpeed = 1;
    void LateUpdate() // Lerps The Camera Based On The Player -Lud
    {
        Vector3 PlayerCamPostion = new Vector3(Player.transform.position.x, Player.transform.position.y, 10);
        gameObject.transform.Translate(Vector3.Lerp(gameObject.transform.position, PlayerCamPostion, LerpSpeed * Time.deltaTime));
    }
}
