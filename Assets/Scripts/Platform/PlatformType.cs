using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class PlatformType
{
    public GameObject PlatformObject;

    //size
    [Range(1, 10)]
    public int maxsize;
    [Range(1, 10)]
    public int minsize;

    //spawn rules
    public int MaxSpawnCounter;//how many platforms need to spawn in before it can spawn a new one of this type in
    public int MinSpawnCounter;//how many platforms need to spawn in before it can spawn a new one of this type in
    public int SpawnCounter;

    //diffculty
    public int diffcultylevel;//how far up the player must reach before this can spawn

}
