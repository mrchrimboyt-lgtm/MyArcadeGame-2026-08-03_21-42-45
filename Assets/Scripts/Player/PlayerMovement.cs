using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //OLD PLAYER SCRIPT NOT IN USE
    //GitHub Test
    public float MovementSpeed;
    public float JumpHeight;

    private Vector2 MovementInput;
    private Vector2 JumpInput;

    public InputActionReference Movementaction;
    public InputActionReference JumpAction;

    public Transform[] WallChecks; //Down, Left, Right
    private Vector2[] WallJumpValues = new Vector2[3]{new Vector2(0, 1),new Vector2(2, 0.5f),new Vector2(-2, 0.5f)}; //the velocity change for each wall direction

    public Rigidbody2D rigidbody;
    public Collider2D playerCollider;

    

    // Update is called once per frame
    void Update()
    {
        MovementInput = Movementaction.action.ReadValue<Vector2>();
        
    }
    void FixedUpdate()
    {
        rigidbody.linearVelocity = new Vector2(MovementInput.x * MovementSpeed, rigidbody.linearVelocity.y); //* Time.deltaTime
        if(JumpInput.y != 0)
        {
            rigidbody.linearVelocity = new Vector2(((0-MovementInput.x) + JumpInput.x) * MovementSpeed * 2, JumpInput.y * JumpHeight);
            JumpInput = new Vector2(0, 0);
        }
    }

    private void UpdateJumpValues()
    {
        JumpInput = new Vector2(0, 0);
        for (int i =0; i < 3; i++)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(WallChecks[i].position, 0.2f);
            foreach (Collider2D hit in hits)
            {
                if (hit != playerCollider) 
                {
                    JumpInput= JumpInput+ WallJumpValues[i];
                    break; 
                }
            }
        }
    }


    private void OnEnable() { JumpAction.action.started += Jump; }
    private void OnDisable() { JumpAction.action.started -= Jump; }
    private void Jump(InputAction.CallbackContext obj)
    {
        UpdateJumpValues();
    }
}
