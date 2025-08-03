using UnityEngine;

public class PausePopup : MonoBehaviour
{
    [SerializeField] Popup popup;
    
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
            OpenPausePopup();
        }
        else
        {
            ClosePausePopup();
        }
    }

    private void OpenPausePopup()
    {
        popup.OpenPopup();
        AudioSystem.Instance.Play(AudioSoundType.PauseMusic);
    }

    private void ClosePausePopup()
    {
        popup.ClosePopup();
        AudioSystem.Instance.Play(AudioSoundType.BackgroundMusic);
    }
}