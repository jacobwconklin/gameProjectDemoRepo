
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Collections;

/**
 * STRUCTS
 */

public class Player
{
    public GameObject selectedCharacter;
    public AvailableNode currSelection; // Used as a cursor / iterator of sorts
    public int score;
    public PlayerInput playerInput;
    public int playerNum;
}

public class Skin
{
    public Vector3 originalLocation;
    public Quaternion originalRotation;
    public string name;
}

public class PersistentValues
{
    // Use game is started to allow player movement
    public bool gameIsStarted = false;

    public bool canSelectCharacters = false;

    public static PersistentValues persistentValues;

    public int numPlayers = 0;

    public List<Player> players; // A list of player structs keeping all of their necessary info
    // Plan to even keep track of player's cosmetic choices here 
    // Selected Characters are moved to this List and removed form available Characters list

    // Permanent list of Selectable Characters with corresponding names and skins
    public AvailableLinkedList availableCharacters;

    // Don't have access to a map unfortunately :( but should be relatively small amount of
    // available skins so just save names and initial locations. 
    private List<Skin> skins;

    public static void Initialize(List<GameObject> selectableCharacters)
    {
            persistentValues = new PersistentValues();
            persistentValues.players = new List<Player>();
            persistentValues.availableCharacters = new AvailableLinkedList(selectableCharacters);

            persistentValues.skins = new List<Skin>();
            // Manually put in all character skinNames to locations into map
            foreach(GameObject skin in selectableCharacters)
            {
                persistentValues.skins.Add(new Skin
                {
                    name = skin.name,
                    originalLocation = skin.transform.position,
                    originalRotation = skin.transform.rotation
                }) ;
            }

    }

    public void Reset()
    {
        numPlayers = 0; // And resets the number of Players, leave player's skin and flag selections for now. 

        // Don't want to just erase this because I want to preserve player skin choices, but for now I will just reset it. 
        players = new List<Player>();
        availableCharacters.setAllAvailable();
        gameIsStarted = false;
    }


    // returns number telling the player that it is the nth player to join 
    // Needs to be called when players are created. 
    public int AddPlayer(PlayerInput playerInput)
    {
        Debug.Log("in persistent values add player called");
        numPlayers++; 
        Player newPlayer = new Player();
        newPlayer.playerInput = playerInput;
        newPlayer.currSelection = availableCharacters.getFirstAvailable();
        newPlayer.selectedCharacter = newPlayer.currSelection.value;
        // SET NEW SKIN AS CHILD OF PLAYERINPUT AND SET PLAYERINPUT'S PLAYERMOVMENT ANIMATOR TO THE NEW SKIN
        newPlayer.selectedCharacter.transform.SetParent(newPlayer.playerInput.transform);
        // MANUALLY SET POSITION AND ROTATION?
        newPlayer.selectedCharacter.transform.position = newPlayer.playerInput.transform.position;
        newPlayer.selectedCharacter.transform.rotation = newPlayer.playerInput.transform.rotation;
        Animator newSkinAnimator = newPlayer.selectedCharacter.GetComponent<Animator>();
        PlayerGameplay playerGameplay = newPlayer.playerInput.GetComponent<PlayerGameplay>();
        playerGameplay.animator = newSkinAnimator;
        newSkinAnimator.ResetTrigger("TouchedGround");
        newSkinAnimator.ResetTrigger("Jumped");
        // NEED TO DO SOMETHING TO ANIMATOR TO SET UP JUMP TRIGGERING CORRECTLY FOR SOME REASON IDK WHAY THE PROB IS 
        playerGameplay.playerNum = numPlayers - 1;
        newPlayer.playerNum = numPlayers - 1;

        newPlayer.score = 0;
        players.Add(newPlayer);
        return numPlayers;
    }

    public void NextSkin(int playerNum)
    {
        Player player = players[playerNum];
        // NEED TO REMOVE PARENT??
        Animator oldSkinAnimator = player.selectedCharacter.GetComponent<Animator>();
        oldSkinAnimator.SetTrigger("TouchedGround");
        player.selectedCharacter.transform.SetParent(null);
        // PUT CHARACTER SKINS "BACK" BASED ON CHARACTER SKIN NAME 
        // Alternatively could just throw unselected skin under stage (easy) --> player.selectedCharacter.transform.position = new Vector3(0, -50, 0);
        foreach (Skin skin in skins)
        {
            if (skin.name == player.selectedCharacter.name)
            {
                player.selectedCharacter.transform.position = skin.originalLocation;
                player.selectedCharacter.transform.rotation = skin.originalRotation;
                break;
            }
        }
            

        player.currSelection = availableCharacters.nextAvailable(player.currSelection);
        player.selectedCharacter = player.currSelection.value;
        // SET NEW SKIN AS CHILD OF PLAYERINPUT AND SET PLAYERINPUT'S PLAYERMOVMENT ANIMATOR TO THE NEW SKIN
        player.selectedCharacter.transform.SetParent(player.playerInput.transform);
        // MANUALLY SET POSITION AND ROTATION?
        player.selectedCharacter.transform.position = player.playerInput.transform.position;
        player.selectedCharacter.transform.rotation = player.playerInput.transform.rotation;
        Animator newSkinAnimator = player.selectedCharacter.GetComponent<Animator>();
        PlayerGameplay playerGameplay = player.playerInput.GetComponent<PlayerGameplay>();
        playerGameplay.animator = newSkinAnimator;
        newSkinAnimator.ResetTrigger("TouchedGround");
    }

    public void AddPlayerVictory(int playerNum) // When a player wins add them based on their index in the playerName list
    {
        // Get name from game object?
        Player player = players[playerNum];
        player.score++;
    }

}