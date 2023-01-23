using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    [SerializeField] private GameObject intact;
    [SerializeField] private GameObject cracked;
    [SerializeField] private GameObject crackedMore;
    [SerializeField] private AudioSource touchedSound;

    private int timesTouched = 0;

    // Start is called before the first frame update
    void Start()
    {
        intact.SetActive(true);
        cracked.SetActive(false);
        crackedMore.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // increase times touched
        touched();
    }

    private void touched()
    {
        // Make sound of being touched
        touchedSound.Play();
        // Increase times touched and check for thresholds to be passed to perform action
        timesTouched++;
        // at 3 switch to cracked
        if (timesTouched == 3)
        {
            intact.SetActive(false);
            cracked.SetActive(true);
        }
        // at 7 switch to crackedMore
        else if (timesTouched == 7)
        {
            cracked.SetActive(false);
            crackedMore.SetActive(true);
        }
        // Finally at 10 break iceblock, could have iceblock descend or particle effect of it breaking
        else if (timesTouched == 10)
        {
            Destroy(gameObject);
        }
    }
}
