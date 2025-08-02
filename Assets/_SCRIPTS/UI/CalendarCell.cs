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

    [SerializeField] private Color normalColor;
    [SerializeField] private Color normalColorYellow;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color occupiedColor;

    [SerializeField] private Image background;

    private void Awake()
    {
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
        if (state == CellState.Highlighted)
            background.color = highlightColor;
        else if (state == CellState.Occupied)
            background.color = occupiedColor;
        else if (state == CellState.Normal)
        {
            background.color = transform.GetSiblingIndex()%2==0?normalColor:normalColorYellow;
        }
    }
}

public enum CellState
{
    Normal,
    Highlighted,
    Occupied
}
