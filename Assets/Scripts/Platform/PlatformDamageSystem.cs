using UnityEngine;

public class PlatformDamageSystem : MonoBehaviour
{
    private bool DamagingState; //whether the platform is in damage mode or not
    private float MaxStateTimer; //how long it waits inbetween state changes
    private float StateTimer;//timer for when the state changes
    public SpriteRenderer spriteRenderer; //the sprite
    public GameObject DamageTrigger; //the trigger which cause damage to the player
    public Platform platform; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DamagingState = false;
        MaxStateTimer = Random.Range(3, 10);//random diffculty of platform
        StateTimer = MaxStateTimer;
    }

    // Update is called once per frame
    void Update()
    {
        StateTimer-=Time.deltaTime;
        if(StateTimer < 0 && platform.Active)//everytime timer runs out change the state
        {
            StateTimer = MaxStateTimer;
            DamagingState = !DamagingState;
            DamageTrigger.SetActive(DamagingState);
            if (DamagingState) //damage colour
            {
                spriteRenderer.color = new Color32(255, 0, 0, 255);
            }
            else
            { //Normal colour
                spriteRenderer.color = new Color32(56, 38, 9, 255);
            }
        }
    }
}
