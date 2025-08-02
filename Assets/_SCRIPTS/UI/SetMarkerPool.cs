using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class SetMarkerPool : MonoBehaviour
{
    [SerializeField] private SetMarkerUI prefab;

    [SerializeField] private CalendarGrid grid;

    [SerializeField] private List<CheckMarker> sourceMarkers;

    private void Start()
    {
        var checkManager = CheckManager.Instance;
        foreach (var cm in sourceMarkers)
        {
            var ui = Instantiate(prefab, transform);
            ui.Init(cm, grid, checkManager);
        }
    }
}