using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool NextActionIsPausing;
    private bool CanPauseTheGame;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        NextActionIsPausing = true;
        CanPauseTheGame = true;
        GameEvents.OnPauseGameKeyPressed += ProcessTimePausingInGame;
    }

    private void OnDestroy()
    {
        GameEvents.OnPauseGameKeyPressed -= ProcessTimePausingInGame;
    }

    // Update is called once per frame
    private void Update()
    {
        DetectKeyPressed();
    }

    private void DetectKeyPressed()
    {
        if (!CanPauseTheGame && NextActionIsPausing)
        {
            return;
        }
        
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.P))
        {
            GameEvents.RaisePauseGameKeyPressed(NextActionIsPausing);
            NextActionIsPausing = !NextActionIsPausing;
            CanPauseTheGame = false;
        }
    }

    private void ProcessTimePausingInGame(bool isPausing)
    {
        var manager = GetComponent<TimelineManager>();
        
        if (isPausing)
        {
            manager.PauseGame();
        }
        else
        {
            manager.ResumeGame();
        }
    }
}
