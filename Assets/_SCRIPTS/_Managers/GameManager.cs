using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int MaxNumberOfTimesThatPauseCanBeOpened = 1;
    
    [Header("Pause Input Buffer (seconds)")]
    [SerializeField] private float pauseBufferTime = 0.5f;

    private bool nextActionIsPausing = true;
    private bool canPauseTheGame = true;
    private Coroutine bufferCoroutine;
    private int pauseOpenedCounter;
    private bool isGameStarted;

    private void OnEnable()
    {
        GameEvents.OnGameStart += SetGameStarted;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= SetGameStarted;
    }

    private void SetGameStarted() => isGameStarted = true;
    
    private void Update()
    {
        if (!isGameStarted) return;
        
        DetectKeyPressed();
    }

    private void DetectKeyPressed()
    {
        if (!canPauseTheGame || pauseOpenedCounter >= MaxNumberOfTimesThatPauseCanBeOpened)
            return;
        
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.P))
        {
            GameEvents.RaisePauseGameKeyPressed(nextActionIsPausing);
            ProcessTimePausingInGame(nextActionIsPausing);

            nextActionIsPausing = !nextActionIsPausing;
            canPauseTheGame = false;

            if (bufferCoroutine != null)
                StopCoroutine(bufferCoroutine);
            bufferCoroutine = StartCoroutine(PauseInputBuffer());
        }
    }

    private IEnumerator PauseInputBuffer()
    {
        yield return new WaitForSeconds(pauseBufferTime);
        canPauseTheGame = true;
    }

    private void ProcessTimePausingInGame(bool isPausing)
    {
        if (isPausing)
            TimelineManager.Instance.PauseGame();
        else
        {
            TimelineManager.Instance.ResumeGame();
            pauseOpenedCounter++;
        }
    }
}