using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SetMarkerUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private const int SizePer10Minutes = 25;
    private const int LeftSideOffset = 113;
    private const int TopSideOffset = 12;
    
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private Color normalBackgroundColor = new(1, 1, 1, 0.6f);
    [SerializeField] private Color dragBackgroundColor = new(1, 1, 1, 0.6f);

    private RectTransform rect;
    private RectTransform parentRect;
    private LayoutElement layoutElement;
    private Vector2 grabOffset;
    private CalendarGrid grid;
    private CheckManager checkManager;
    private CheckMarker marker;
    private Vector3 homePos;
    private Vector3 targetAnchoredPos;
    private bool isPlaced;

    public CheckMarkerId MarkerId => marker.Id;
    public int Duration => marker.DurationInMinutes;

    public void Init(CheckMarker marker, CalendarGrid grid, CheckManager cm)
    {
        this.marker = marker;
        this.grid = grid;
        checkManager = cm;
        
        rect = GetComponent<RectTransform>();
        Vector2 size = rect.sizeDelta;
        size.y = (float)marker.DurationInMinutes / 10 * SizePer10Minutes;   
        rect.sizeDelta = size;
        
        parentRect = transform.parent.GetComponent<RectTransform>();
        layoutElement = GetComponent<LayoutElement>();
        homePos = rect.anchoredPosition;

        titleText.text = marker.Message;
        bodyText.text = $"{marker.SenderPerson.FullName}\n{marker.DurationInMinutes} min";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        grabOffset = eventData.position - new Vector2(rect.position.x, rect.position.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlaced) return;
        SetColors(true);

        rect.position = eventData.position - grabOffset;

        Vector3 worldTop = rect.position + new Vector3(0, rect.rect.height / 2f, 0);
        var cell = grid.GetCellAtPointer(worldTop);

        grid.MarkRange(new TimeRange(0, 24 * 60), CellState.Normal);

        if (cell == null) return;

        var range = new TimeRange(cell.Tick, cell.Tick + Duration);

        bool isPossibleToPlace = range.BeginTick >= 0 && range.EndTick <= (8 * 60);
        isPossibleToPlace &= grid.IsRangeFree(range);

        if (isPossibleToPlace)
        {
            var firstCellAtRangeTransform = grid.GetFirstCellAtRange(range);
            if (firstCellAtRangeTransform)
            {
                targetAnchoredPos = ConvertAnchoredPosition(firstCellAtRangeTransform, grid.GetComponent<RectTransform>());
            }
        }

        grid.MarkRange(range, isPossibleToPlace ? CellState.Highlighted : CellState.Occupied);
    }


    private void SetColors(bool isDragging)
    {
        backgroundImage.color = isDragging ? dragBackgroundColor : normalBackgroundColor;
        titleText.color = isDragging ? normalBackgroundColor : dragBackgroundColor;
        bodyText.color = isDragging ? normalBackgroundColor : dragBackgroundColor;
    }
    
    private Vector2 ConvertAnchoredPosition(Transform firstCellTf, RectTransform gridParent)
    {
        var firstCell = firstCellTf as RectTransform;

        float worldTopY = firstCell.position.y + firstCell.rect.height * firstCell.lossyScale.y * 0.5f;

        Vector2 localTop = gridParent.InverseTransformPoint(
            new Vector3(firstCell.position.x, worldTopY, 0));

        localTop.y -= rect.rect.height * 0.5f + TopSideOffset;
        localTop.x = rect.sizeDelta.x * 0.5f;

        return localTop;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        SetColors(false);

        var cell = grid.GetCellAtPointer(eventData.position);
        if (cell == null)
        {
            ReturnHome();
            return;
        }

        var range = new TimeRange(cell.Tick, cell.Tick + Duration);
        if (!grid.IsRangeFree(range) || range.EndTick > 8 * 60)
        {
            ReturnHome();
            return;
        }

        grid.MarkRange(range, CellState.Occupied);
        PlaceMarker(cell.StartTime);
    }

    private void PlaceMarker(GameTime startTime)
    {
        if (isPlaced)
        {
            return;
        }

        var sm = new SetMarker { Id = MarkerId, Timestamp = startTime };
        layoutElement.ignoreLayout = true;
        checkManager.AddNewSetMarker(sm);
        isPlaced = true;
        
        rect.SetParent(grid.transform);
        rect.anchoredPosition = targetAnchoredPos;
        rect.anchoredPosition += Vector2.right * LeftSideOffset;
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
    }

    private void ReturnHome()
    {
        rect.anchoredPosition = homePos;
        layoutElement.ignoreLayout = false;
        grid.MarkRange(new TimeRange(0, 24 * 60), CellState.Normal);
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
    }
}