using UnityEngine;
using TMPro;

public class TimeTick : MonoBehaviour
{
    [SerializeField]
    private TMP_Text TimeLabel;
    
    private void Start()
    {
        GameEvents.GameTimeChanged += OnGameTimeChanged;
    }

    private void OnDestroy()
    {
        GameEvents.GameTimeChanged -= OnGameTimeChanged;
    }

    private void OnGameTimeChanged(int gameTick)
    {
        var gameTime = gameTick.FromGameTick();
        TimeLabel.text = $"{FormatToTwoDigits(gameTime.Hours)}:{FormatToTwoDigits(gameTime.Minutes)}";
    }

    private string FormatToTwoDigits(int value)
    {
        if (value <= 9)
        {
            return $"0{value}";
        }
        return value.ToString();
    }
}
