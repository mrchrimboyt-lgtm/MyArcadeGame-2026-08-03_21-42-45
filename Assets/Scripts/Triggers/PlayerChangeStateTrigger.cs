using UnityEngine;

public class PlayerChangeStateTrigger : MonoBehaviour
{
    public int newstate; //state to change players too
    public bool AllStates; //tick if you want trigger to change players state no matter what 
    public int IfState;//set if you want trigger to only change players state if player is already set to a state
    public string playsound;//sound effect name to play
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))//if player enters trigger
        {
            GonkPlayer player = other.GetComponent<GonkPlayer>();//grab player
            if(player.State == IfState || AllStates)//if the correct state or allstates is ticked
            {
                player.State = newstate;//set player to new state
                if (playsound != "")//if sound effect name is present
                {
                    player.PlayerSounds.Play(playsound);//play sound effect
                }                    
            }
                
        }
    }
}
