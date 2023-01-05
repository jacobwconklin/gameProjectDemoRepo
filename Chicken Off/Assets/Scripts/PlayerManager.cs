using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // private List<PlayerInput> players = new List<PlayerInput>();
    // private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] private List<Transform> spawnPoints;
    private PlayerInputManager playerInputManager;

    // Start is called before the first frame update
    void Start()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    /**
     * Called each time a new player clicks a button on their controller
     * thus entering the game
     */
    public void addPlayer(PlayerInput player)
    {
        // players.Add(player); 
        // On player spawn add them to PersistentValues player list
        // and place them at spawn point. 
        int nthPlayer = PersistentValues.persistentValues.AddPlayer(player);
        player.transform.position = spawnPoints[nthPlayer - 1].position;
        player.transform.rotation = spawnPoints[nthPlayer - 1].rotation;
        // Freeze player rotations on adding them
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

    }

    /**
     * Just repositions players
     */
    public void placePlayer(PlayerInput player, int playerNum)
    {
        // Freeze player rotations on adding them 
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // NEED TO DO SOMETHING TO ANIMATOR TO SET UP JUMP TRIGGERING CORRECTLY FOR SOME REASON IDK WHAY THE PROB IS 

        player.transform.position = spawnPoints[playerNum].position;
        player.transform.rotation = spawnPoints[playerNum].rotation;
    }

}
