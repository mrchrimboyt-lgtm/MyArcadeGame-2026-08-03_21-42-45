using UnityEngine;

public class PlateformDeleteTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("platform"))//when platform enters, run delete script
        {
            Platform platform = other.GetComponent<Platform>();
            platform.DeleteMe();
        }
    }
}
