using System;
using System.Collections;
using UnityEngine;

public class TimelineManager : SingletonPersistent<TimelineManager>
{
    private WaitForSeconds waitTime;
    private bool isGameRunning;
    private Coroutine timelineCoroutine;

    private int gameTicks;

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

    private void ResumeGame()
    {
        isGameRunning = true;
        timelineCoroutine = StartCoroutine(CountGameTicksCoroutine());
    }

    private void PauseGame()
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
