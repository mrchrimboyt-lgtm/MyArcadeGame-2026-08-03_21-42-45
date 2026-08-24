using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Holds Input and individual player info
    public int playernumber; // my player number
    public MainPlayerManager mymanager;// the main manager
    public string PlayerName;//the player name
    public int Score;
    public InputAction Movementaction;
    public InputAction JumpAction;
    public InputAction StartAction;
    public PlayerCard menucard;//the main menu name card
    
    private void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();//when start get all my inputs
        Movementaction = playerInput.actions.FindAction("Move");
        StartAction = playerInput.actions.FindAction("Start");
        JumpAction = playerInput.actions.FindAction("Jump");
        StartAction.Enable();
        JumpAction.Enable();
        StartAction.started += TestStart;//check for start button press
    }

    private void OnDisable()
    {
        StartAction.started -= TestStart;
    }

    private void TestStart(InputAction.CallbackContext context)
    {
        if (playernumber == 1) { mymanager.StartTheGame(); } //if start is pressed and the player is player 1, start the game
    }
    
   
}

