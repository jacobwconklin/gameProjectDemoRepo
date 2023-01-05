using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    private PlayerGameplay gameplay;

    // Start is called before the first frame update
    void Start()
    {
        gameplay = GetComponentInParent<PlayerGameplay>();
    }

    // Update is called once per frame
    void Update()
    {
        // If head goes below 0 it also dies.
        if (gameplay.getAliveStatus() && transform.position.y < 0)
        {
            gameplay.headOnGround();
        }
    }

    // When head collides with something need to know if it is:
    // 1) The ground (Player loses)
    // 2) another player (taking a hit)
    // 3) mundane obstacle / wall (do nothing)
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision detected");
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("GROUND"))
        {
            Debug.Log("Head hit ground");
            gameplay.headOnGround();
        } else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            gameplay.takeAHit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision detected");
        if (other.gameObject.layer == LayerMask.NameToLayer("GROUND"))
        {
            Debug.Log("Head hit ground");
            gameplay.headOnGround();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            gameplay.takeAHit();
        }
    }
}
