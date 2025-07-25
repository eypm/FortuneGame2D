using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPopup : MonoBehaviour
{
    private UIPopup[] activePopups;
    [NonSerialized] public PopupType popupType;
    public Action onAction;

    private void Start()
    {
        AudioManager.GetInstance().PlaySound(AudioManager.Sounds.popupOpen);
    }

    protected void ClosePopup()
    {
        List<UIPopup> popups = UIManager.activePopups;
        UIPopup uiPopup = popups.Find(x => x.popupType == popupType);
        popups.Remove(uiPopup);
        Destroy(uiPopup.gameObject);
    }


    protected void CloseAllPopup()
    {
        List<UIPopup> popups = UIManager.activePopups;
        for (int i = popups.Count - 1; i >= 0; i--)
        {
            UIPopup uiPopup = popups[i];
            Destroy(uiPopup.gameObject);
        }

        popups.Clear();
    }
}

public enum PopupType
{
    SpinPopup,
    LoseAllPopup,
    UpgradePopup,
    InfoPopup,
    StorePopup,
}