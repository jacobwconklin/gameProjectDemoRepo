using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private int levelSelectScene = 2;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject winnerDisplay;
    [SerializeField] private TextMeshProUGUI winnerName;
    private int numPlayers;
    private int numDeadPlayers;
    private List<int> livingPlayers = new List<int>();
    private Player winner = null;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Awake()
    {
        // Places every character
        StartCoroutine(PlaceAllCharacters());
    }

    // Coroutine and ienumerator allows waiting within function (for game begin countdown)
    IEnumerator PlaceAllCharacters()
    {
        List<Player> allPlayers = PersistentValues.persistentValues.players;
        foreach (Player player in allPlayers)
        {
            playerManager.placePlayer(player.playerInput, player.playerNum);
            // Make sure all players are alive again.
            PlayerGameplay playerGameplay = player.playerInput.GetComponent<PlayerGameplay>();
            playerGameplay.NewMatch();
            playerGameplay.SetGameController(this);
            playerGameplay.SetPlayerNum(player.playerNum);
            livingPlayers.Add(player.playerNum);
            // Reset skin's parent? TODO DON'T KNOW WHY THIS NEEDS TO BE DONE
            player.selectedCharacter.transform.SetParent(player.playerInput.transform);
        }
        // DISPLAY COUNTOWN ON UI TODO
        yield return new WaitForSeconds(3);
        // Unfreeze rigid body rotations

        foreach (Player newPlayer in allPlayers)
        {
            Rigidbody rb = newPlayer.playerInput.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
        }
        PersistentValues.persistentValues.gameIsStarted = true;
    }

    public void ReportDeath(int playerNum)
    {
        livingPlayers.Remove(playerNum);
    }

    // Update is called once per frame
    void Update()
    {
        // Determine when game is over
        // if (one or 0 players left alive) end game
        if (!gameOver && livingPlayers.Count <= 1)
        {
            gameOver = true;
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        Debug.Log("ENDING GAME");
        if (livingPlayers.Count == 1)
        {
            winner = PersistentValues.persistentValues.players[livingPlayers[0]];
            PersistentValues.persistentValues.AddPlayerVictory(livingPlayers[0]);
            // End game 
            Debug.Log("winner is: " + winner.selectedCharacter.gameObject.name);
            // DISPLAY WINNER ON UI
            ShowWinner();
            yield return new WaitForSeconds(5);

        }
        else if (livingPlayers.Count < 1)
        {
            // Everyone loses
            Debug.Log("Everyone lost");
            yield return new WaitForSeconds(5);
        }
        // end game essentially and move to level select:
        PersistentValues.persistentValues.gameIsStarted = false;
        // want a quick Victory pop up here before returning to scene selection so will need coroutines probably. 
        SceneManager.LoadScene(levelSelectScene);
    }

    public void ShowWinner()
    {
        // Enable winnerDisplay on GameUI so winner's name appears
        winnerDisplay.SetActive(true);
        if (winner != null)
        {
            winnerName.text = winner.selectedCharacter.gameObject.name;
            // Would be great to change WinnerName color depending on player
            SkinInfo skinInfo = winner.selectedCharacter.gameObject.GetComponent<SkinInfo>();
            if (skinInfo != null) winnerName.color = skinInfo.GetColor();
        } else
        {
            winnerName.text = "No One";
            winnerName.color = Color.black;
        }
        

        //Invoke("HideWinner", 3);

    }

    public void HideWinner()
    {
        // Maybe call this in this class after a certain amount of time, or call it upon death from death and respawn. 
        winnerDisplay.SetActive(false);
    }
}
