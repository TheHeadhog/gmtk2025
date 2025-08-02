using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CalendarCell : MonoBehaviour
{
    public GameTime StartTime { get; private set; }
    public int Tick => StartTime.ToGameTick();
    public CellState State { get; private set; } = CellState.Normal;
    public RectTransform Rect { get; private set; }

    [SerializeField] private Color normalColor = new(0, 0.4f, 0.25f, 0);
    [SerializeField] private Color highlightColor = new(0, 0.5f, 0.3f, 0.3f);
    [SerializeField] private Color occupiedColor = new(0, 0.4f, 0.25f, 1);

    private Image background;

    private void Awake()
    {
        background = GetComponent<Image>();
        Rect = GetComponent<RectTransform>();
    }

    public void Init(GameTime startTime)
    {
        StartTime = startTime;
        SetState(CellState.Normal);
    }

    public void SetState(CellState state)
    {
        State = state;
        background.color = state switch
        {
            CellState.Highlighted => highlightColor,
            CellState.Occupied => occupiedColor,
            _ => normalColor
        };
    }
}

public enum CellState
{
    Normal,
    Highlighted,
    Occupied
}
