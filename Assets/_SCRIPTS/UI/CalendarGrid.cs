using System;
using System.Collections.Generic;
using UnityEngine;

public class CalendarGrid : MonoBehaviour
{
    private const int Rows = 8;
    private const int SubRows = 6;

    [SerializeField] private CalendarCell cellPrefab;
    [SerializeField] private Transform hourRowPrefab;

    private readonly List<CalendarCell> cells = new();

    private void Start()
    {
        BuildGrid();
    }

    private void BuildGrid()
    {
        var t = 0;
        for (var hour = 0; hour < Rows; hour++)
        {
            var hourRow = Instantiate(hourRowPrefab, transform);
            for (var minute = 0; minute < SubRows; minute++, t += 10)
            {
                var hours = 9 + hour;
                var minutes = minute * 10;
                var gameTime = new GameTime(hours, minutes);
                
                var cell = Instantiate(cellPrefab, hourRow);
                cell.Init(gameTime);
                cells.Add(cell);
            }
        }
    }

    public bool IsRangeFree(TimeRange range)
    {
        foreach (var cell in cells)
        {
            if (cell.State == CellState.Occupied &&
                new TimeRange(cell.Tick, cell.Tick + 10).Overlaps(range))
            {
                return false;
            }
        }

        return true;
    }

    public void MarkRange(TimeRange range, CellState state)
    {
        foreach (var cell in cells)
        {
            if (cell.Tick >= range.BeginTick && cell.Tick < range.EndTick)
            {
                cell.SetState(state);
            }
        }
    }

    public Transform GetFirstCellAtRange(TimeRange range)
    {
        foreach (var cell in cells)
        {
            if (cell.Tick >= range.BeginTick && cell.Tick < range.EndTick)
            {
                return cell.transform;
            }
        }

        return null;
    }
    public CalendarCell GetCellAtPointer(Vector2 screenPos)
    {
        foreach (var cell in cells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(
                    cell.GetComponent<RectTransform>(), screenPos))
            {
                return cell;
            }
        }

        return null;
    }
}