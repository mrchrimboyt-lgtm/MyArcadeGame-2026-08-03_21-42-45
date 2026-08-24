using UnityEngine;

public class DamagePlayerTrigger : MonoBehaviour
{
    public float DamageTimer;//the time in seconds the player falls
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))//if a player enters this, damage him for set amount of time
        {
            GonkPlayer player = other.GetComponent<GonkPlayer>();
            player.DamagePlayer(DamageTimer);

        }
    }
}
