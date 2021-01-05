using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI[] levels;

    private void Start()
    {
        StartCoroutine(ShowLevelsScores());
    }


    IEnumerator ShowLevelsScores()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        bool hasSomeScore = false;
        for(int i = 0; i< levels.Length; i++)
        {
            if (PlayerPrefs.HasKey("bestTimeTexLevel_" + i))
            {
                hasSomeScore = true;
                levels[i].text = "<b>" + (i+1) + ": </b> " + PlayerPrefs.GetString("bestTimeTexLevel_" + i);
            }
            else
            {
                levels[i].text = "<b>" + (i+1) + ": </b> ";
            }
        }

        if (hasSomeScore)
        {
            GetComponent<Animator>().enabled = true;
        }

    }

}
