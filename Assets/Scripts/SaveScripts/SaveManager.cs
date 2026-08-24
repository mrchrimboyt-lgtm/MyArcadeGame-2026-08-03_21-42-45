using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //this script makes communicating with the SavingScript alot easier
    public List<Score> highscorelist = new List<Score>();
    void Awake()
    {
        LoadGame();//loads data in the beginning
    }
    public void SaveGame()//save game data (call this method when changes have been made)
    {
        SavingScript.SaveTheData(this);
    }
    public void LoadGame() //loading up data previously saved
    {
        try//in case savefile can not be found and load fails
        {
            SaveData data = SavingScript.LoadTheData();
            highscorelist = data.highscorelist;
        }
        catch { }
    }
}
