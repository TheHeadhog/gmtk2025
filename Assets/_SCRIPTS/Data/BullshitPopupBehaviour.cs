using System;
using System.Collections.Generic;
using DefaultNamespace;
using Helpers;
using UnityEngine;

public class BullshitPopupBehaviour : MonoBehaviour
{
    private List<BullshitPopupType> allPopupTypes;
    
    [SerializeField] private Popup AndreaPopup;
    [SerializeField] private Popup BossCakePopup;
    [SerializeField] private Popup HamsterPopup;
    [SerializeField] private Popup MilkologistPopup;
    [SerializeField] private Popup SensoryBoothPopup;
    [SerializeField] private Popup SexySinglesPopup;
    [SerializeField] private Popup StrongCalendarPopup;
    [SerializeField] private Popup GiftCardPopup;
    [SerializeField] private Popup JohnAiPopup;
    [SerializeField] private Popup ShitCoinPopup;

    private void Start()
    {
        foreach (var type in Enum.GetValues(typeof(BullshitPopupType)))
        {
            allPopupTypes.Add((BullshitPopupType)type);
        }
        GameEvents.OnBullshitMarkerAppear += ProcessBullshitPopupRequest;
    }

    private void OnDestroy()
    {
        GameEvents.OnBullshitMarkerAppear -= ProcessBullshitPopupRequest;
    }

    private void ProcessBullshitPopupRequest(BullshitMarker marker)
    {
        if (marker.Feature != Channel.BSPOPUP) return;
        
        var popupType = allPopupTypes[0]; // Risk of there not being an element is mitigated through design
        allPopupTypes.RemoveAt(0);

        switch (popupType)
        {
            case BullshitPopupType.ANDREA:
                AndreaPopup.OpenPopup();
                break;
            case BullshitPopupType.BOSS_CAKE:
                BossCakePopup.OpenPopup();
                break;
            case BullshitPopupType.HAMSTER:
                HamsterPopup.OpenPopup();
                break;
            case BullshitPopupType.MILKOLOGIST:
                MilkologistPopup.OpenPopup();
                break;
            case BullshitPopupType.SENSORY_BOOTH:
                SensoryBoothPopup.OpenPopup();
                break;
            case BullshitPopupType.SEXY_SINGLES:
                SexySinglesPopup.OpenPopup();
                break;
            case BullshitPopupType.STRONG_CALENDAR:
                StrongCalendarPopup.OpenPopup();
                break;
            case BullshitPopupType.GIFT_CARD:
                GiftCardPopup.OpenPopup();
                break;
            case BullshitPopupType.JOHN_AI:
                JohnAiPopup.OpenPopup();
                break;
            case BullshitPopupType.SHIT_COIN:
                ShitCoinPopup.OpenPopup();
                break;
        }
    }
}

public enum BullshitPopupType
{
    ANDREA,
    BOSS_CAKE,
    HAMSTER,
    MILKOLOGIST,
    SENSORY_BOOTH,
    SEXY_SINGLES,
    STRONG_CALENDAR,
    GIFT_CARD,
    JOHN_AI,
    SHIT_COIN
}
