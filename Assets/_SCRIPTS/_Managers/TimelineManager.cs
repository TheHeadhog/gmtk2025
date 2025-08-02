using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TimelineManager : SingletonPersistent<TimelineManager>
{
    private WaitForSeconds waitTime;
    private bool isGameRunning;
    private Coroutine timelineCoroutine;

    private int gameTicks;

    private void OnEnable()
    {
        GameEvents.OnGameStart += StartGame;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= StartGame;
    }

    public int GameTicks
    {
        get => gameTicks;
        private set
        {
            gameTicks = value;
            GameEvents.RaiseGameTimeChanged(gameTicks);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        waitTime = new WaitForSeconds(GameConfig.RealSecondsPerGameMinute);
    }

    private void StartGame()
    {
        gameTicks = 0;
        ResumeGame();
    }

    public void ResumeGame()
    {
        isGameRunning = true;
        timelineCoroutine = StartCoroutine(CountGameTicksCoroutine());
    }

    public void PauseGame()
    {
        isGameRunning = false;
        StopCoroutine(timelineCoroutine);
    }

    private IEnumerator CountGameTicksCoroutine()
    {
        while (isGameRunning)
        {
            yield return waitTime;
            GameTicks++;
        }
    }
}