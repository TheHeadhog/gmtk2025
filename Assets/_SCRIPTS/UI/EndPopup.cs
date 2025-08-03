using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPopup : MonoBehaviour
{
    [SerializeField] Popup badPopup;
    [SerializeField] Popup neutralPopup;
    [SerializeField] Popup goodPopup;
    
    private void Start()
    {
        GameEvents.OnGameEnd += OpenPopup;
    }

    private void OnDestroy()
    {
        GameEvents.OnGameEnd -= OpenPopup;
    }

    private void OpenPopup(float finalScore)
    {
        var popup = finalScore switch
        {
            <= 0.5f => badPopup,
            <= 0.8f => neutralPopup,
            _ => goodPopup
        };
        
        popup.OpenPopup();
        StartCoroutine(ClosePopupCoroutine(popup));
    }

    private IEnumerator ClosePopupCoroutine(Popup popup)
    {
        yield return new WaitForSeconds(10);
        popup.OnClosed += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        popup.ClosePopup();
    }
}