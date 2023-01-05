using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFoot : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerGameplay playerGameplay;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerGameplay = GetComponentInParent<PlayerGameplay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO may want to only allow some kinds of collision to count as ground?
        // For now just don't count object itself
        Debug.Log("trigger ENTER");
        
        playerMovement.isGrounded = true;
        playerGameplay.animator.SetTrigger("TouchedGround"); 
        
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("trigger EXIT");
        // TODO not sure how to tell if this foot is no longer touching anything?
        playerMovement.isGrounded = false;
        // Tell animator?
        playerGameplay.animator.SetTrigger("Jumped");
    }

}
