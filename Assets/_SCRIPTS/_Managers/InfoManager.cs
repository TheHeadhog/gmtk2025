using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace _Managers
{
    public class InfoManager : SingletonPersistent<InfoManager>
    {
        [SerializeField] private List<InfoMarker> allInfoMarkers;
        
        public void SetNewMarker(InfoMarker marker) => allInfoMarkers.Add(marker);

        private void OnEnable()
        {
            GameEvents.GameTimeChanged += CheckGameTick;
        }
        
        private void OnDisable()
        {
            GameEvents.GameTimeChanged -= CheckGameTick;
        }

        private void CheckGameTick(int gameTick)
        {
            var infoMarkers = allInfoMarkers.Where(m => m.Timestamp.ToGameTick() == gameTick).ToList();

            foreach (var infoMarker in infoMarkers)
            {
                GameEvents.RaiseInfoMarkerAppear(infoMarker);
            }
        }
    }
}