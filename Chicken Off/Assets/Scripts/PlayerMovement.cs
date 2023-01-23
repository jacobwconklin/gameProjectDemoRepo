using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 4.0f;
    [SerializeField] private float rotSpeed = 700f;
    [SerializeField] private float jumpForce = 50f;

    // As player gets "tired" or takes damage should decrease their degreesPerSecond that they can tilt
    [SerializeField] private float degreesPerSecond = 180.0f;

    // TODO should move to player gameplay
    private PlayerGameplay gameplay;

    // Where player Input is stored to be read by Update
    private Vector2 movementInput;
    private Vector2 tiltInput;
    // Jumping has a slight cooldown to fix glitchy jumps where collider stays inside 
    // other blocks and player jumps insanely high
    private float jumpCooldown = 0;
    private bool pressedJump = false;
    public bool isGrounded = true;
    public bool pressedStart = false; // Everyone press start at same time to start game?


    // Start is called before the first frame update
    void Start()
    {
        gameplay = GetComponent<PlayerGameplay>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameplay.getAliveStatus() || gameplay.animator == null)
        {
            // While player is dead don't move
            // animator.SetFloat("Speed", -1); This doesn't do anything I don't know why. Need to create death animation. 
            return;
        }

        // If grounded and pressed jump perform jump. Animation should be triggered soley by colider?? maybe
        if (pressedJump && isGrounded && Time.time > jumpCooldown)
        {
            GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            // Cue sound
            gameplay.playerAudio.playJumpSound();
            jumpCooldown = Time.time + 0.5f;
        }

        if (movementInput != Vector2.zero)
        {

            /*
             * MOVE PLAYER
             */
            Vector3 velocity = new Vector3(movementInput.x, 0, movementInput.y);

            if (PersistentValues.persistentValues != null && PersistentValues.persistentValues.gameIsStarted)
            {
                transform.Translate(velocity * Time.deltaTime * speed, Space.World);
            }
            gameplay.animator.SetFloat("Speed", velocity.magnitude);

            // velocity.Normalize(); TODO trying to move / delete this line

            /*
             * ROTATE PLAYER
             * Allows player movement direction to turn Player along y axis
             */
        
            // Rotate towards direction of movemebt while moving
            velocity.y = transform.position.y;
            Quaternion toRotate = Quaternion.LookRotation(velocity, Vector3.up);
            // Movement causes rotation soley around y axis and leaves x / z rotation up to
            // player input. 
            Quaternion newRot = Quaternion.RotateTowards(transform.rotation, toRotate, rotSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, newRot.eulerAngles.y, transform.eulerAngles.z);
        } else
        {
            gameplay.animator.SetFloat("Speed", 0);
        }

        /*
         * TILT PLAYER
         * Allow player to tilt their character to maintain its balance
         * IE rotation along x and z axis
         */
        if (tiltInput != Vector2.zero)
        {
            if (PersistentValues.persistentValues != null && PersistentValues.persistentValues.gameIsStarted)
            {
                transform.Rotate(tiltInput.y * degreesPerSecond * Time.deltaTime,
                    0, tiltInput.x * -1 * degreesPerSecond * Time.deltaTime, Space.World);
            }
        }
    }

    // Input receiving methods
    public void OnMove(InputAction.CallbackContext moveValue) => movementInput = moveValue.ReadValue<Vector2>();
    public void OnTilt(InputAction.CallbackContext tiltValue) => tiltInput = tiltValue.ReadValue<Vector2>();
    public void OnJump(InputAction.CallbackContext jumped) => pressedJump = jumped.ReadValueAsButton(); 
    public void OnStart(InputAction.CallbackContext started) => pressedStart = started.ReadValueAsButton();
}
