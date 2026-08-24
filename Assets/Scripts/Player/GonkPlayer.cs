using UnityEngine;
using UnityEngine.InputSystem;

public class GonkPlayer : MonoBehaviour
{
    private struct StateSettings//stores settings for each state
    {
        public float Gravity;//whether gravity is up down or off
        public bool JumpActive;//if player can jump
        public bool SideMotion;//if player can move on the x axis
        public bool PlatformCollision;//if player is effected by platform collision
    }


    public AudioManager PlayerSounds;//Holds player sound effects
    public int State;//stores the current state of the player
    private int prevstate; //hold state value for detecting changes in state
    //List of states
    private StateSettings[] PlayerStateSettings = new StateSettings[5] {
        new StateSettings{Gravity = -1f,JumpActive = true,SideMotion = true,PlatformCollision = true},//Normal Motion = 0 
        new StateSettings { Gravity = 0f, JumpActive = false, SideMotion = false, PlatformCollision = false },//Grab = 1 
        new StateSettings { Gravity = -2f, JumpActive = false, SideMotion = true, PlatformCollision = false },//Falling = 2 
        new StateSettings { Gravity = 1f, JumpActive = false, SideMotion = true, PlatformCollision = false },//Balloon = 3
        new StateSettings { Gravity = 0f, JumpActive = false, SideMotion = false, PlatformCollision = false }//Dead = 4
    };

    private GameEnvironment environmentcontrol;//holds collision data
    public Transform transform; //my transform

    public float JumpHeight;
    public float TicSpeed;//how fast the player updates movement
    private float Tic;
    private Vector2 velocity;

    public PlayerController playercontroller;//holds player inputs

    //State Timers
    private float GrabTime;//how long the player grabs for
    private float FallTime;//how long the player falls for

    //score
    public float TopHeight;//the top height this player reached
    public float Timer;//how long the player has been going for

    private int Lives; //How many balloons the player has


