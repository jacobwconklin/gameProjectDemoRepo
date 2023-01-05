using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGameplay : MonoBehaviour
{
    private bool isAlive = true;
    private GameController gameController = null;
    // Animator and player number get set by PersistentValues code
    public Animator animator;
    public int playerNum;

    // For receiving input:
    private bool pressedSwitch = false;

    /*
     *  Track important values for Gameplay and gameplay events occurring. 
     */
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Switch player skins while in character select stage
        if ( pressedSwitch && PersistentValues.persistentValues.canSelectCharacters)
        {
            PersistentValues.persistentValues.NextSkin(playerNum);
        }
        pressedSwitch = false;
    }

    // Reset important gameplay values
    public void NewMatch()
    {
        isAlive = true;
    }

    // Sets GameController for current scene to report death tod
    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

    // Sets GameController for current scene to report death tod
    public void SetPlayerNum(int playerNum)
    {
        this.playerNum = playerNum;
    }

    // Interaction methods here for SSOT for fields. 

    // Taking a hit may decrease degrees per second and movement speed which could be recovered
    // automatically over time. It may also display some effect?
    public void takeAHit()
    {

    }

    public bool getAliveStatus() { return isAlive; }

    // To be called by head's script when it collides with a layer designated as ground
    // (This is when a player should lose)
    public void headOnGround()
    {
        // Player loses
        isAlive = false;
        Debug.Log("He ded");
        if (gameController != null)
        {
            gameController.ReportDeath(playerNum);
        }
    }

    public void OnSwitch(InputAction.CallbackContext switched) // This is set up for switching skins, need to change name and button and
    {
        if (switched.performed)
        {
            pressedSwitch = true;
        }
    }

}
