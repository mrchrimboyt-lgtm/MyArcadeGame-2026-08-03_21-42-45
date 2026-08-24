using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerSpawner : MonoBehaviour
{
    public GameObject PlayerManagerObject;//Main Player Manager Object
    public GameObject[] PlayerCardList;//list of player name cards in the menu
    private MainPlayerManager playerManager; //Main Player Manager script
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerManager=PlayerManagerObject.GetComponent<MainPlayerManager>();//find the manager
    }

    public void OnPlayerJoined(PlayerInput playerInput)//When the Unity input system spawns a player
    {
        playerInput.transform.parent = PlayerManagerObject.transform;//move player controller to the player manager
        PlayerController myplayercontroller = playerInput.GetComponent<PlayerController>(); //get PlayerController script
        playerManager.PlayerList.Add(myplayercontroller);// add controller to player list in manager
        int PlayerCount = playerManager.PlayerList.Count;//grab current player count
        PlayerCardList[PlayerCount - 1].SetActive(true);//activate player name card for new player
        PlayerCard card = PlayerCardList[PlayerCount - 1].GetComponent<PlayerCard>(); //get PlayerCard script
        card.myplayer = myplayercontroller;//link player to playercard
        myplayercontroller.menucard = card;//link playercard to player
        myplayercontroller.playernumber = PlayerCount;//tell player its number
        myplayercontroller.mymanager = playerManager;//tell player its manager
        playerManager.globalsoundeffects.Play("Join");//play sound effect
    }
}
