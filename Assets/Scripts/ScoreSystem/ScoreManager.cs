using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{

    public MainPlayerManager players; //main player manager
    public GameObject ScoreObject; //Holds text for game scores
    public GameObject HighscoreObject;//Holds text for highscores
    public SaveManager savesystem;
    private TextMeshPro Score;
    private TextMeshPro Highscore;
    private List<Score> gamescorelist = new List<Score>();//List of game scores

    private float Timer;//animation timer for when to switch scoreboards
    private float ResetTimer;//timer for when to reset game
    private void SetScore()
    {
        for (int i = 0; i < players.PlayerList.Count; i++)//Go though all players
        {
            gamescorelist.Add(new Score(players.PlayerList[i].Score, players.PlayerList[i].PlayerName));//add  their score and name to the list
        }
        gamescorelist.Sort((a, b) => b.ScoreValue.CompareTo(a.ScoreValue));//order list

        WriteScoreList(Score, gamescorelist, "Scores");//writes score data to textmesh
        //highscore
        savesystem.LoadGame();//loads highscore data
        for(int i = 0;i < gamescorelist.Count; i++)//loops through new scores
        {
            int index = savesystem.highscorelist.FindIndex(x => x.PlayerName == gamescorelist[i].PlayerName);//checks if player name already appears
            if (index != -1)
            {
                if (gamescorelist[i].ScoreValue > savesystem.highscorelist[index].ScoreValue)//if players score is larger than before. Set players score
                {
                    savesystem.highscorelist[index].ScoreValue = gamescorelist[i].ScoreValue;
                }
            }
            else{savesystem.highscorelist.Add(gamescorelist[i]); }//if new player add their score to the list
        }
        savesystem.highscorelist.Sort((a, b) => b.ScoreValue.CompareTo(a.ScoreValue));//sort highscore list
        WriteScoreList(Highscore, savesystem.highscorelist, "HighScores");//write highscores to textmesh
        savesystem.SaveGame();//save new highscore list
    }
    private void WriteScoreList(TextMeshPro textobject, List<Score> list,string Title)//Writes scores to a textmesh
    {
        string output = Title + "\n\n";//Start with a title
        for (int i = 0; i < list.Count; i++) //go through each score in list
        {
            output= output + list[i].PlayerName + ": " + (list[i].ScoreValue).ToString() + "\n"; // write score to string
        }
        textobject.text = output; //write output string to TextMesh
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Score=ScoreObject.GetComponent<TextMeshPro>();//find textmesh components
        Highscore=HighscoreObject.GetComponent<TextMeshPro>();
        SetScore();//Find and set the scores
        Timer = 3;//change score board every 3 seconds
        ResetTimer = 10;//reset game after 10 secs
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        ResetTimer -= Time.deltaTime;//move timers
        if (Timer < 0)//change the scoreboard that is shown
        {
            Timer = 3;
            ScoreObject.SetActive(!ScoreObject.activeSelf);
            HighscoreObject.SetActive(!ScoreObject.activeSelf);
        }
        if (ResetTimer < 0) { SceneManager.LoadScene("MainGame"); }//reset the game
    }
}
