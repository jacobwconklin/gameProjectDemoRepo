using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsanePatientMovement : MonoBehaviour
{
    /*
     * Not 100% sure how I want him to move yet, Ideally his red light would indicate when he finds a target and would shoot towards them maybe?
     * to start I plan on just sporadic random quick movements, Then I can add tracking players, and then finally the light being an indicator
     */
    private Rigidbody rb;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float turnDegreesPerSecond = 180f;
    [SerializeField] private float lookingDistance = 40f;
    [SerializeField] private float chaseDuration = 2f; // Time in seconds that patient keeps up chase
    [SerializeField] private float initialDelay = 4.5f; // Give delay for game to begin
    private float beginTime;
    // private float endChase;
    private bool hasTarget = false;
    private Vector3 targetPosition = Vector3.zero;
    //private bool hasCollided = false;
    private RaycastHit hitInfo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        beginTime = Time.time + initialDelay;
    }

    void Update()
    {
        if (Time.time < beginTime) return;
        // Check for having reached position
        // if (transform.position == targetPosition)
        //{
        //    hasTarget = false;
        //}

        // Idea: Turn a small amount and shoot out a raycast, if a living player is hit charge in that direction until colliding with something.
        if (!hasTarget) // Should break inside into its own method TODO
        {
            // Find a target
            transform.Rotate(0, turnDegreesPerSecond * Time.deltaTime, 0, Space.World);
            Vector3 launchOrigin = transform.position;
            launchOrigin.y += 0.2f;
            if (Physics.Raycast(launchOrigin, transform.forward, out hitInfo, lookingDistance))
            {
                Debug.Log("Shooting raycast");
                // Check that a player with a rigid body has been hit. If so pursue the player
                if (hitInfo.rigidbody)
                {
                    PlayerGameplay playerGameplay = hitInfo.rigidbody.gameObject.GetComponent<PlayerGameplay>();
                    if (playerGameplay != null && playerGameplay.getIsAlive())
                    {
                        // Successfully hit a living player, now move in that direction
                        hasTarget = true;
                        targetPosition = hitInfo.point;
                        targetPosition.y = transform.position.y;
                        // endChase = Time.time + chaseDuration;
                        StartCoroutine(MoveDirectly(targetPosition));
                        // Create visual for moving? TODO
                    }
                }
            }
        } /*else
        {
            // go for chaseDuration seconds
            if (Time.time < endChase)
            {
                // Does have a target get to target spot
                rb.MovePosition(targetPosition * Time.deltaTime * speed);
            } else
            {
                hasTarget = false;
            }
                
        } */
        
    }

    // This actually moves the patient and all children
    IEnumerator MoveDirectly( Vector3 target)
    {
        Vector3 startPosition = transform.position;
        float time = 0f;

        while (transform.position != target)
        {
            transform.position = Vector3.Lerp(startPosition, target, (time / Vector3.Distance(startPosition, target)) * speed);
            time += Time.deltaTime;
            yield return null;
        }
        hasTarget = false;
    }

}
