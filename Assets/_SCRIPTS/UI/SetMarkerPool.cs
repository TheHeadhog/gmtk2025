using UnityEngine;

public class SetMarkerPool : MonoBehaviour
{
    [SerializeField] private SetMarkerUI prefab;

    [SerializeField] private CalendarGrid grid;

    private void Start()
    {
        var checkManager = CheckManager.Instance;
        foreach (var cm in checkManager.AllCheckMarkers)
        {
            var ui = Instantiate(prefab, transform);
            ui.Init(cm, grid, checkManager);
        }
    }

    private void OnEnable()
    {
                
    }
}