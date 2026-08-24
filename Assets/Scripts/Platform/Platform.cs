using UnityEngine;

public class Platform : MonoBehaviour
{
    private GameEnvironment environmentcontrol;//Stores large list of collision data
    public EnvironmentData mycoorddata; //grabs collision data object for this platforms y level 
    public Transform transform;
    public int DiffcultyMultiplier; //stores diffculty for other scripts to use
    public bool Active; //any extra behaviour that should only start when on screen
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        environmentcontrol = GameObject.FindFirstObjectByType<GameEnvironment>();//find main enviroment controll
        mycoorddata = environmentcontrol.FindEnvironmentData(transform.position.y);//find the collision data for the y level the platform is on
    }
    public void PlacePlatform(int startlocation, int size)
    {
        startlocation = Mathf.Clamp(startlocation, 0, 16 - size); //make sure start location is capable of holding a platform of that size
        for (int i = 0; i < 16; i++)//loop though all x coords in collision data
        {
            mycoorddata.CollisionData[i]=(i >= startlocation && i < startlocation + size); //set collision to whether or not platform apears in that space
        }
        transform.position = new Vector3((float)startlocation, transform.position.y, 0);//move platform to location
        transform.localScale = new Vector3(size, 1, 1);//scale platform to size
    }
    public void DeleteMe()//remove platform from collsion data and delete object
    {
        environmentcontrol.CollisionTable.Remove(mycoorddata);
        Destroy(gameObject);
    }
}
