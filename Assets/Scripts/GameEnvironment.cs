using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEnvironment : MonoBehaviour
{
    public List<GonkPlayer> Players = new List<GonkPlayer>();//Contains List Of All Players for location reasons. Was planning to do more with this

    public List<EnvironmentData> CollisionTable;//contains list of where solid coords are located

    public Transform cameray; //The Camera
    
    
    private float TopHeight; //The Top Height currently reached by a player

    public GameObject PlatformDeleteTrigger; //Contains large trigger that deletes lower platforms not in use. (Gets activated when player reaches certain height)

    public Transform PlatformSpawnPoint; //Stores Location of where plateforms should be spawned
    public PlatformManager platformspawner; //link to plateform spawn script
    public int DeathCounter;//keeps track of how many players have died
    public MainPlayerManager playerManager; //link to main player manager.


    public void PlayerDied()//Player calls this on death
    {
        DeathCounter++;//Keeps track of death
        if(DeathCounter == Players.Count)//if all players have died
        {
            cameray.position = new Vector3(8, 8, -10);//reset the camera
            playerManager.LoadScoreMenu();//load the score menu
        }
    }

    public EnvironmentData FindEnvironmentData(float y)//finds EnvironmentData class for y level
    {
        for (int i = 0; i < CollisionTable.Count; i++)//loops through list
        {
            if (CollisionTable[i].YAxis == y)//returns EnvironmentData if correct y level is found
            {
                return CollisionTable[i];
            }
        }
        return null;//returns null if not in list
    }

    public bool ReturnValue(float[] coord)//returns collsion value of a coord
    {
        EnvironmentData xinfo = FindEnvironmentData(coord[1]); //finds EnvironmentData class belonging to the y level
        if (xinfo!=null) {return xinfo.ReturnValue((int)coord[0]); }    //if found than return the collsion value of the x coord
        return false;//if nothing found return false
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        CollisionTable = new List<EnvironmentData>{
        new EnvironmentData(0f,new bool[16]{true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true}),
        };//load a solid ground into the enviroment at y level 0
    }

    // Update is called once per frame
    void Update()
    {
        //makes camera follow the highest point the player reached.
        float HighestPlayerHeight = 0; //stores what is current highest point a player has reached right now
        for (int i = 0;i < Players.Count; i++)//loop through all players
        {
            if (Players[i].TopHeight > TopHeight && Players[i].State < 4) { TopHeight = Players[i].TopHeight; } //if there height is higher than the top height than set top height to player unless they are dead
            if(Players[i].transform.position.y > HighestPlayerHeight && Players[i].State < 4) { HighestPlayerHeight=Players[i].transform.position.y;//if there height is higher than the HighestPlayerHeight than set HighestPlayerHeight to player unless they are dead }
            }
        if (TopHeight > 32)//if the player reaches y level 32 than the camera will stick to the heighest height they have reached. If lower the camera will follow the current highest player.
        {
            cameray.position = new Vector3(8, TopHeight, -10);
        }
        else { cameray.position = new Vector3(8, Mathf.Clamp(HighestPlayerHeight, 8, 32), -10); }
        PlatformDeleteTrigger.SetActive(TopHeight > 32);//activate plateform delete trigger when the camera stops moving down
        if (TopHeight + 12 > PlatformSpawnPoint.position.y)//Request a new platform when PlatformSpawnPoint has increased
            platformspawner.RequestNewPlatform((int)(TopHeight + 12));
            PlatformSpawnPoint.position = new Vector3(0, TopHeight + 12, 0);
        }
    }
}
