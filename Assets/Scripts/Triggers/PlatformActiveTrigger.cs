using UnityEngine;

public class PlatformActiveTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("platform"))//once a platform enters this trigger, activate platform
        {
            Platform platform = other.GetComponent<Platform>();
            platform.Active = true; //this isnt really used much currently. The idea was for platforms with funtionality such as movement and enemies to be active only when coming on screen.
        }
    }
}
