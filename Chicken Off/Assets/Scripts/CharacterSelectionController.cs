using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionController : MonoBehaviour
{
    [SerializeField] private List<GameObject> selectableCharacters;
    private List<Player> allPlayers;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("initializing persistent values");
        // PersistentValues
        PersistentValues.Initialize(selectableCharacters);
        PersistentValues.persistentValues.canSelectCharacters = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Start game once there are enough people and everyone presses start
        allPlayers = PersistentValues.persistentValues.players;
        if (allPlayers.Count > 1)
        {
            // TODO CURRENTLY JUST NEEDS ONE PERSON TO PRESS START
            foreach (Player player in allPlayers)
            {
                PlayerMovement playerMovement = player.playerInput.GetComponent<PlayerMovement>();
                if (playerMovement.pressedStart)
                {
                    // Starting next scene, set all Characters to not destroy on load now so other skins aren't 
                    // also saved (problem from before).
                    // No longer allow for character selection:
                    PersistentValues.persistentValues.canSelectCharacters = false;

                    foreach (Player onePlayer in allPlayers)
                    {
                        DontDestroyOnLoad(onePlayer.playerInput.gameObject);
                    }

                    SceneManager.LoadScene(2);
                }
            }
        }
    }
}
