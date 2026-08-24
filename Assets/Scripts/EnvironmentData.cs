using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class EnvironmentData
{    
    //Contains x coord collision infomation
    public float YAxis; //what y coord this info is linked to
    public bool[] CollisionData = new bool[16]; // the collision status of the Xcoords within the Ycoord

    public EnvironmentData(float yAxis, bool[] collisionData)//create new
    {
        YAxis = yAxis;
        CollisionData = collisionData;
    }

    public bool ReturnValue(int index)//trys to return collision value. Returns false if out of range.
    {
        try {return CollisionData[index];}
        catch { return false; }
    }
}
