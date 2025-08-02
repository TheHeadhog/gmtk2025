using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace _Managers
{
    public class BullshitManager : SingletonPersistent<BullshitManager>
    {
        [SerializeField] private List<BullshitMarker> allBullshitMarkers;

        public void SetNewMarker(BullshitMarker marker) => allBullshitMarkers.Add(marker);

        public void OnEnable()
        {
            GameEvents.GameTimeChanged += CheckGameTick;
        }
        
        public void OnDisable()
        {
            GameEvents.GameTimeChanged -= CheckGameTick;
        }

        private void CheckGameTick(int gameTick)
        {
            var bullshitMarkers = allBullshitMarkers.Where(m => m.Timestamp.ToGameTick() == gameTick).ToList();

            foreach (var bullshitMarker in bullshitMarkers)
            {
                GameEvents.RaiseBullshitMarkerAppear(bullshitMarker);
            }
        }
    }
}