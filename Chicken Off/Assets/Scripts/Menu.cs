
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private int freeForAllSceneNumber = 1;
    [SerializeField] private int lastPairStandingSceneNumber = 2;
    [SerializeField] private int campaignSceneNumber = 3;
    public void StartGame ()
    {
        /* Peak maintains some data this way not sure If I will in ChickenOff
        // Reset the Persistent Values, should really store them in a file but for now they are not. Resets only what is necessary
        PersistentValues.Initialize();
        PersistentValues.persistentValues.Reset();
        */

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartFreeForAll()
    {
        // Should load Character selection screen for FreeForAll game
        SceneManager.LoadScene(freeForAllSceneNumber);
    }

    public void StartLastPairStanding()
    {
        SceneManager.LoadScene(lastPairStandingSceneNumber);
    }

    public void StartCampaign()
    {
        SceneManager.LoadScene(campaignSceneNumber);
    }
}
