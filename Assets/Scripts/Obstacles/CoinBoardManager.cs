using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class CoinBoardManager : MonoBehaviour
{
    public static CoinBoardManager Instance;

    [Header("Puzzle Settings")]
    public List<CoinSlot> slots; 

    [Header("Puzzle Riddle Phases")]
    public List<CoinPhase> correctPhases;

    public List<string> poemPhases; 

    [Header("UI + Final Reward")]
    public TextMeshProUGUI poemText; 
    public GameObject rewardObject;  

    private int currentPhase = 0;

    private void Awake()
    {
        Instance = this;
        UpdatePoem();
    }

    public void CheckBoardState()
    {
        if (slots.Exists(slot => !slot.IsFilled))
            return; 

        bool correct = true;

        for (int i = 0; i < slots.Count; i++)
        {
            string visibleID = slots[i].GetVisibleSideID();
            if (visibleID != correctPhases[currentPhase].visibleIDs[i])
            {
                correct = false;
                break;
            }
        }

        if (correct)
        {
            Debug.Log("Correct configuration!");

            currentPhase++;

            if (currentPhase >= correctPhases.Count)
            {
                Debug.Log("Puzzle complete!");
                CompletePuzzle();
            }
            else
            {
                UpdatePoem();
            }
        }
        else
        {
            Debug.Log("Incorrect configuration. Try again.");
        }
    }

    void UpdatePoem()
    {
        if (poemText != null && poemPhases.Count > currentPhase)
        {
            poemText.text = poemPhases[currentPhase];
        }
    }

    void CompletePuzzle()
    {
        if (rewardObject != null)
            rewardObject.SetActive(true);

        if (poemText != null)
            poemText.text = "You hear a click nearby...";

        StartCoroutine(ExitAfterDelay(1.5f));
    }

    IEnumerator ExitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PuzzleCameraController.Instance.ExitPuzzleMode();
    }

}

[System.Serializable]
public class CoinPhase
{
    public string[] visibleIDs = new string[3]; // e.g. ["sun", "flower", "sword"]
}


