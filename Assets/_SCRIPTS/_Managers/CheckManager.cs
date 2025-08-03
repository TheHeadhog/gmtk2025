using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class CheckManager : SingletonPersistent<CheckManager>
{
    [SerializeField] private List<CheckMarker> allCheckMarkers;

    private List<SetMarker> setMarkers = new();
    public List<CheckMarker> AllCheckMarkers => allCheckMarkers;

    public int Score { get; private set; }

    private void OnEnable()
    {
        GameEvents.GameTimeChanged += CheckGameTick;
    }

    private void OnDisable()
    {
        GameEvents.GameTimeChanged -= CheckGameTick;
    }

    public void AddNewSetMarker(SetMarker setMarker)
    {
        setMarkers.Add(setMarker);
    }

    private void CheckGameTick(int gameTick)
    {
        var setMarkersInCurrentTick = setMarkers.Where(m => m.Timestamp.ToGameTick() == gameTick).ToList();
        var checkMarkersInCurrentTick = allCheckMarkers.Where(m => m.Timestamp.ToGameTick() == gameTick).ToList();

        var isThereAnySetMarker = setMarkersInCurrentTick.Count != 0;
        var isThereAnyCheckMarker = checkMarkersInCurrentTick.Count != 0;

        if (!isThereAnySetMarker && !isThereAnyCheckMarker)
        {
            return;
        }

        if (setMarkersInCurrentTick.Count > 1 || checkMarkersInCurrentTick.Count > 1)
        {
            Debug.LogError("More than one marker on the same tick!");
        }

        if (isThereAnyCheckMarker && isThereAnySetMarker)
        {
            var setMarker = setMarkersInCurrentTick[0];
            var checkMarker = checkMarkersInCurrentTick[0];

            if (setMarker.Id == checkMarker.Id)
            {
                GameEvents.RaiseGoodResponse(checkMarker.GetGoodResponse(), checkMarker.SenderPerson);
                Score += checkMarker.Points;
                return;
            }

            GameEvents.RaiseBadResponse(checkMarker.GetBadResponse(gameTick), checkMarker.SenderPerson);
            GameEvents.RaiseBadResponse(GetMarker(setMarker).GetBadResponse(gameTick), checkMarker.SenderPerson);
        }
        else if (isThereAnyCheckMarker)
        {
            GameEvents.RaiseBadResponse(checkMarkersInCurrentTick[0].GetBadResponse(gameTick),checkMarkersInCurrentTick[0].SenderPerson);
        }
        else
        {
            var checkMarker = GetMarker(setMarkersInCurrentTick[0]);
            GameEvents.RaiseBadResponse(checkMarker.GetBadResponse(gameTick),checkMarker.SenderPerson);
        }
    }

    private CheckMarker GetMarker(SetMarker setMarker) => allCheckMarkers.FirstOrDefault(m => m.Id == setMarker.Id);
    
    public float CalculateFinalScore()
    {
        return (float)Score / allCheckMarkers.Sum(c => c.Points);
    }
}