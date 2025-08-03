using TMPro;
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
    [SerializeField] private TMP_Text timeLabel;

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
        else if (state == CellState.Normal || state == CellState.Occupied)
        {
            background.color = transform.GetSiblingIndex()%2==0?normalColor:normalColorYellow;
        }

        timeLabel.text = StartTime.ToString();
    }
}

public enum CellState
{
    Normal,
    Highlighted,
    Occupied
}
