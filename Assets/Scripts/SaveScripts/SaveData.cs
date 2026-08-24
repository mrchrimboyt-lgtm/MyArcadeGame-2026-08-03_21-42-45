using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class SaveData {
    //This is the data that will be saved in the savefile
    public List<Score> highscorelist = new List<Score>();
    public SaveData(SaveManager savemanager)
    {
        highscorelist = savemanager.highscorelist;
    }
}
