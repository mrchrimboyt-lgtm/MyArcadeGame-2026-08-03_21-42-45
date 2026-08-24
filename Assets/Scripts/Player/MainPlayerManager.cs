using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class MainPlayerManager : MonoBehaviour
{

    //hold list of all player controllers
    public List<PlayerController> PlayerList = new List<PlayerController>();
    //holds list of the colours each player needs to be set to
    public Color32[] PlayerColours;
    private int SceneNum; //stores current scene 0 - menu 1- game 2-scores
    public GameObject[] Scenes;//object containing scenes
    public GameObject GonkPlayerPrefab;
    public AudioManager globalsoundeffects;
    void Start()
    {
        SceneNum = 0;
    }

    public void LoadScoreMenu()//Load Score Scene 
    {
        if (SceneNum != 1) { return; }//only run if in game scene
        Scenes[1].SetActive(false);
        Scenes[2].SetActive(true);
        SceneNum = 2;

    }
    public void StartTheGame()//setup the game
    {
        if (SceneNum != 0) { return; }//only run if in menu
        Scenes[0].SetActive(false);
        Scenes[1].SetActive(true);
        SceneNum = 1;
        for (int i = 0; i < PlayerList.Count; i++)//go through player list
        {
            PlayerList[i].PlayerName = PlayerList[i].menucard.GetNameValue();//grab and store the name the player inputed
            GameObject newGonkPlayer = Instantiate(GonkPlayerPrefab);//spawn new player
            GonkPlayer newplayerscript = newGonkPlayer.GetComponent<GonkPlayer>();
            newplayerscript.playercontroller = PlayerList[i];//set new player to player contoller
            newGonkPlayer.transform.parent = Scenes[1].transform;//move player into game scene
            newGonkPlayer.transform.position = new Vector3((float)i, 5, 0);//set spawnpoint
            newplayerscript.sprites.ClothspriteRenderer.color = PlayerColours[i];//set players colour

        }
    }


}