    public PlayerSprites sprites;//link to script that controlls player animations
    private int SpriteYdir;//stores looking direction for that sprite script
    private int SpriteXdir;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        environmentcontrol = GameObject.FindFirstObjectByType<GameEnvironment>();//finds game enviroment script
        environmentcontrol.Players.Add(this);//adds itself to the scripts list of players
        State = 0; //normal state
        velocity = new Vector2 (0,0);//no velocity
        Tic = TicSpeed;
        Lives = 3;
        playercontroller.JumpAction.started += Jump;//start detecting the jump input
    }

    public void SnapToCoord(float[] Coord)//Snap Player To New Coord
    {
        Coord[0] = Mathf.Clamp(Coord[0], 0, 15);//Keep Player In Play Area
        if (Coord[1] <= 0)//prevents player from going past ground limit
        {
            Coord[1] = 1;
            State = 0;//normal state if on ground
        }
        else if (Coord[1] > TopHeight) { TopHeight = Coord[1]; } //increases score
        transform.position = new Vector3(Coord[0], Coord[1], 0);//sets to coord
        sprites.UpdateSprite(State,SpriteXdir,SpriteYdir,-2);//updates sprite
    }

    private void Move()//Move One Space
    {
        float velocityX = Mathf.Clamp(velocity.x, -1, 1);//clamp velocity to one space at a time
        float velocityY = Mathf.Clamp(velocity.y, -1, 1);
        float[] Coord = new float[2] { transform.position.x + velocityX, transform.position.y + velocityY };//get new coord for player to move to
        if (State != 1 && velocityX != 0) { SpriteXdir = (int)velocityX; }//set SpriteX direction for visuals to be correct
        SpriteYdir = (int)velocityY; //set SpriteY direction for visuals to be correct
        if (velocityX != 0 || velocityY != 0) { SnapToCoord(Coord); }//if movement than move the player
        velocity.x = Mathf.MoveTowards(velocity.x, 0, 1);//change the velocity by 1 towards 0
        velocity.y = Mathf.MoveTowards(velocity.y, 0, 1);
    }

    private bool CheckNearCoord(float x, float y)//check a coords collision near the player
    {
        return environmentcontrol.ReturnValue(new float[2] { transform.position.x + x, transform.position.y + y});
    }

    private void MovePlayerToPlateform()//Move player to safe location once finished grabbing
    {
        for (int x = -1; x < 2; x++) //check x coords left center right to player
        {
            if (CheckNearCoord(x, 0))//if true than coord is a plateform and player can be placed above it
            {
                SpriteXdir = 1;
                SnapToCoord(new float[] { transform.position.x + x, transform.position.y + 1 });
                break;
            }
        }
        State = 0;//set to normal movement
    }

    public void DamagePlayer(float timerset)//called by triggers that damage player
    {
        if(State < 2) { //if State is normal or grab then make player fall
            FallTime = timerset;
            State = 2;
            PlayerSounds.Play("Damage");
        }
    }



    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        //Perform This Code When State Changes
        if (prevstate != State)
        {
            prevstate = State;
            velocity = new Vector2(0, 0);//Reset Velocity
            GrabTime = 1;//sets a grab timer
            sprites.UpdateSprite(State, SpriteXdir, SpriteYdir, -2);//updates visuals 
            if(State == 3)//if in balloon mode 
            {
                Lives--;//take a life
                if (Lives <= 0) //Death
                {
                    State = 4;
                    environmentcontrol.PlayerDied();
                    playercontroller.Score = (int)((TopHeight / (Timer/60)) * 10000);//calc score
                    playercontroller.mymanager.globalsoundeffects.Play("Death");
                    gameObject.SetActive(false);
                }//no more lives kill players
                else { PlayerSounds.Play("BalloonSpawn"); }
            }
        }

        //Runs Every Movement Tic
        Tic -= Time.deltaTime;
        if (Tic < 0)
        { //Move Every Tic
            Tic = TicSpeed;
            Move();
        }

        //state specfic commands
        switch (State)
        {
            case 1: //grab
                GrabTime -= Time.deltaTime;//reduce grab timers
                if (GrabTime <= 0) { MovePlayerToPlateform(); }//once finished move player to platform
                break;
            case 2://fall
                FallTime -= Time.deltaTime;//reduce fall timers
                if (FallTime <= 0) { State = 0; }//once finished make state normal
                break;
        }

        //Calc Velocity and Collision
        if (velocity.y == 0) { velocity = new Vector2(velocity.x, PlayerStateSettings[State].Gravity); } //set gravity onto velocity.y if velocity.y isnt in motion
        if (PlayerStateSettings[State].PlatformCollision)//if collision is on
        {
            bool[] checksurroundings = new bool[3] { CheckNearCoord(0, 0), CheckNearCoord(-1, 0), CheckNearCoord(1, 0) };//check the coord left center and right to the player
            if (checksurroundings[0] || checksurroundings[1] || checksurroundings[2] )//if any are true than grab onto that coord
            { 
                State = 1;//grab state
                int[] animationnumbers = new int[3] {0,-1, 1 };//numbers to set animation varible to. I would of linked it to the index but I want it to check middle first
                for (int i = 0; i < 3; i++)
                {
                    if (checksurroundings[i])
                    {
                        sprites.UpdateSprite(State, SpriteXdir, SpriteYdir, animationnumbers[i]);//Sprite animation needs to know what direction the player is grabbing
                        break;
                    }
                }
            }//Grab if next to solid block
            else if (velocity.y < 0 && CheckNearCoord(0, -1)) { velocity = new Vector2(velocity.x, 0); }//stop falling if a solid block is below
        }
        if (PlayerStateSettings[State].SideMotion)//if side motion is active
        {
            Vector2 MovementInput = playercontroller.Movementaction.ReadValue<Vector2>(); //set movement input to velocity
            velocity = new Vector2(Mathf.Round(MovementInput.x), velocity.y);
        }



    }


    //Jump
    //private void OnEnable() {  }
    private void OnDisable() { playercontroller.JumpAction.started -= Jump; }
    private void Jump(InputAction.CallbackContext obj)//when jump button is pressed
    {
        if (PlayerStateSettings[State].JumpActive && CheckNearCoord(0,-1))//if on solid ground and jump active
        {
            velocity = new Vector2(velocity.x, JumpHeight);//add jump to the velocity
            PlayerSounds.Play("Jump");
        }
    }
}
