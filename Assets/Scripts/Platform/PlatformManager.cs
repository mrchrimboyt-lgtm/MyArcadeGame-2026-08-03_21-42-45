using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlatformManager : MonoBehaviour
{
    public GameEnvironment gameenvironment; //link to collision data
    public List<PlatformType> platformtypes = new List<PlatformType>(); //list of platform types
    private int SpaceUntilNextPlatform; //how many spaces until the next platform can be spawned

    private int xpoint; //the x position platforms spawn around
    private int xmovedir;//the dir xpoint moves towards
    public int xspread;//how far away platforms can spawn around xpoint

    private int GenerateRandomXpos()//generate a x axis pos for the platform
    {
        int min = Mathf.Clamp(xpoint - xspread, 0, 14); //find the minium x spawn value
        int max = Mathf.Clamp(xpoint + xspread, 1, 15);//find the maxium x spawn value
        xpoint += xmovedir;//move x in direction for next GenerateRandomXpos()
        xpoint = Mathf.Clamp(xpoint, 0, 15);//make sure xpoint is in game boundaries
        if (xpoint == 0 || xpoint == 15)//change direction if xpoint is on edge
        {
            xmovedir = 0 - xmovedir;
        }
        return Random.Range(min, max+1);//random x between min and max
    }

    private void SpawnNewPlatform(PlatformType platformtype, int ylevel)//spawn new plateform
    {
        gameenvironment.CollisionTable.Add(new EnvironmentData((float)ylevel, new bool[16] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false }));//create fresh collsion data
        Platform newplatform = Instantiate(platformtype.PlatformObject, new Vector3(0, ylevel, 0), Quaternion.identity, gameObject.transform).GetComponent<Platform>(); //create new platform object
        newplatform.PlacePlatform(GenerateRandomXpos(), Random.Range(platformtype.minsize, platformtype.maxsize + 1)); //set platform up
        newplatform.DiffcultyMultiplier = platformtype.diffcultylevel; //set diffculty which doesnt get used atm
        SpaceUntilNextPlatform = Random.Range(2, 5);//spaces until another platform can be placed

    }

    public void RequestNewPlatform(int ylevel)//platform request
    {
        if (SpaceUntilNextPlatform <= 0)//if it has placed enough space between the last platform
        {
            List<PlatformType> AvailablePlatforms = new List<PlatformType>();//create a list to contain all valid platform types
            for (int i = 0; i < platformtypes.Count; i++)//loop though all platform types
            {
                if (ylevel > platformtypes[i].diffcultylevel && platformtypes[i].SpawnCounter <= 0)//if platform types spawn conditions are valid
                {
                    AvailablePlatforms.Add(platformtypes[i]);//add them to the list
                }
                platformtypes[i].SpawnCounter = Mathf.Max(0, platformtypes[0].SpawnCounter - 1);//reduce the interal SpawnCounter in each platform class
            }
            PlatformType chosenplatform = AvailablePlatforms[Random.Range(0, AvailablePlatforms.Count)]; //choose a random platform        
            chosenplatform.SpawnCounter = Random.Range(chosenplatform.MinSpawnCounter, chosenplatform.MaxSpawnCounter + 1);//set spawncounter on chosen platform

            SpawnNewPlatform(chosenplatform, ylevel);//spawn chosen platform
        }
        else { SpaceUntilNextPlatform--; }//if not enough space has been granted then dont spawn platform and reduce SpaceUntilNextPlatform
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xpoint = 15;
        xmovedir = -1;
        SpaceUntilNextPlatform=3;
        int[] startcoord = new int[4] {4,7,13,17};//starting platform locations
        for(int i = 0; i < 4; i++)
        {
            SpawnNewPlatform(platformtypes[0], startcoord[i]);
        }
        

    }
}
