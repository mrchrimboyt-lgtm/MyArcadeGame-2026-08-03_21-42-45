using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerCard : MonoBehaviour
{
    //Menu card for each player
    public PlayerController myplayer;//link to the contoller
    public TextMeshPro[] Digits;//Each of the digits in the 3 letters
    private int DigitSelect;//which digit the selection is on
    private float UpdateTimer;//how quickly inputs effect selection
    private bool animationtracker;//setting for text animation
    void Start()
    {
        DigitSelect = 0;
        UpdateTimer = 0.2f;
    }

    private void ChangeDigit(int digitnum,int changeby)//changes the digit.
    {
        changeby = 0 - changeby;//inverses the input just cause
        char letter = Digits[digitnum].text[0];//grabs current digit
        if (letter == 'Z' && changeby == 1) { letter = 'A'; }//if moving past Z than change to A vise versa 
        else if (letter == 'A' && changeby == -1) { letter = 'Z'; }
        else
        {
            letter += (char)changeby;//if not at the start or end of the alphabet then change digit
        }
        Digits[digitnum].text=letter.ToString();
    }
    public string GetNameValue()//combinds all digits to make name
    {
        string output = Digits[0].text[0].ToString() + Digits[1].text[0].ToString() + Digits[2].text[0].ToString();//combinds digits
        if (output == "DER") { output = "DerpTree"; }//easter egg
        return output;
    }
    void Update()
    {
        UpdateTimer -= Time.deltaTime;
        if( UpdateTimer < 0)//every 0.2secs
        {
            UpdateTimer = 0.2f;
            animationtracker = !animationtracker;//change the animation
            Vector2 rawstickinput = myplayer.Movementaction.ReadValue<Vector2>();//get the stick input
            Vector2 stickinput = new Vector2((float)Mathf.RoundToInt(rawstickinput.x), (float)Mathf.RoundToInt(rawstickinput.y));//round it to nearest whole num
            if ((int)stickinput.x != 0) { //if movement on the x
                DigitSelect += (int)stickinput.x; //increase the digit your selecting by direction
                DigitSelect = Mathf.Clamp(DigitSelect, 0, 2);//dont let it slip out of range
                animationtracker = true;//reset animation
            }
            else if ((int)stickinput.y != 0)//if movement on the y and none on x
            {
                ChangeDigit(DigitSelect, (int)stickinput.y); //change digit
            }
            for (int i = 0; i < 3; i++) //go though all digits and set fontstyle based on animation and selection
            {
                Digits[i].fontStyle = (i == DigitSelect && animationtracker) ? FontStyles.Italic : FontStyles.Normal;
            }

        }
    }
}
