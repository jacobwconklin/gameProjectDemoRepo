using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacks : MonoBehaviour
{
    // Describes the max distance that a player's push can reach
    [SerializeField] private float pushMaxDistance = 5.0f;
    // Amount of force behind a player's push
    [SerializeField] private float pushForce = 10.0f;
    private RaycastHit hitInfo;

    // amount of time required for push to cooldown
    [SerializeField] private float pushCooldown = 1.0f;
    private float nextPushTime = 0;
    private bool pressedPush = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Perform a push forwards
        // needs a cooldown entire animation at least should execute
        if (pressedPush && Time.time >= nextPushTime)
        {
            ForcePush();
            nextPushTime = Time.time + pushCooldown;
        }

    }

    // input receiving methods
    public void OnPush(InputAction.CallbackContext pushed) => pressedPush = pushed.ReadValueAsButton();

    void ForcePush()
    {
        // Show animation regardless
        Animator animator = GetComponent<PlayerGameplay>().animator;
        animator.SetTrigger("Push");

        // Only allow when game is started
        if (PersistentValues.persistentValues == null || !PersistentValues.persistentValues.gameIsStarted) return;

        Vector3 launchOrigin = transform.position;
        launchOrigin.y += 2.0f;
        Debug.Log("force pushing new raycast launch origin is:\n x:" + launchOrigin.x + " y: " + launchOrigin.y + " z:" + launchOrigin.z);
        if (Physics.Raycast(launchOrigin, transform.forward, out hitInfo, pushMaxDistance))
        {
            // Check that an ememy with a rigid body has been hit. If so apply the
            // force to the rigid body
            if (hitInfo.rigidbody)
            {
                Debug.Log("push found rigidbody: " + hitInfo.rigidbody.gameObject.name);
                Vector3 pushDirection = transform.forward;
                pushDirection.Normalize();
                // for some reason pushes are so powerful? so Im going to decrease this vector substantially
                // pushDirection = pushDirection * 0.0001f;
                // hitInfo.rigidbody.AddForceAtPosition(pushDirection * (pushForce - (hitInfo.distance / 100)), hitInfo.rigidbody.transform.position);
                hitInfo.rigidbody.AddForce(pushDirection * pushForce, ForceMode.VelocityChange);
                // Check that enemy has a PlayerGameplay component and if they
                // do report the hit
                PlayerGameplay enemyGameplay = hitInfo.rigidbody.gameObject.GetComponent<PlayerGameplay>();
                if (enemyGameplay) 
                {
                    enemyGameplay.takeAHit();
                }
            }
        }
    }
}
