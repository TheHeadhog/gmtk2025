using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseGameBehaviour : MonoBehaviour
{
    [SerializeField]
    private Image PauseScreenImage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        GameEvents.OnPauseGameKeyPressed += ProcessPauseSignal;
    }

    private void OnDestroy()
    {
        GameEvents.OnPauseGameKeyPressed -= ProcessPauseSignal;
    }

    private void ProcessPauseSignal(bool isPausing)
    {
        if (isPausing)
        {
            ProcessGamePausing();
        }
        else
        {
            ProcessGameUnpausing();
        }
    }

    private void ProcessGamePausing()
    {
        
    }
    
    private void ProcessGameUnpausing()
    {
        
    }
}
